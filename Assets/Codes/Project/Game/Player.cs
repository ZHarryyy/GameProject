using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class Player : MonoBehaviour, IController
    {
        private Rigidbody2D mRig;
        private float mGroundMoveSpeed = 5f;
        private float mJumpForce = 12f;

        private bool mJumpInput;
        private int mFaceDir = 1;

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
            this.GetSystem<ICameraSystem>().SetTarget(this.transform);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                var obj = Resources.Load<GameObject>("Item/Bullet");
                obj = GameObject.Instantiate(obj, transform.position, Quaternion.identity);
                Bullet bullet = obj.GetComponent<Bullet>();
                bullet.InitDir(mFaceDir);
            }
            if(Input.GetKeyDown(KeyCode.K))
            {
                mJumpInput = true;
            }
        }

        private void FixedUpdate()
        {
            if(mJumpInput)
            {
                mJumpInput = false;
                mRig.velocity = new Vector2(mRig.velocity.x, mJumpForce);
            }
            float h = Input.GetAxisRaw("Horizontal");
            if(h != 0 && h != mFaceDir)
            {
                mFaceDir = -mFaceDir;
                transform.Rotate(0, 180, 0);
            }
            mRig.velocity = new Vector2(h * mGroundMoveSpeed, mRig.velocity.y);
        }

        private void LateUpdate()
        {
            this.GetSystem<ICameraSystem>().Update();
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.gameObject.CompareTag("Reward"))
            {
                GameObject.Destroy(coll.gameObject);
                this.GetModel<IGameModel>().Score.Value++;
                // mMainPanel.UpdateScoreText(1);
            }
            if(coll.gameObject.CompareTag("Door"))
            {
                this.SendCommand(new NextLevelCommand("GamePassScene"));
            }
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return PlatformShootGame.Interface;
        }
    }
}
