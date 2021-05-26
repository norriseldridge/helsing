using System.Collections.Generic;
using Helsing.Client.Audio.Api;
using UnityEngine;
using UnityEngine.Audio;

namespace Helsing.Client.Audio
{
    public class AudioPool : MonoBehaviour, IAudioPool
    {
        [SerializeField]
        [Min(1)]
        int maxSize;

        [SerializeField]
        AudioMixerGroup mixerGroup;

        Queue<AudioSource> pool = new Queue<AudioSource>();

        public AudioSource Next()
        {
            AudioSource temp;
            if (pool.Count < maxSize)
            {
                temp = gameObject.AddComponent<AudioSource>();
                temp.outputAudioMixerGroup = mixerGroup;
            }
            else
            {
                temp = pool.Dequeue();
            }

            temp.pitch = 1;
            temp.volume = 1;
            pool.Enqueue(temp);
            return temp;
        }
    }
}