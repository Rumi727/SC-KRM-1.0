using IngameDebugConsole;
using SCKRM.NBS;
using SCKRM.Resource;
using SCKRM.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Command
{
    public class Command
    {
        [ConsoleMethod("GameSpeed", "")] public static float GetGameSpeed() => Kernel.gameSpeed;
        [ConsoleMethod("GameSpeed", "")] public static void SetGameSpeed(float value) => Kernel.gameSpeed = value;

        [ConsoleMethod("StandardFPS", "")] public static float GetStandardFPS() => Kernel.Data.standardFPS;
        [ConsoleMethod("StandardFPS", "")] public static void SetStandardFPS(float value) => Kernel.Data.standardFPS = value;

        [ConsoleMethod("AllRefresh", "")] public static void AllRefresh(bool onlyText = false) => Kernel.AllRefresh(onlyText).Forget();


        #region Sound
        [ConsoleMethod("PlaySound", "")]
        public static SoundObject PlaySound(string sound, float volume, bool loop, float pitch, float tempo, float panStereo)
        {
            string nameSpace = ResourceManager.GetNameSpace(sound, out sound);
            return SoundManager.PlaySound(sound, nameSpace, volume, loop, pitch, tempo, panStereo);
        }

        [ConsoleMethod("PlaySound", "")]
        public static SoundObject PlaySound(string sound, float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0, float minDistance = 0, float maxDistance = 16, float x = 0, float y = 0, float z = 0)
        {
            string nameSpace = ResourceManager.GetNameSpace(sound, out sound);
            return SoundManager.PlaySound(sound, nameSpace, volume, loop, pitch, tempo, panStereo, false, minDistance, maxDistance, null, x, y, z);
        }

        [ConsoleMethod("StopSound", "")]
        public static void StopSound(string sound, bool all = false)
        {
            string nameSpace = ResourceManager.GetNameSpace(sound, out sound);
            SoundManager.StopSound(sound, nameSpace, all);
        }

        [ConsoleMethod("StopSoundAll", "")] public static void StopSoundAll() => SoundManager.StopSoundAll();
        [ConsoleMethod("StopSoundAll", "")] public static void StopSoundAll(bool bgm) => SoundManager.StopSoundAll(bgm);



        [ConsoleMethod("PlayNBS", "")]
        public static NBSPlayer PlayNBS(string nbs, float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0)
        {
            string nameSpace = ResourceManager.GetNameSpace(nbs, out nbs);
            return SoundManager.PlayNBS(nbs, nameSpace, volume, loop, pitch, tempo, panStereo);
        }

        [ConsoleMethod("PlayNBS", "")]
        public static NBSPlayer PlayNBS(string nbs, float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0, float minDistance = 0, float maxDistance = 48, float x = 0, float y = 0, float z = 0)
        {
            string nameSpace = ResourceManager.GetNameSpace(nbs, out nbs);
            return SoundManager.PlayNBS(nbs, nameSpace, volume, loop, pitch, tempo, panStereo, false, minDistance, maxDistance, null, x, y, z);
        }

        [ConsoleMethod("StopNBS", "")]
        public static void StopNBS(string nbs, bool all = false)
        {
            string nameSpace = ResourceManager.GetNameSpace(nbs, out nbs);
            SoundManager.StopNBS(nbs, nameSpace, all);
        }

        [ConsoleMethod("StopNBSAll", "")] public static void StopNBSAll() => SoundManager.StopNBSAll();
        #endregion
    }
}