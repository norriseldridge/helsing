using Helsing.Client.UI.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helsing.Client.UI
{
    public class DeadPopupController : MonoBehaviour, IDeadPopup
    {
        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public void OnClickRetry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Update()
        {
            if (Visible)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    OnClickRetry();
            }
        }
    }
}