/*
 * Created by C.J. Kimberlin
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2019
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * 
 * TERMS OF USE - EASING EQUATIONS
 * Open source under the BSD License.
 * Copyright (c)2001 Robert Penner
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE 
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 *
 * ============= Description =============
 *
 * Below is an example of how to use the easing functions in the file. There is a getting function that will return the function
 * from an enum. This is useful since the enum can be exposed in the editor and then the function queried during Start().
 * 
 * EasingFunction.Ease ease = EasingFunction.Ease.EaseInOutQuad;
 * EasingFunction.EasingFunc func = GetEasingFunction(ease;
 * 
 * double value = func(0, 10, 0.67f);
 * 
 * EasingFunction.EaseingFunc derivativeFunc = GetEasingFunctionDerivative(ease);
 * 
 * double derivativeValue = derivativeFunc(0, 10, 0.67f);
 */

using System;

namespace SCKRM.Easing
{
    public static class EasingFunction
    {
        public enum Ease
        {
            EaseInQuad = 0,
            EaseOutQuad,
            EaseInOutQuad,
            EaseInCubic,
            EaseOutCubic,
            EaseInOutCubic,
            EaseInQuart,
            EaseOutQuart,
            EaseInOutQuart,
            EaseInQuint,
            EaseOutQuint,
            EaseInOutQuint,
            EaseInSine,
            EaseOutSine,
            EaseInOutSine,
            EaseInExpo,
            EaseOutExpo,
            EaseInOutExpo,
            EaseInCirc,
            EaseOutCirc,
            EaseInOutCirc,
            Linear,
            Spring,
            EaseInBounce,
            EaseOutBounce,
            EaseInOutBounce,
            EaseInBack,
            EaseOutBack,
            EaseInOutBack,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic,
        }

        private const double NATURAL_LOG_OF_2 = 0.693147181f;

        //
        // Easing functions
        //

        public static double Linear(double start, double end, double value) => start.Lerp(end, value, true);

