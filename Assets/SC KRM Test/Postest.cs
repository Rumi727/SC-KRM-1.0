using SCKRM;
using SCKRM.Input;
using SCKRM.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Postest : MonoBehaviour
{
    Vector2 rotation = Vector2.zero;

    SoundPlayer soundPlayer;
    Map map;
    public Text text;

    public GameObject test;

    void Awake()
    {
        soundPlayer = SoundManager.PlaySound("grateful_friends", "school-live", 0.25f, true, 1, 1, 0);
        map = new Map(new MapInfo(), new MapEffect());
        map.info.offset = 1.545;
        map.effect.bpm.Add(new BeatValuePair<double>(0, 171));
        map.effect.dropPart.Add(new BeatValuePair<bool>(0, false));
        map.effect.dropPart.Add(new BeatValuePair<bool>(161, true));
        map.effect.dropPart.Add(new BeatValuePair<bool>(288, false));
        map.effect.dropPart.Add(new BeatValuePair<bool>(429, true));
        map.effect.dropPart.Add(new BeatValuePair<bool>(556, false));
        map.effect.dropPart.Add(new BeatValuePair<bool>(605, true));
        map.effect.dropPart.Add(new BeatValuePair<bool>(732, false));

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

        /*map.effect.test.Add(new BeatValuePairAni<double>(0, 0, 0, EasingFunction.Ease.Linear));
        bool temp = false;
        for (int i = 0; i < 1000; i += 4)
        {
            if (temp)
                map.effect.test.Add(new BeatValuePairAni<double>(i, 0, 3, EasingFunction.Ease.EaseInOutBounce));
            else
                map.effect.test.Add(new BeatValuePairAni<double>(i, 1, 3, EasingFunction.Ease.EaseInOutBounce));

            temp = !temp;
        }*/
    }

    void Update()
    {
        if (!RhythmManager.isPlaying)
            RhythmManager.Play(soundPlayer, map);

        float speed;
        if (InputManager.GetKey(KeyCode.LeftControl, InputType.Alway))
            speed = 0.25f * Kernel.fpsUnscaledDeltaTime;
        else
            speed = 0.125f * Kernel.fpsUnscaledDeltaTime;

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

        //test.transform.position = new Vector2((float)RhythmManager.map.effect.test.GetValue(), 0);
    }
}
