using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    public class PlatformShootGame : Architecture<PlatformShootGame>
    {
        protected override void Init()
        {
            RegisterModel<IGameModel>(new GameModel());
            RegisterModel<IGameAudioModel>(new GameAudioModel());

            RegisterSystem<ITimerSystem>(new TimerSystem());
            RegisterSystem<IObjectPoolSystem>(new ObjectPoolSystem());
            RegisterSystem<IAudioMgrSystem>(new AudioMgrSystem());
            RegisterSystem<ICameraSystem>(new CameraSystem());
        }
    }

    public class PlatformShootGameController: MonoBehaviour, IController
    {
        IArchitecture IBelongToArchitecture.GetArchitecture() => PlatformShootGame.Interface;
    }
}