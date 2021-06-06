using UnityEngine;

namespace Helsing.Client.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        float speed;

        [SerializeField]
        Transform target;

        private void Update()
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    target.position + (Vector3.back * 10),
                    Time.deltaTime * speed);
            }
        }
    }
}
