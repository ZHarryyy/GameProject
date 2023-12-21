using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PlatformShoot
{
    public class StartPanel : PlatformShootGameController
    {
        private void Awake()
        {
            transform.Find("StartBtn").GetComponent<Button>().onClick.AddListener(OnStartBtn);
            transform.Find("ExitBtn").GetComponent<Button>().onClick.AddListener(OnExitBtn);
        }

        private void OnExitBtn()
        {
            Application.Quit();
        }

        private void OnStartBtn()
        {
            this.SendCommand(new NextLevelCommand("SampleScene"));
        }
    }
}
