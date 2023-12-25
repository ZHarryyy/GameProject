using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class ScoreItem : PlatformShootGameController, IInteractiveItem
    {
        public void Trigger()
        {
            GameObject.Destroy(gameObject);
            this.GetModel<IGameModel>().Score.Value++;
            this.GetSystem<IAudioMgrSystem>().PlaySound("拾取金币");
        }
    }
}
