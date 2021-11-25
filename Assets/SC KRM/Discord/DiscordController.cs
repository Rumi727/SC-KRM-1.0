using System;
using UnityEngine;
using DiscordPresence;
using SCKRM;

namespace MarpleRPG.Discord
{
    public class DiscordController : PresenceManager
    {
        long startTime = -1;

        public override void Awake()
        {
            base.Awake();
            startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        void FixedUpdate()
        {
            string version;
#if UNITY_EDITOR
            version = $"{Kernel.productName} {Kernel.version} | Unity {Kernel.unityVersion}";
#else
            version = $"{Kernel.productName} {Kernel.version}";
#endif

            UpdatePresence("Simsimhan Chobo Kernel Manager", "Rich Presence Test", startTime, presence.endTimestamp, presence.largeImageKey, version);
        }

#region Var To Bar
        public static string IntToBar(int value, int max, int length)
        {
            string text = "";

            for (float i = 0.5f; i < length + 0.5f; i++)
            {
                if (value / max >= i / length)
                    text += "■";
                else
                {
                    if (value / max >= (i - 0.5f) / length)
                        text += "▣";
                    else
                        text += "□";
                }
            }
            return text;
        }

        public static string FloatToBar(double value, double max, int length)
        {
            string text = "";

            for (float i = 0.5f; i < length + 0.5f; i++)
            {
                if (value / max >= i / length)
                    text += "■";
                else
                {
                    if (value / max >= (i - 0.5f) / length)
                        text += "▣";
                    else
                        text += "□";
                }
            }
            return text;
        }

        public static string DoubleToBar(double value, double max, int length)
        {
            string text = "";

            for (float i = 0.5f; i < length + 0.5f; i++)
            {
                if (value / max >= i / length)
                    text += "■";
                else
                {
                    if (value / max >= (i - 0.5f) / length)
                        text += "▣";
                    else
                        text += "□";
                }
            }
            return text;
        }
#endregion
    }
}