        public static double Spring(double start, double end, double value)
        {
            value = value.Clamp01();
            value = (value * Math.PI * (0.2f + 2.5f * value * value * value) * Math.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        public static double EaseInQuad(double start, double end, double value)
        {
            end -= start;
            return end * value * value + start;
        }

        public static double EaseOutQuad(double start, double end, double value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        public static double EaseInOutQuad(double start, double end, double value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end * 0.5f * value * value + start;
            value--;
            return -end * 0.5f * (value * (value - 2) - 1) + start;
        }

        public static double EaseInCubic(double start, double end, double value)
        {
            end -= start;
            return end * value * value * value + start;
        }

        public static double EaseOutCubic(double start, double end, double value)
        {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }

        public static double EaseInOutCubic(double start, double end, double value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end * 0.5f * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value + 2) + start;
        }

        public static double EaseInQuart(double start, double end, double value)
        {
            end -= start;
            return end * value * value * value * value + start;
        }

        public static double EaseOutQuart(double start, double end, double value)
        {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

        public static double EaseInOutQuart(double start, double end, double value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end * 0.5f * value * value * value * value + start;
            value -= 2;
            return -end * 0.5f * (value * value * value * value - 2) + start;
        }

        public static double EaseInQuint(double start, double end, double value)
        {
            end -= start;
            return end * value * value * value * value * value + start;
        }

        public static double EaseOutQuint(double start, double end, double value)
        {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }

        public static double EaseInOutQuint(double start, double end, double value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end * 0.5f * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value * value * value + 2) + start;
        }

        public static double EaseInSine(double start, double end, double value)
        {
            end -= start;
            return -end * Math.Cos(value * (Math.PI * 0.5f)) + end + start;
        }

        public static double EaseOutSine(double start, double end, double value)
        {
            end -= start;
            return end * Math.Sin(value * (Math.PI * 0.5f)) + start;
        }

        public static double EaseInOutSine(double start, double end, double value)
        {
            end -= start;
            return -end * 0.5f * (Math.Cos(Math.PI * value) - 1) + start;
        }

        public static double EaseInExpo(double start, double end, double value)
        {
            end -= start;
            return end * Math.Pow(2, 10 * (value - 1)) + start;
        }

        public static double EaseOutExpo(double start, double end, double value)
        {
            end -= start;
            return end * (-Math.Pow(2, -10 * value) + 1) + start;
        }

        public static double EaseInOutExpo(double start, double end, double value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return end * 0.5f * Math.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5f * (-Math.Pow(2, -10 * value) + 2) + start;
        }

        public static double EaseInCirc(double start, double end, double value)
        {
            end -= start;
            return -end * (Math.Sqrt(1 - value * value) - 1) + start;
        }

        public static double EaseOutCirc(double start, double end, double value)
        {
            value--;
            end -= start;
            return end * Math.Sqrt(1 - value * value) + start;
        }

        public static double EaseInOutCirc(double start, double end, double value)
        {
            value /= .5f;
            end -= start;
            if (value < 1)
                return -end * 0.5f * (Math.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end * 0.5f * (Math.Sqrt(1 - value * value) + 1) + start;
        }

        public static double EaseInBounce(double start, double end, double value)
        {
            end -= start;
            double d = 1f;
            return end - EaseOutBounce(0, end, d - value) + start;
        }

        public static double EaseOutBounce(double start, double end, double value)
        {
            value /= 1f;
            end -= start;
            if (value < (1 / 2.75f))
            {
                return end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        public static double EaseInOutBounce(double start, double end, double value)
        {
            end -= start;
            double d = 1f;
            if (value < d * 0.5f)
                return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else
                return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        public static double EaseInBack(double start, double end, double value)
        {
            end -= start;
            value /= 1;
            double s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        public static double EaseOutBack(double start, double end, double value)
        {
            double s = 1.70158f;
            end -= start;
            value = (value) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        public static double EaseInOutBack(double start, double end, double value)
        {
            double s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        public static double EaseInElastic(double start, double end, double value)
        {
            end -= start;

            double d = 1f;
            double p = d * .3f;
            double s;
            double a = 0;

            if (value == 0)
                return start;

            if ((value /= d) == 1)
                return start + end;

            if (a == 0f || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            return -(a * Math.Pow(2, 10 * (value -= 1)) * Math.Sin((value * d - s) * (2 * Math.PI) / p)) + start;
        }

        public static double EaseOutElastic(double start, double end, double value)
        {
            end -= start;

            double d = 1f;
            double p = d * .3f;
            double s;
            double a = 0;

            if (value == 0)
                return start;

            if ((value /= d) == 1)
                return start + end;

            if (a == 0f || a < Math.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            return (a * Math.Pow(2, -10 * value) * Math.Sin((value * d - s) * (2 * Math.PI) / p) + end + start);
        }

        public static double EaseInOutElastic(double start, double end, double value)
        {
            end -= start;

            double d = 1f;
            double p = d * .3f;
            double s;
            double a = 0;

            if (value == 0)
                return start;

            if ((value /= d * 0.5f) == 2)
                return start + end;

            if (a == 0f || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            if (value < 1)
                return -0.5f * (a * Math.Pow(2, 10 * (value -= 1)) * Math.Sin((value * d - s) * (2 * Math.PI) / p)) + start;
            return a * Math.Pow(2, -10 * (value -= 1)) * Math.Sin((value * d - s) * (2 * Math.PI) / p) * 0.5f + end + start;
        }

        //
        // These are derived functions that the motor can use to get the speed at a specific time.
        //
        // The easing functions all work with a normalized time (0 to 1) and the returned value here
        // reflects that. Values returned here should be divided by the actual time.
        //
        // TODO: These functions have not had the testing they deserve. If there is odd behavior around
        //       dash speeds then this would be the first place I'd look.

        public static double LinearD(double start, double end, double value)
        {
            return end - start;
        }

        public static double EaseInQuadD(double start, double end, double value)
        {
            return 2d * (end - start) * value;
        }

        public static double EaseOutQuadD(double start, double end, double value)
        {
            end -= start;
            return -end * value - end * (value - 2);
        }

        public static double EaseInOutQuadD(double start, double end, double value)
        {
            value /= .5;
            end -= start;

            if (value < 1)
            {
                return end * value;
            }

            value--;

            return end * (1 - value);
        }

        public static double EaseInCubicD(double start, double end, double value)
        {
            return 3d * (end - start) * value * value;
        }

        public static double EaseOutCubicD(double start, double end, double value)
        {
            value--;
            end -= start;
            return 3d * end * value * value;
        }

        public static double EaseInOutCubicD(double start, double end, double value)
        {
            value /= .5;
            end -= start;

            if (value < 1)
            {
                return (3d / 2d) * end * value * value;
            }

            value -= 2;

            return (3d / 2d) * end * value * value;
        }

        public static double EaseInQuartD(double start, double end, double value)
        {
            return 4d * (end - start) * value * value * value;
        }

        public static double EaseOutQuartD(double start, double end, double value)
        {
            value--;
            end -= start;
            return -4d * end * value * value * value;
        }

        public static double EaseInOutQuartD(double start, double end, double value)
        {
            value /= .5;
            end -= start;

            if (value < 1)
            {
                return 2d * end * value * value * value;
            }

            value -= 2;

            return -2d * end * value * value * value;
        }

        public static double EaseInQuintD(double start, double end, double value)
        {
            return 5d * (end - start) * value * value * value * value;
        }

        public static double EaseOutQuintD(double start, double end, double value)
        {
            value--;
            end -= start;
            return 5d * end * value * value * value * value;
        }

        public static double EaseInOutQuintD(double start, double end, double value)
        {
            value /= .5;
            end -= start;

            if (value < 1)
            {
                return (5d / 2d) * end * value * value * value * value;
            }

            value -= 2;

            return (5d / 2d) * end * value * value * value * value;
        }

        public static double EaseInSineD(double start, double end, double value)
        {
            return (end - start) * 0.5d * Math.PI * Math.Sin(0.5d * Math.PI * value);
        }

        public static double EaseOutSineD(double start, double end, double value)
        {
            end -= start;
            return (Math.PI * 0.5d) * end * Math.Cos(value * (Math.PI * 0.5d));
        }

        public static double EaseInOutSineD(double start, double end, double value)
        {
            end -= start;
            return end * 0.5d * Math.PI * Math.Sin(Math.PI * value);
        }
        public static double EaseInExpoD(double start, double end, double value)
        {
            return (10d * NATURAL_LOG_OF_2 * (end - start) * Math.Pow(2d, 10d * (value - 1)));
        }

        public static double EaseOutExpoD(double start, double end, double value)
        {
            end -= start;
            return 5d * NATURAL_LOG_OF_2 * end * Math.Pow(2d, 1d - 10d * value);
        }

        public static double EaseInOutExpoD(double start, double end, double value)
        {
            value /= .5;
            end -= start;

            if (value < 1)
            {
                return 5d * NATURAL_LOG_OF_2 * end * Math.Pow(2d, 10d * (value - 1));
            }

            value--;

            return (5d * NATURAL_LOG_OF_2 * end) / (Math.Pow(2d, 10d * value));
        }

        public static double EaseInCircD(double start, double end, double value)
        {
            return ((end - start) * value) / Math.Sqrt(1d - value * value);
        }

        public static double EaseOutCircD(double start, double end, double value)
        {
            value--;
            end -= start;
            return (-end * value) / Math.Sqrt(1d - value * value);
        }

        public static double EaseInOutCircD(double start, double end, double value)
        {
            value /= .5;
            end -= start;

            if (value < 1)
            {
                return (end * value) / (2d * Math.Sqrt(1d - value * value));
            }

            value -= 2d;

            return (-end * value) / (2d * Math.Sqrt(1d - value * value));
        }

        public static double EaseInBounceD(double start, double end, double value)
        {
            end -= start;
            double d = 1d;

            return EaseOutBounceD(0, end, d - value);
        }

        public static double EaseOutBounceD(double start, double end, double value)
        {
            value /= 1d;
            end -= start;

            if (value < (1d / 2.75))
            {
                return 2d * end * 7.5625 * value;
            }
            else if (value < (2 / 2.75))
            {
                value -= (1.5 / 2.75);
                return 2d * end * 7.5625 * value;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25 / 2.75);
                return 2d * end * 7.5625 * value;
            }
            else
            {
                value -= (2.625 / 2.75);
                return 2d * end * 7.5625 * value;
            }
        }

        public static double EaseInOutBounceD(double start, double end, double value)
        {
            end -= start;
            double d = 1d;

            if (value < d * 0.5)
            {
                return EaseInBounceD(0, end, value * 2d) * 0.5;
            }
            else
            {
                return EaseOutBounceD(0, end, value * 2d - d) * 0.5;
            }
        }

        public static double EaseInBackD(double start, double end, double value)
        {
            double s = 1.70158;

            return 3d * (s + 1d) * (end - start) * value * value - 2d * s * (end - start) * value;
        }

        public static double EaseOutBackD(double start, double end, double value)
        {
            double s = 1.70158;
            end -= start;
            value = (value) - 1;

            return end * ((s + 1d) * value * value + 2d * value * ((s + 1d) * value + s));
        }

        public static double EaseInOutBackD(double start, double end, double value)
        {
            double s = 1.70158;
            end -= start;
            value /= .5;

            if ((value) < 1)
            {
                s *= (1.525);
                return 0.5 * end * (s + 1) * value * value + end * value * ((s + 1d) * value - s);
            }

            value -= 2;
            s *= (1.525);
            return 0.5 * end * ((s + 1) * value * value + 2f * value * ((s + 1f) * value + s));
        }

        public static double EaseInElasticD(double start, double end, double value)
        {
            return EaseOutElasticD(start, end, 1d - value);
        }

        public static double EaseOutElasticD(double start, double end, double value)
        {
            end -= start;

            double d = 1d;
            double p = d * .3d;
            double s;
            double a = 0;

            if (a == 0d || a < Math.Abs(end))
            {
                a = end;
                s = p * 0.25d;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            return (a * Math.PI * d * Math.Pow(2d, 1d - 10d * value) *
                Math.Cos((2d * Math.PI * (d * value - s)) / p)) / p - 5d * NATURAL_LOG_OF_2 * a *
                Math.Pow(2d, 1d - 10d * value) * Math.Sin((2d * Math.PI * (d * value - s)) / p);
        }

        public static double EaseInOutElasticD(double start, double end, double value)
        {
            end -= start;

            double d = 1d;
            double p = d * .3d;
            double s;
            double a = 0;

            if (a == 0f || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Math.PI) * Math.Asin(end / a);
            }

            if (value < 1)
            {
                value -= 1;

                return -5d * NATURAL_LOG_OF_2 * a * Math.Pow(2d, 10d * value) * Math.Sin(2 * Math.PI * (d * value - 2d) / p) -
                    a * Math.PI * d * Math.Pow(2d, 10d * value) * Math.Cos(2 * Math.PI * (d * value - s) / p) / p;
            }

            value -= 1;

            return a * Math.PI * d * Math.Cos(2d * Math.PI * (d * value - s) / p) / (p * Math.Pow(2d, 10d * value)) -
                5d * NATURAL_LOG_OF_2 * a * Math.Sin(2d * Math.PI * (d * value - s) / p) / (Math.Pow(2d, 10d * value));
        }

        public static double SpringD(double start, double end, double value)
        {
            value = value.Clamp01();
            end -= start;

            // Damn... Thanks http://www.derivative-calculator.net/
            // TODO: And it's a little bit wrong
            return end * (6d * (1d - value) / 5d + 1d) * (-2.2 * Math.Pow(1d - value, 1.2) *
                Math.Sin(Math.PI * value * (2.5 * value * value * value + 0.2)) + Math.Pow(1d - value, 2.2) *
                (Math.PI * (2.5 * value * value * value + 0.2) + 7.5 * Math.PI * value * value * value) *
                Math.Cos(Math.PI * value * (2.5 * value * value * value + 0.2)) + 1) -
                6d * end * (Math.Pow(1 - value, 2.2) * Math.Sin(Math.PI * value * (2.5 * value * value * value + 0.2)) + value
                / 5d);

        }

        public delegate double Function(double s, double e, double v);

        /// <summary>
        /// Returns the function associated to the easingFunction enum. This value returned should be cached as it allocates memory
        /// to return.
        /// </summary>
        /// <param name="easingFunction">The enum associated with the easing function.</param>
        /// <returns>The easing function</returns>
        public static Function GetEasingFunction(Ease easingFunction)
        {
            if (easingFunction == Ease.EaseInQuad)
            {
                return EaseInQuad;
            }

            if (easingFunction == Ease.EaseOutQuad)
            {
                return EaseOutQuad;
            }

            if (easingFunction == Ease.EaseInOutQuad)
            {
                return EaseInOutQuad;
            }

            if (easingFunction == Ease.EaseInCubic)
            {
                return EaseInCubic;
            }

            if (easingFunction == Ease.EaseOutCubic)
            {
                return EaseOutCubic;
            }

            if (easingFunction == Ease.EaseInOutCubic)
            {
                return EaseInOutCubic;
            }

            if (easingFunction == Ease.EaseInQuart)
            {
                return EaseInQuart;
            }

            if (easingFunction == Ease.EaseOutQuart)
            {
                return EaseOutQuart;
            }

            if (easingFunction == Ease.EaseInOutQuart)
            {
                return EaseInOutQuart;
            }

            if (easingFunction == Ease.EaseInQuint)
            {
                return EaseInQuint;
            }

            if (easingFunction == Ease.EaseOutQuint)
            {
                return EaseOutQuint;
            }

            if (easingFunction == Ease.EaseInOutQuint)
            {
                return EaseInOutQuint;
            }

            if (easingFunction == Ease.EaseInSine)
            {
                return EaseInSine;
            }

            if (easingFunction == Ease.EaseOutSine)
            {
                return EaseOutSine;
            }

            if (easingFunction == Ease.EaseInOutSine)
            {
                return EaseInOutSine;
            }

            if (easingFunction == Ease.EaseInExpo)
            {
                return EaseInExpo;
            }

            if (easingFunction == Ease.EaseOutExpo)
            {
                return EaseOutExpo;
            }

            if (easingFunction == Ease.EaseInOutExpo)
            {
                return EaseInOutExpo;
            }

            if (easingFunction == Ease.EaseInCirc)
            {
                return EaseInCirc;
            }

            if (easingFunction == Ease.EaseOutCirc)
            {
                return EaseOutCirc;
            }

            if (easingFunction == Ease.EaseInOutCirc)
            {
                return EaseInOutCirc;
            }

            if (easingFunction == Ease.Linear)
            {
                return Linear;
            }

            if (easingFunction == Ease.Spring)
            {
                return Spring;
            }

            if (easingFunction == Ease.EaseInBounce)
            {
                return EaseInBounce;
            }

            if (easingFunction == Ease.EaseOutBounce)
            {
                return EaseOutBounce;
            }

            if (easingFunction == Ease.EaseInOutBounce)
            {
                return EaseInOutBounce;
            }

            if (easingFunction == Ease.EaseInBack)
            {
                return EaseInBack;
            }

            if (easingFunction == Ease.EaseOutBack)
            {
                return EaseOutBack;
            }

            if (easingFunction == Ease.EaseInOutBack)
            {
                return EaseInOutBack;
            }

            if (easingFunction == Ease.EaseInElastic)
            {
                return EaseInElastic;
            }

            if (easingFunction == Ease.EaseOutElastic)
            {
                return EaseOutElastic;
            }

            if (easingFunction == Ease.EaseInOutElastic)
            {
                return EaseInOutElastic;
            }

            return null;
        }

        /// <summary>
        /// Gets the derivative function of the appropriate easing function. If you use an easing function for position then this
        /// function can get you the speed at a given time (normalized).
        /// </summary>
        /// <param name="easingFunction"></param>
        /// <returns>The derivative function</returns>
        public static Function GetEasingFunctionDerivative(Ease easingFunction)
        {
            if (easingFunction == Ease.EaseInQuad)
            {
                return EaseInQuadD;
            }

            if (easingFunction == Ease.EaseOutQuad)
            {
                return EaseOutQuadD;
            }

            if (easingFunction == Ease.EaseInOutQuad)
            {
                return EaseInOutQuadD;
            }

            if (easingFunction == Ease.EaseInCubic)
            {
                return EaseInCubicD;
            }

            if (easingFunction == Ease.EaseOutCubic)
            {
                return EaseOutCubicD;
            }

            if (easingFunction == Ease.EaseInOutCubic)
            {
                return EaseInOutCubicD;
            }

            if (easingFunction == Ease.EaseInQuart)
            {
                return EaseInQuartD;
            }

            if (easingFunction == Ease.EaseOutQuart)
            {
                return EaseOutQuartD;
            }

            if (easingFunction == Ease.EaseInOutQuart)
            {
                return EaseInOutQuartD;
            }

            if (easingFunction == Ease.EaseInQuint)
            {
                return EaseInQuintD;
            }

            if (easingFunction == Ease.EaseOutQuint)
            {
                return EaseOutQuintD;
            }

            if (easingFunction == Ease.EaseInOutQuint)
            {
                return EaseInOutQuintD;
            }

            if (easingFunction == Ease.EaseInSine)
            {
                return EaseInSineD;
            }

            if (easingFunction == Ease.EaseOutSine)
            {
                return EaseOutSineD;
            }

            if (easingFunction == Ease.EaseInOutSine)
            {
                return EaseInOutSineD;
            }

            if (easingFunction == Ease.EaseInExpo)
            {
                return EaseInExpoD;
            }

            if (easingFunction == Ease.EaseOutExpo)
            {
                return EaseOutExpoD;
            }

            if (easingFunction == Ease.EaseInOutExpo)
            {
                return EaseInOutExpoD;
            }

            if (easingFunction == Ease.EaseInCirc)
            {
                return EaseInCircD;
            }

            if (easingFunction == Ease.EaseOutCirc)
            {
                return EaseOutCircD;
            }

            if (easingFunction == Ease.EaseInOutCirc)
            {
                return EaseInOutCircD;
            }

            if (easingFunction == Ease.Linear)
            {
                return LinearD;
            }

            if (easingFunction == Ease.Spring)
            {
                return SpringD;
            }

            if (easingFunction == Ease.EaseInBounce)
            {
                return EaseInBounceD;
            }

            if (easingFunction == Ease.EaseOutBounce)
            {
                return EaseOutBounceD;
            }

            if (easingFunction == Ease.EaseInOutBounce)
            {
                return EaseInOutBounceD;
            }

            if (easingFunction == Ease.EaseInBack)
            {
                return EaseInBackD;
            }

            if (easingFunction == Ease.EaseOutBack)
            {
                return EaseOutBackD;
            }

            if (easingFunction == Ease.EaseInOutBack)
            {
                return EaseInOutBackD;
            }

            if (easingFunction == Ease.EaseInElastic)
            {
                return EaseInElasticD;
            }

            if (easingFunction == Ease.EaseOutElastic)
            {
                return EaseOutElasticD;
            }

            if (easingFunction == Ease.EaseInOutElastic)
            {
                return EaseInOutElasticD;
            }

            return null;
        }
    }
}