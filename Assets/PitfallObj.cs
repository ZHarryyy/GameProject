using QFramework;

namespace PlatformShoot
{
    public interface IInteractiveItem
    {
        void Trigger();
    }

    public class PitfallObj : PlatformShootGameController, IInteractiveItem
    {
        public void Trigger()
        {
            this.SendCommand(new NextLevelCommand("GamePassScene"));
        }
    }
}
