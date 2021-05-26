using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Helsing.Client.UI
{
    public class Message : MonoBehaviour
    {
        [SerializeField]
        float lifeTime;

        [SerializeField]
        float fadeSpeed;

        [SerializeField]
        Text text;

        public string Text
        {
            get => text.text;
            set => text.text = value;
        }

        private void Start() => StartCoroutine(WaitThenDestroy());

        IEnumerator WaitThenDestroy()
        {
            yield return new WaitForSeconds(lifeTime);

            var color = text.color;
            while (color.a > 0.0f)
            {
                color.a -= fadeSpeed * Time.deltaTime;
                text.color = color;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}