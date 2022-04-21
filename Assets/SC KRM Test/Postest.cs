using SCKRM;
using SCKRM.Input;
using SCKRM.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postest : MonoBehaviour
{
    Vector2 rotation = Vector2.zero;
    //int nextBeat = 0;

    SoundPlayer soundPlayer;

    void Update()
    {
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

        /*if (Input.GetKeyDown(KeyCode.Space))
            RhythmManager.isBeatPlay = !RhythmManager.isBeatPlay;

        float currentBeat = (soundPlayer.time - 1.18f) * (171f / 60f);
        RhythmManager.bpm = 171;
        while (currentBeat >= nextBeat)
        {
            nextBeat++;
            RhythmManager.OneBeatInvoke();
        }*/
    }
}
