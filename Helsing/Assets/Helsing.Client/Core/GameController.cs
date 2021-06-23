using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Core.Api;
using Helsing.Client.Entity;
using Helsing.Client.Entity.Api;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.Extensions;
using Helsing.Client.Item.Api;
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
        IPlayerController playerController;
        IInventory playerInventory;

        [Inject]
        void Inject(IMessageBroker broker,
            IDeadPopup deadPopup,
            IPlayerController playerController,
            IInventory playerInventory) =>
            (this.broker, this.deadPopup, this.playerController, this.playerInventory) = (broker, deadPopup, playerController, playerInventory);

        private void Start()
        {
            deadPopup.Visible = false;

            // listen for tile movers
            broker.Receive<TileMoverMovedMessage>()
                .Subscribe(OnTileMoverMoved)
                .AddTo(this);

            // listen for player dead
            playerController.Living
                .LivesAsObservable
                .Where(l => l <= 0)
                .Subscribe(_ => OnPlayerDied())
                .AddTo(this);

            // add everything else into another group
            var otherGroup = new TurnTakerGroup();
            var turnTakers = FindObjectsOfType<MonoBehaviour>().OfType<ITurnTaker>().Where(t => !(t is IPlayerController));
            foreach (var turnTaker in turnTakers)
                otherGroup.TurnTakers.Add(turnTaker);
            turnTakerGroups.Add(otherGroup);

            // add the player into a group
            var playerGroup = new TurnTakerGroup();
            playerGroup.TurnTakers.Add(playerController);
            turnTakerGroups.Add(playerGroup);
        }

        private void FixedUpdate()
        {
            if (!isPerformingTurns)
            {
                PerformTurns();
            }
        }

        private void OnTileMoverMoved(TileMoverMovedMessage movedMessage) =>
            ResolveCombatAtTile(movedMessage.tileMover.CurrentTile.Value);

        private void OnPlayerDied() =>
            deadPopup.Visible = true;

        private async void PerformTurns()
        {
            isPerformingTurns = true;
            await turnTakerGroups.AsyncForEach(async t => await t.TakeTurn());
            await Task.Delay(550);
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
            var player = livings.Where(l => l.GetComponent<IPlayerController>() != null).FirstOrDefault();
            if (player == null)
                return;

            var playerLiving = player.GetComponent<ILiving>();
            var playerMover = player.GetComponent<ITileMover>();

            var toDealDamage = new List<ILiving>();
            var enemies = livings.Where(l => l.GetComponent<IEnemy>() != null);
            foreach (var enemyGo in enemies)
            {
                var enemyLiving = enemyGo.GetComponent<ILiving>();
                var enemyMover = enemyGo.GetComponent<ITileMover>();
                if (playerMover.CurrentTile.Value == enemyMover.CurrentTile.Value)
                {
                    var enemy = enemyGo.GetComponent<IEnemy>();
                    try
                    {
                        // if we have the item, kill the enemy
                        playerInventory.RemoveItem(enemy.KillItem, 1);
                        toDealDamage.Add(enemyLiving);
                    }
                    catch
                    {
                        // if not, kill the player
                        toDealDamage.Add(playerLiving);
                    }
                }
            }

            foreach (var living in toDealDamage)
                living.DealDamage();
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
    }
}