using Helsing.Client.UI.Api;
using UnityEngine;
using UnityEngine.UI;

namespace Helsing.Client.UI
{
    public class PromptMessage : MonoBehaviour, IPromptMessage
    {
        [SerializeField]
        Text text;

        [SerializeField]
        Image backing;

        string currentMessage;

        public void SetMessage(string message) => currentMessage = message;

        void Update()
        {
            if (currentMessage != null)
            {
                text.text = currentMessage;
                Visible = true;
                currentMessage = null;
            }
            else
            {
                Visible = false;
            }
        }

        bool Visible
        {
            set => text.enabled = backing.enabled = value;
        }
    }
}
