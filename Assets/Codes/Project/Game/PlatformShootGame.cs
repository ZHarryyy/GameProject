using QFramework;

namespace PlatformShoot
{
    public class PlatformShootGame : Architecture<PlatformShootGame>
    {
        protected override void Init()
        {
            RegisterModel<IGameModel>(new GameModel());

            RegisterSystem<ICameraSystem>(new CameraSystem());

            RegisterSystem<IAudioMgrSystem>(new AudioMgrSystem());

            RegisterSystem<IObjectPoolSystem>(new ObjectPoolSystem());

            RegisterSystem<ITimerSystem>(new TimerSystem());
        }
    }
}