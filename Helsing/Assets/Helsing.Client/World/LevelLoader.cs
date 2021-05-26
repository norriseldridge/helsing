namespace Helsing.Client.World
{
    public static class LevelLoader
    {
        public static void Load(string scene) => UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}