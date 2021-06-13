using Helsing.Client.World;using UnityEngine;namespace Helsing.Client{    public class GameInitializer : MonoBehaviour    {        private void Start()        {
            // TODO load game state?

            // TODO load main menu?

            // TODO don't do this
#if UNITY_EDITOR            var currentScene = PlayerPrefs.GetString("currentScene", "SampleScene");            LevelLoader.Load(currentScene);
#else
            LevelLoader.Load("SampleScene");
#endif        }

#if UNITY_EDITOR        [UnityEditor.InitializeOnLoadMethod]        private static void OnEditMode()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= OnExitingEditMode;
            UnityEditor.EditorApplication.playModeStateChanged += OnExitingEditMode;
        }        private static void OnExitingEditMode(UnityEditor.PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case UnityEditor.PlayModeStateChange.ExitingEditMode:
                    var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                    PlayerPrefs.SetString("currentScene", currentScene);
                    break;
            }
        }
#endif    }}