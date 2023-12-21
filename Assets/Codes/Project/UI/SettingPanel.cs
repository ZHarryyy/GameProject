using UnityEngine.UI;
using QFramework;

namespace PlatformShoot
{
    public class SettingPanel : PlatformShootGameController
    {
            private IGameAudioModel gameAudio;

        private void Awake()
        {
            transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(OnCloseSelf);
            gameAudio = this.GetModel<IGameAudioModel>();
            var bgm = transform.Find("BgmVolume").GetComponent<Slider>();
            bgm.value = gameAudio.BgmVolume.Value;
            bgm.onValueChanged.AddListener(OnBgmValueChanged);
            
            var sound = transform.Find("SoundVolume").GetComponent<Slider>();
            sound.value = gameAudio.SoundVolume.Value;
            sound.onValueChanged.AddListener(OnSoundValueChanged);
        }

        private void OnSoundValueChanged(float arg)
        {
            gameAudio.SoundVolume.Value = arg;
        }

        private void OnBgmValueChanged(float arg)
        {
            gameAudio.BgmVolume.Value = arg;
        }

        private void OnCloseSelf()
        {
            Destroy(gameObject);
        }
    }
}
