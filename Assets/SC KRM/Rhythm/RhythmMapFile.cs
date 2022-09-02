using SCKRM.Easing;
using SCKRM.Json;
using System.Collections.Generic;

namespace SCKRM.Rhythm
{
    public interface IBeatValuePairList<TValue> : IBeatValuePairList<TValue, IBeatValuePair<TValue>> { }

    public interface IBeatValuePairList<TValue, TPair> : IList<TPair> where TPair : IBeatValuePair<TValue>
    {
        TValue GetValue();
        TValue GetValue(double currentBeat);

        TValue GetValue(double currentBeat, out double beat, out bool isValueChanged);
    }



    public interface IBeatValuePair<TValue>
    {
        double beat { get; set; }
        TValue value { get; set; }
    }

    public interface IBeatValuePairAni<TValue> : IBeatValuePair<TValue>
    {
        double length { get; set; }
        EasingFunction.Ease easingFunction { get; set; }
    }
}
