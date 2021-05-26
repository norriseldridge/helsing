using UnityEngine;

namespace Helsing.Client.Item
{
    public class ItemView : MonoBehaviour
    {
        public bool FlipX
        {
            get => flipX;
            set
            {
                flipX = value;

                var scaleX = flipX ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x);
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

                var x = flipX ? Mathf.Abs(transform.localPosition.x) : -Mathf.Abs(transform.localPosition.x);
                transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            }
        }
        bool flipX;
    }
}