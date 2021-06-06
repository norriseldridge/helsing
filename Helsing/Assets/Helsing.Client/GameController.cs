using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Api;
using Helsing.Client.Audio.Api;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.World.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client
{
    public class GameController : MonoBehaviour
    {
        bool isPerformingTurns = false;
        List<TurnTakerGroup> turnTakerGroups = new List<TurnTakerGroup>();
        ITileMap tileMap;
        IEnemyBlackboard enemyBlackboard;
        IPlayerController playController;

        [Inject]
        void Inject(ITileMap tileMap,
            IEnemyBlackboard enemyBlackboard,
            IPlayerController playController) =>
            (this.tileMap, this.enemyBlackboard, this.playController) = (tileMap, enemyBlackboard, playController);

        private void Start()
        {
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
                ResolveCombat();
            }
            isPerformingTurns = false;
        }

        private void ResolveCombat()
        {
            List<GameObject> livings = GetAllLivingGameObjects();
            if (livings.Count > 1)
            {
                DealDamage(livings);
                CleanUpDead(livings);
            }
        }

        private List<GameObject> GetAllLivingGameObjects()
        {
            List<GameObject> livings = new List<GameObject>();
            foreach (var tile in tileMap.Tiles)
            {
                var gameObjects = tile.GetGameObjectsOnTile();
                foreach (var g in gameObjects)
                {
                    var living = g.GetComponent<ILiving>();
                    if (living != null)
                        livings.Add(g);
                }
            }
            return livings;
        }

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
                        var dist = Vector2.Distance(livings[i].transform.position, livings[j].transform.position);
                        if (dist < 1f)
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