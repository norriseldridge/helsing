using UnityEngine;

namespace Helsing.Client.Common
{
    public class DontDestroy : MonoBehaviour
    {
        void Start() => DontDestroyOnLoad(gameObject);
    }
}