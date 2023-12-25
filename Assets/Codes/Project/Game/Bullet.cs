using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class Bullet : PlatformShootGameController
    {
        private LayerMask mLayerMask;

        private Vector2 bulletDir;

        private float moveSpeed = 20f;

        private Timer mTimer;

        public void Awake()
        {
            mLayerMask = LayerMask.GetMask("Ground", "Trigger", "Enemy");
        }

        private void OnEnable()
        {
            mTimer = this.GetSystem<ITimerSystem>().AddTimer(3f, () => 
            {
                this.GetSystem<IObjectPoolSystem>().Recovery(gameObject);
            });
        }

        private void OnDisable()
        {
            mTimer.Stop();
        }

        public void InitDir(Vector2 dir)
        {
            bulletDir = dir.normalized;
        }

        public void Update()
        {
            transform.Translate(bulletDir * moveSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            var coll = Physics2D.OverlapBox(transform.position, transform.localScale, 0, mLayerMask);
            if(coll)
            {
                if(coll.CompareTag("Enemy"))
                {
                    GameObject.Destroy(coll.gameObject);
                    this.GetSystem<IAudioMgrSystem>().PlaySound("碰铃撞击");
                }
                else if(coll.CompareTag("Trigger"))
                {
                    GameObject.Destroy(coll.gameObject);
                    this.SendCommand<ShowPassDoorCommand>();
                    this.GetSystem<IAudioMgrSystem>().PlaySound("碰铃撞击");
                }
                else
                {
                    this.GetSystem<IAudioMgrSystem>().PlaySound("清脆高频击中");
                }
                this.GetSystem<IObjectPoolSystem>().Recovery(gameObject);
            }
        }
    }
}
