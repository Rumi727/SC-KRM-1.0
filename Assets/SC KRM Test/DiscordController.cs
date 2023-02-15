using System;
using UnityEngine;
using DiscordPresence;
using SCKRM;
using SCKRM.Discord;

public class DiscordController : MonoBehaviour
{
    long startTime = -1;
    //float timer = 0;

    void Awake() => startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
    void Update()
    {
        string version;
#if UNITY_EDITOR
        version = $"{Kernel.productName} {Kernel.version} | Unity {Kernel.unityVersion}";
#else
        version = $"{Kernel.productName} {Kernel.version}";
#endif

        if (DiscordManager.presence != null)
            DiscordManager.UpdatePresence("Simsimhan Chobo Kernel Manager", "Rich Presence Test", startTime, DiscordManager.presence.endTimestamp, DiscordManager.presence.largeImageKey, version);
    }
}