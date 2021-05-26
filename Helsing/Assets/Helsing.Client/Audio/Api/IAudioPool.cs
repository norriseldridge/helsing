using UnityEngine;

namespace Helsing.Client.Audio.Api
{
    public interface IAudioPool
    {
        AudioSource Next();
    }
}