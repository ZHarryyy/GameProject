using QFramework;

namespace PlatformShoot
{
    public class NextLevel : PlatformShootGameController
    {
        private void Start()
        {
            gameObject.SetActive(false);
            this.RegisterEvent<ShowPassDoorEvent>(OnCanGamePass).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnCanGamePass(ShowPassDoorEvent e)
        {
            gameObject.SetActive(true);
        }
    }
}
