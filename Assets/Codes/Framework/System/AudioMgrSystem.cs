using UnityEngine;

namespace QFramework
{
    public interface IAudioMgrSystem : ISystem
    {
        void PlayBgm(string name);
        void StopBgm(bool isPause);
        void PlaySound(string name);
        AudioSource GetSound(string name);
        void RecoverySound(AudioSource source);
        void Clear();
    }
    public class AudioMgrSystem : AbstractSystem, IAudioMgrSystem
    {
        private AudioSource mBGM;
        private AudioSource tempSource;
        private FadeNum mFade;
        private ResPool<AudioClip> mClipPool;
        private ComponentPool<AudioSource> mSourcePool;
        private IGameAudioModel mAudioModel;

        protected override void OnInit()
        {
            mClipPool = new ResPool<AudioClip>();
            mSourcePool = new ComponentPool<AudioSource>("GameSound");

            mAudioModel = this.GetModel<IGameAudioModel>();

            mFade = new FadeNum();
            mFade.SetMinMax(0, mAudioModel.BgmVolume.Value);

            mAudioModel.BgmVolume.Register(OnBgmVolumeChanged);
            mAudioModel.SoundVolume.Register(OnSoundVolumeChanged);

            PublicMono.Instance.OnUpdate += UpdateVolume;
        }

        void IAudioMgrSystem.PlaySound(string name)
        {
            InitSource();
            mClipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSource.clip = clip;
                tempSource.loop = false;
                tempSource.Play();
            });
        }

        AudioSource IAudioMgrSystem.GetSound(string name)
        {
            InitSource();
            mClipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSource.clip = clip;
                tempSource.loop = true;
            });
            return tempSource;
        }

        void IAudioMgrSystem.RecoverySound(AudioSource source)
        {
            mSourcePool.Push(source, source.Stop);
        }

        void IAudioMgrSystem.PlayBgm(string name)
        {
            mClipPool.Get("Audio/Bgm/" + name, PlayBgm);
        }

        void IAudioMgrSystem.StopBgm(bool isPause)
        {
            if(mBGM == null || !mBGM.isPlaying) return;

            mFade.SetState(FadeState.FadeOut, () =>
            {
                if(isPause) mBGM.Pause();
                else mBGM.Stop();
            });
        }

        void IAudioMgrSystem.Clear()
        {
            mClipPool.Clear();
        }

        private void PlayBgm(AudioClip clip)
        {
            if(mBGM == null)
            {
                var o = new GameObject("GameBGM");
                GameObject.DontDestroyOnLoad(o);
                mBGM = o.AddComponent<AudioSource>();
                mBGM.loop = true;
                mBGM.volume = 0;
            }
            mBGM.clip = clip;
            if(!mBGM.isPlaying) PlayBgm();
            else mFade.SetState(FadeState.FadeOut, PlayBgm);
        }

        private void PlayBgm()
        {
            mFade.SetState(FadeState.FadeIn);
            mBGM.Play();
        }

        private void OnBgmVolumeChanged(float v)
        {
            if(mBGM == null) return;
            mFade.SetMinMax(0, v);
            mBGM.volume = v;
        }

        private void UpdateVolume()
        {
            if(!mFade.IsEnabled) return;
            mBGM.volume = mFade.Update(Time.deltaTime);
        }

        public void InitSource()
        {
            mSourcePool.AutoPush(source => !source.isPlaying);
            mSourcePool.Get(out tempSource);
            tempSource.volume = mAudioModel.SoundVolume.Value;
        }

        private void OnSoundVolumeChanged(float v)
        {
            mSourcePool.SetAllEnabledComponent(source => source.volume = v);
        }
    }
}
