using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    public sealed class GraphicBeatColor : UI
    {
        [SerializeField] float _alpha = 1;
        public float alpha { get => _alpha; set => _alpha = value; }

        [SerializeField] bool _dropPartMode = false;
        public bool dropPartMode => _dropPartMode;

        protected override void OnEnable()
        {
            if (dropPartMode)
                RhythmManager.oneBeatDropPart += ColorChange;
            else
                RhythmManager.oneBeat += ColorChange;
        }

        protected override void OnDisable()
        {
            if (dropPartMode)
                RhythmManager.oneBeatDropPart -= ColorChange;
            else
                RhythmManager.oneBeat -= ColorChange;
        }

        void Update()
        {
            if (RhythmManager.isPlaying)
                graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, graphic.color.a - 0.01f * RhythmManager.bpmFpsDeltaTime);
            else
                graphic.color = graphic.color.MoveTowards(new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0), 0.025f * Kernel.fpsUnscaledDeltaTime);
        }

        void ColorChange() => graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }
}
