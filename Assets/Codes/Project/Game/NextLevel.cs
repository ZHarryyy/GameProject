using QFramework;

namespace PlatformShoot
{
    public class NextLevel : PlatformShootGameController, IInteractiveItem
    {
        private void Start()
        {
            this.RegisterEvent<ShowPassDoorEvent>(OnCanGamePass).UnRegisterWhenGameObjectDestroyed(gameObject);
            gameObject.SetActive(false);
        }

        private void OnCanGamePass(ShowPassDoorEvent e)
        {
            gameObject.SetActive(true);
        }

        public void Trigger()
        {
            this.SendCommand(new NextLevelCommand("GamePassScene"));
            this.GetSystem<IAudioMgrSystem>().PlaySound("通关音效");
        }
    }
}
