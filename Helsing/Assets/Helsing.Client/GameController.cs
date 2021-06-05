using System.Collections.Generic;
using System.Linq;
using Helsing.Client.Api;
using Helsing.Client.Entity.Player.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client
{
    public class GameController : MonoBehaviour
    {
        bool isPerformingTurns = false;
        List<TurnTakerGroup> turnTakerGroups = new List<TurnTakerGroup>();
        IPlayerController playController;

        [Inject]
        void Inject(IPlayerController playController) =>
            this.playController = playController;

        private void Start()
        {
            // add everything else into another group
            var otherGroup = new TurnTakerGroup();
            var turnTakers = FindObjectsOfType<MonoBehaviour>().OfType<ITurnTaker>().Where(t => !(t is IPlayerController));
            foreach (var turnTaker in turnTakers)
            {
                otherGroup.TurnTakers.Add(turnTaker);
            }
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

        private async void PerformTurns()
        {
            isPerformingTurns = true;
            foreach (var turnTakerGroup in turnTakerGroups)
                await turnTakerGroup.TakeTurn();
            isPerformingTurns = false;
        }
    }
}