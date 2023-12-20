using System;

namespace QFramework
{
    public enum FadeState
    {
        Close,
        FadeIn,
        FadeOut,
    }

    public class FadeNum
    {
        private FadeState mFadeState = FadeState.Close;
        public bool IsEnabled => mFadeState != FadeState.Close;
        private bool mInit = false;
        private Action mOnEvent;
        private float mCurrentValue;
        public float CurrentValue => mCurrentValue;
        private float mMin = 0, mMax = 1;

        public void SetMinMax(float min, float max)
        {
            mMin = min;
            mMax = max;
        }

        public void SetState(FadeState state, Action action = null)
        {
            mOnEvent = action;
            mFadeState = state;
            mInit = false;
        }

        public void Update(float step)
        {
            switch(mFadeState)
            {
                case FadeState.FadeIn:
                    if(!mInit)
                    {
                        mCurrentValue = mMin;
                        mInit = true;
                    }
                    if(mCurrentValue < mMax)
                    {
                        mCurrentValue += step;
                    }
                    else
                    {
                        mOnEvent?.Invoke();
                        mCurrentValue = mMax;
                        if(mInit) mFadeState = FadeState.Close;
                    }
                    break;
                case FadeState.FadeOut:
                    if(!mInit)
                    {
                        mCurrentValue = mMax;
                        mInit = true;
                    }
                    if(mCurrentValue > mMin)
                    {
                        mCurrentValue -= step;
                    }
                    else
                    {
                        mOnEvent?.Invoke();
                        mCurrentValue = mMin;
                        if(mInit) mFadeState = FadeState.Close;
                    }
                    break;
            }
        }
    }
}