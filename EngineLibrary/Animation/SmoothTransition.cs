using SharpDX;
using System;

namespace EngineLibrary.Animation
{
    public class SmoothTransition : Transition
    {
        public override float CurrentTime => MathUtil.SmootherStep(base.CurrentTime);

        public SmoothTransition(float start, float end, float duration) : base(start, end, duration)
        {
        }
    }
}
