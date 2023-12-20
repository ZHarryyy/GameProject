using System;
using UnityEngine;

namespace QFramework
{
    public struct StopBgmEvent
    { 
        public bool isStop;
    }

    public interface IAudioMgrSystem : ISystem
    {
        void PlayBgm(string name);
        void PlaySound(string name);
        void GetSound(string name, Action<AudioSource> callback);
        void StopSound(AudioSource source);

        BindableProperty<float> BgmVolume { get; }
        BindableProperty<float> SoundVolume{ get; }
    }
    public class AudioMgrSystem : AbstractSystem, IAudioMgrSystem
    {
        public BindableProperty<float> BgmVolume { get; } = new BindableProperty<float>(1);
        public BindableProperty<float> SoundVolume { get; } = new BindableProperty<float>(1);

        private AudioSource mBGM;
        private AudioSource tempSource;
        private FadeNum mFade;
        private ResPool<AudioClip> mClipPool;
        protected ComponentPool<AudioSource> mSourcePool;

        protected override void OnInit()
        {
            mSourcePool = new ComponentPool<AudioSource>("GameSound");

            mClipPool = new ResPool<AudioClip>();

            mFade = new FadeNum();
            mFade.SetMinMax(0, BgmVolume.Value);

            this.RegisterEvent<StopBgmEvent>(OnStopBgm);

            BgmVolume.RegisterWithInitValue(OnBgmVolumeChanged);
            SoundVolume.RegisterWithInitValue(OnSoundVolumeChanged);
        }

        private void Update()
        {
            if(!mFade.IsEnabled) return;
            mFade.Update(Time.deltaTime);
            mBGM.volume = mFade.CurrentValue;
        }

        private void OnBgmVolumeChanged(float v)
        {
            mFade.SetMinMax(0, v);

            if(mBGM == null) return;

            mBGM.volume = v;
        }

        private void OnSoundVolumeChanged(float v)
        {
            mSourcePool.SetAllEnabledComponent(source => source.volume = v);
        }

        private void OnStopBgm(StopBgmEvent e)
        {
            if(mBGM == null || !mBGM.isPlaying) return;
            PublicMono.Instance.OnUpdate += Update;
            mFade.SetState(FadeState.FadeOut, () =>
            {
                PublicMono.Instance.OnUpdate -= Update;
                if(e.isStop) mBGM.Stop();
                else mBGM.Pause();
            });
        }

        public void PlaySound(string name)
        {
            mSourcePool.AutoPush(cp => !cp.isPlaying);
            mSourcePool.Get(out tempSource);
            mClipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSource.clip = clip;
                tempSource.loop = false;
                tempSource.volume = SoundVolume.Value;
                tempSource.Play();
            });
        }

        public void GetSound(string name, Action<AudioSource> callback)
        {
            mSourcePool.AutoPush(cp => !cp.isPlaying);
            mSourcePool.Get(out tempSource);
            mClipPool.Get("Audio/Sound" + name, clip =>
            {
                tempSource.clip = clip;
                tempSource.loop = true;
                tempSource.volume = SoundVolume.Value;
                callback(tempSource);
            });
        }

        public void StopSound(AudioSource source)
        {
            mSourcePool.Push(source, source.Stop);
        }

        public void PlayBgm(string name)
        {
            if(mBGM == null)
            {
                var o = new GameObject("GameBGM");
                GameObject.DontDestroyOnLoad(o);
                mBGM = o.AddComponent<AudioSource>();
                mBGM.loop = true;
                mBGM.volume = 0;
            }
            mClipPool.Get("Audio/BGM/" + name, audioClip =>
            {
                PublicMono.Instance.OnUpdate += Update;
                if(!mBGM.isPlaying)
                {
                    mFade.SetState(FadeState.FadeIn, () =>
                    {
                        PublicMono.Instance.OnUpdate -= Update;
                    });
                    mBGM.clip = audioClip;
                    mBGM.Play();
                }
                else
                {
                    mFade.SetState(FadeState.FadeOut, () =>
                    {
                        mFade.SetState(FadeState.FadeIn, () =>
                        {
                            PublicMono.Instance.OnUpdate -= Update;
                        });
                        mBGM.clip = audioClip;
                        mBGM.Play();
                    });
                }
            });
        }
    }
}
