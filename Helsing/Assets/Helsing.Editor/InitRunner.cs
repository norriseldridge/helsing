using UnityEditor.SceneManagement;
using UnityEngine;

namespace Helsing.Editor
{
    public static class InitRunner
    {
        const string INIT_SCENE = "Init";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforePlay() => EditorSceneManager.LoadScene(INIT_SCENE);
    }
}