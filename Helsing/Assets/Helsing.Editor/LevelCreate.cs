using UnityEditor;
using UnityEditor.SceneManagement;

namespace Helsing.Editor
{
    public static class LevelCreate
    {
        const string TEMPLATE = "Assets/Scenes/LevelTemplate.unity";

        [MenuItem("Helsing/Scene/Create Level")]
        public static void CreateLevel()
        {
            string file = EditorUtility.SaveFilePanelInProject("Create Level",
                "NewLevel",
                "unity",
                "Create Level",
                "Assets/Scenes");

            if (file == null)
                return;

            FileUtil.CopyFileOrDirectory(TEMPLATE, file);
            AssetDatabase.Refresh();
            EditorSceneManager.OpenScene(file);
        }
    }
}
