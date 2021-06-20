using UnityEngine;

namespace Helsing.Client.Common
{
    [ExecuteInEditMode]
    public class Reparent : MonoBehaviour
    {
        // TODO how do we get a "tag" choice here instead?
        [SerializeField]
        string parentTag;

#if UNITY_EDITOR
        private Transform reParent;

        private void Update()
        {
            if (gameObject.scene.name == gameObject.name)
                return;

            if (reParent == null)
                reParent = GameObject.FindGameObjectWithTag(parentTag)?.transform;

            if (transform.parent != reParent && reParent != null)
                transform.SetParent(reParent);
        }
#endif
    }
}
