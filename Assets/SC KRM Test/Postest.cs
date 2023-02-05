using SCKRM;
using SCKRM.Input;
using SCKRM.Rhythm;
using SCKRM.Sound;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Postest : MonoBehaviour
{
    Vector2 rotation = Vector2.zero;

    SoundPlayer soundPlayer;
    public TMP_Text text;

    public GameObject test;

    void Start()
    {
        /*soundPlayer = SoundManager.PlaySound("grateful_friends", "school-live", 0.25f, true, 1, 1, 0);
        rhythmMap = new RhythmMap();

        rhythmMap.info.offset = 1.194;
        rhythmMap.globalEffect.bpm.Add(new BeatValuePair<double>(0, 171));
        rhythmMap.globalEffect.yukiMode.Add(new BeatValuePair<bool>(0, false));
        rhythmMap.globalEffect.yukiMode.Add(new BeatValuePair<bool>(161, true));
        rhythmMap.globalEffect.yukiMode.Add(new BeatValuePair<bool>(288, false));
        rhythmMap.globalEffect.yukiMode.Add(new BeatValuePair<bool>(429, true));
        rhythmMap.globalEffect.yukiMode.Add(new BeatValuePair<bool>(556, false));
        rhythmMap.globalEffect.yukiMode.Add(new BeatValuePair<bool>(605, true));
        rhythmMap.globalEffect.yukiMode.Add(new BeatValuePair<bool>(732, false));*/

        /*bool temp = false;
        for (int i = 4; i < 1000; i += 4)
        {
            if (temp)
                map.effect.bpm.Add(new BeatValuePair<double>(i, 171));
            else
            {
                map.effect.bpm.Add(new BeatValuePair<double>(i, 342));
                i += 4;
            }

            temp = !temp;
        }*/

        /*mapEffect.test.Add(new BeatValuePairAni<double>(-3, -2, 0, EasingFunction.Ease.Linear));
        mapEffect.test.Add(new BeatValuePairAni<double>(-3, -1, 1, EasingFunction.Ease.Linear));
        mapEffect.test.Add(new BeatValuePairAni<double>(-2, -2, 1, EasingFunction.Ease.Linear));
        mapEffect.test.Add(new BeatValuePairAni<double>(-1, 2, 1, EasingFunction.Ease.Linear));
        bool temp = false;
        for (int i = 0; i < 1000; i += 4)
        {
            if (temp)
                mapEffect.test.Add(new BeatValuePairAni<double>(i, 1, 3, EasingFunction.Ease.EaseInOutElastic));
            else
                mapEffect.test.Add(new BeatValuePairAni<double>(i, 0, 3, EasingFunction.Ease.EaseInOutElastic));

            temp = !temp;
        }*/
    }

    void Update()
    {
        /*if (!RhythmManager.isPlaying)
            RhythmManager.Play(soundPlayer, rhythmMap);*/

        float speed;
        if (InputManager.GetKey(KeyCode.LeftControl, InputType.Alway))
            speed = 0.25f * Kernel.fpsUnscaledSmoothDeltaTime;
        else
            speed = 0.125f * Kernel.fpsUnscaledSmoothDeltaTime;

        {
            Vector3 rotation = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

            if (InputManager.GetKey(KeyCode.A, InputType.Alway))
                transform.position -= transform.right * speed;
            if (InputManager.GetKey(KeyCode.D, InputType.Alway))
                transform.position += transform.right * speed;
            if (InputManager.GetKey(KeyCode.S, InputType.Alway))
                transform.position -= transform.forward * speed;
            if (InputManager.GetKey(KeyCode.W, InputType.Alway))
                transform.position += transform.forward * speed;
            if (InputManager.GetKey(KeyCode.LeftShift, InputType.Alway))
                transform.position -= transform.up * speed;
            if (InputManager.GetKey(KeyCode.Space, InputType.Alway))
                transform.position += transform.up * speed;

            transform.localEulerAngles = rotation;
        }

        if (InputManager.GetKey(KeyCode.Mouse0, InputType.Alway))
        {
            Vector2 rotation = InputManager.GetMouseDelta() * 0.5f;
            this.rotation += new Vector2(-rotation.y, rotation.x);
        }

        transform.localEulerAngles = rotation;
        text.text = RhythmManager.currentBeat.ToString();

        //test.transform.position = new Vector2((float)mapEffect.test.GetValue(), 0);
    }
}
