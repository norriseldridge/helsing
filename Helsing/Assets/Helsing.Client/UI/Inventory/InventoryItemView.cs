using UnityEngine;
using UnityEngine.UI;

namespace Helsing.Client.UI.Inventory
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField]
        Image image;

        [SerializeField]
        Text countText;

        public void DisplayItem(Sprite sprite, int count)
        {
            image.sprite = sprite;
            UpdateCount(count);
        }

        public void UpdateCount(int count)
        {
            countText.text = count.ToString();
            gameObject.SetActive(count > 0);
        }
    }
}