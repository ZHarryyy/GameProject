using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    public class ShootCommand : AbstractCommand
    {
        private readonly Vector2 shootDir;
        private readonly Vector2 shootPos;

        public ShootCommand(Vector2 dir, Vector2 pos)
        {
            shootDir = dir;
            shootPos = pos;
        }

        protected override void OnExecute()
        {
            this.GetSystem<IAudioMgrSystem>().PlaySound("竖琴");

            this.GetSystem<IObjectPoolSystem>().Get("Item/Bullet", o =>
            {
                o.transform.localPosition = shootPos;
                o.GetComponent<Bullet>().InitDir(shootDir);
            });
        }
    }

    public class PlayerWeapon : Weapon
    {
        protected override void ShootFunc(float shootDir)
        {
            this.SendCommand(new ShootCommand(Vector2.right * shootDir, transform.position));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                var item = other?.GetComponent<BagItem>();
                if(item != null)
                {
                    this.GetSystem<IGunSystem>().GetGun(item.nameId, out GunInfo gun);
                    mGuns.Add(gun);
                    Destroy(other.gameObject);
                }
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                mGuns.LoopPos();
            }
        }
    }
}
