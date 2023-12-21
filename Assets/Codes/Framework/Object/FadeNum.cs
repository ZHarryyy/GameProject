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
        private bool mInit = false;
        private Action mOnFinish;
        private float mCurrentValue;
        private float mMin = 0, mMax = 1;

        public void SetMinMax(float min, float max)
        {
            mMin = min;
            mMax = max;
        }

        public bool IsEnabled => mFadeState != FadeState.Close;

        public void SetState(FadeState state, Action action = null)
        {
            mOnFinish = action;
            mFadeState = state;
            mInit = false;
        }

        private void OnFinish(float value)
        {
            mOnFinish?.Invoke();
            mCurrentValue = value;
            if(!mInit) return;
            mFadeState = FadeState.Close;
        }

        public float Update(float step)
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
                    else OnFinish(mMax);
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
                    else OnFinish(mMin);
                    break;
            }
            return mCurrentValue;
        }
    }
}