using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Core.Api;
using Helsing.Client.Entity;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.UI.Api;
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
        IDeadPopup deadPopup;
        IPlayerController playController;

        [Inject]
        void Inject(IMessageBroker broker,
            IDeadPopup deadPopup,
            IPlayerController playController) =>
            (this.broker, this.deadPopup, this.playController) = (broker, deadPopup, playController);

        private void Start()
        {
            deadPopup.Visible = false;

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
            }
        }

        private void OnTileMoverMoved(ITileMover tileMover) =>
            ResolveCombatAtTile(tileMover.CurrentTile.Value);

        private void OnPlayerDied() =>
            deadPopup.Visible = true;

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
            foreach (var attacker in livings)
            {
                var attackLiving = attacker.GetComponent<ILiving>();
                var attackMover = attacker.GetComponent<ITileMover>();
                var attackEnemy = attacker.GetComponent<IEnemy>();

                foreach (var defender in livings)
                {
                    if (attacker == defender)
                        continue;

                    var defendLiving = defender.GetComponent<ILiving>();
                    var defendMover = defender.GetComponent<ITileMover>();
                    var defendEnemy = defender.GetComponent<IEnemy>();

                    if (attackEnemy != null && defendEnemy != null)
                        continue;

                    if (attackMover.CurrentTile.Value == defendMover.CurrentTile.Value)
                    {
                        toDealDamage.Add(attackLiving);
                        toDealDamage.Add(defendLiving);
                    }
                }
            }

            foreach (var toDamage in toDealDamage.Distinct())
                toDamage.DealDamage();
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