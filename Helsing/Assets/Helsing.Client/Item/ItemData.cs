using Helsing.Client.Item.Api;
using UnityEngine;

namespace Helsing.Client.Item
{
    public class ItemData : ScriptableObject, IItemData
    {
        public int ID => id;
        [SerializeField] int id;

        public string DisplayName => displayName;
        [SerializeField] string displayName;

        public Sprite Sprite => sprite;
        [SerializeField] Sprite sprite;

#if UNITY_EDITOR
        static int itemId;

        [UnityEditor.InitializeOnLoadMethod]
        static void OnOpenEditor()
        {
            string[] assetPaths = UnityEditor.AssetDatabase.FindAssets("t:ItemData");
            itemId = assetPaths.Length;
        }

        [UnityEditor.MenuItem("Helsing/Data/Create Item")]
        static void Create()
        {
            ItemData asset = CreateInstance<ItemData>();
            asset.id = itemId;

            string file = UnityEditor.EditorUtility.SaveFilePanelInProject("Create Item",
                "NewItemData",
                "asset",
                "Create Item",
                "Assets/Data/Item");

            if (file == null)
                return;

            UnityEditor.AssetDatabase.CreateAsset(asset, file);
            UnityEditor.AssetDatabase.SaveAssets();

            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = asset;
        }
#endif
    }
}