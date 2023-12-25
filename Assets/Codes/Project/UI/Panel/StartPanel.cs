using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class StartPanel : PlatformShootUIController
    {
        protected override void OnClick(string name)
        {
            switch(name)
            {
                case "StartBtn":
                    this.SendCommand(new NextLevelCommand("SampleScene"));
                    break;
                case "ExitBtn":
                    Application.Quit();
                    break;
            }
        }

        protected override void onValueChanged(string name, bool value)
        {
            if(value)
            {
                switch(name)
                {
                    case "xxx":
                        break;
                    case "yyy":
                        break;
                }
            }
        }
    }
}
