using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class Bullet : MonoBehaviour, IController
    {
        private LayerMask mLayerMask;

        private int bulletDir;

        void Start()
        {
            GameObject.Destroy(this.gameObject, 3f);
            mLayerMask = LayerMask.GetMask("Ground", "Trigger");
        }

        public void InitDir(int dir)
        {
            bulletDir = dir;
        }

        void Update()
        {
            transform.Translate(bulletDir * 12 * Time.deltaTime, 0, 0);
        }

        private void FixedUpdate()
        {
            var coll = Physics2D.OverlapBox(transform.position, transform.localScale, 0, mLayerMask);
            if(coll)
            {
                if(coll.CompareTag("Trigger"))
                {
                    GameObject.Destroy(coll.gameObject);
                    this.SendCommand<ShowPassDoorCommand>();
                    AudioPlay.Instance.PlaySound("碰铃撞击");
                }
                else
                {
                    AudioPlay.Instance.PlaySound("清脆高频机中");
                }
                GameObject.Destroy(gameObject);
            }
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return PlatformShootGame.Interface;
        }
    }
}
