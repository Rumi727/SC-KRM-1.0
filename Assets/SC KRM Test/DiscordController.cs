using System;
using UnityEngine;
using DiscordPresence;
using SCKRM;

public class DiscordController : MonoBehaviour
{
    long startTime = -1;
    //float timer = 0;

    void Awake()
    {
        startTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        string version;
#if UNITY_EDITOR
        version = $"{Kernel.productName} {Kernel.version} | Unity {Kernel.unityVersion}";
#else
        version = $"{Kernel.productName} {Kernel.version}";
#endif

        PresenceManager.UpdatePresence("Simsimhan Chobo Kernel Manager", "Rich Presence Test", startTime, PresenceManager.instance.presence.endTimestamp, PresenceManager.instance.presence.largeImageKey, version);
    }
}