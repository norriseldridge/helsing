using Helsing.Client.World;
using UnityEngine;

namespace Helsing.Client
{
    public class GameInitializer : MonoBehaviour
    {
        private void Start()
        {
            // TODO load game state?

            // TODO load main menu?

            // TODO don't do this
            LevelLoader.Load("SampleScene");
        }
    }
}