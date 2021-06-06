using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Core.Api;
using Helsing.Client.Entity;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Core
{
    public class GameController : MonoBehaviour
    {
        bool isPerformingTurns = false;
        List<TurnTakerGroup> turnTakerGroups = new List<TurnTakerGroup>();
        IMessageBroker broker;
        ITileMap tileMap;
        IEnemyBlackboard enemyBlackboard;
        IPlayerController playController;

        [Inject]
        void Inject(IMessageBroker broker,
            ITileMap tileMap,
            IEnemyBlackboard enemyBlackboard,
            IPlayerController playController) =>
            (this.broker, this.tileMap, this.enemyBlackboard, this.playController) = (broker, tileMap, enemyBlackboard, playController);

        private void Start()
        {
            // listen for tile movers
            broker.Receive<TileMoverMovedMessage>()
                .Subscribe(t => OnTileMoverMoved(t.tileMover))
                .AddTo(this);

            // listen for player dead
            playController.Living
                .LivesAsObservable
                .Where(l => l <= 0)
                .Subscribe(_ => OnPlayerDied())
                .AddTo(this);

            // add everything else into another group
            var otherGroup = new TurnTakerGroup();
            var turnTakers = FindObjectsOfType<MonoBehaviour>().OfType<ITurnTaker>().Where(t => !(t is IPlayerController));
            foreach (var turnTaker in turnTakers)
            {
                otherGroup.TurnTakers.Add(turnTaker);
            }
            otherGroup.TurnTakers.Add(new MinimumTurnTaker());
            turnTakerGroups.Add(otherGroup);

            // add the player into a group
            var playerGroup = new TurnTakerGroup();
            playerGroup.TurnTakers.Add(playController);
            turnTakerGroups.Add(playerGroup);
        }

        private void Update()
        {
            if (!isPerformingTurns)
            {
                PerformTurns();
                enemyBlackboard.Clear();
            }
        }

        private void OnTileMoverMoved(ITileMover tileMover) =>
            ResolveCombatAtTile(tileMover.CurrentTile.Value);

        private void OnPlayerDied()
        {
            Debug.Log("Player died...");
        }

        private async void PerformTurns()
        {
            isPerformingTurns = true;
            foreach (var turnTakerGroup in turnTakerGroups)
            {
                await turnTakerGroup.TakeTurn();
            }
            isPerformingTurns = false;
        }

        private void ResolveCombatAtTile(ITile tile)
        {
            List<GameObject> livings = GetAllLivingGameObjectsOnTile(tile);
            if (livings.Count > 1)
            {
                DealDamage(livings);
                CleanUpDead(livings);
            }
        }

        private List<GameObject> GetAllLivingGameObjectsOnTile(ITile tile) =>
            new List<GameObject>(TileMover.GetObjectsOnTile(tile).Where(g => g.GetComponent<ILiving>() != null));

        private void DealDamage(List<GameObject> livings)
        {
            // deal damage to all living objects that are "in combat"
            var toDealDamage = new List<ILiving>();
            for (var i = 0; i < livings.Count; ++i)
            {
                var iliving = livings[i].GetComponent<ILiving>();
                if (iliving.Lives <= 0 || toDealDamage.Contains(iliving))
                    continue;

                for (var j = 0; j < livings.Count; ++j)
                {
                    if (i != j)
                    {
                        var imover = livings[i].GetComponent<ITileMover>();
                        var jmover = livings[j].GetComponent<ITileMover>();

                        if (imover == null || jmover == null)
                        {
                            Debug.LogError($"One of the TurnTakers attempting to deal damage is not an ITileMover! {imover} {jmover}");
                            continue;
                        }

                        if (imover.CurrentTile.Value == jmover.CurrentTile.Value)
                        {
                            toDealDamage.Add(iliving);
                        }
                    }
                }
            }

            toDealDamage.ForEach(l => l.DealDamage());
        }

        private void CleanUpDead(List<GameObject> livings)
        {
            // remove any dead one from turn takers
            var deads = new List<GameObject>();
            foreach (var g in livings)
            {
                var living = g.GetComponent<ILiving>();
                if (living.Lives <= 0)
                {
                    var turnTaker = g.GetComponent<ITurnTaker>();
                    foreach (var turnTakerGroup in turnTakerGroups)
                    {
                        turnTakerGroup.TurnTakers.Remove(turnTaker);
                    }
                    deads.Add(g);
                }
            }

            // destroy the dead game objects
            if (deads.Count > 0)
            {
                while (deads.Count > 0)
                {
                    Destroy(deads[0]);
                    deads.RemoveAt(0);
                }
            }
        }

        private class MinimumTurnTaker : ITurnTaker
        {
            public Task TakeTurn() => Task.Delay(50);
        }
    }
}