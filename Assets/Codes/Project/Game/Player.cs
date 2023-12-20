using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class Player : MonoBehaviour, IController
    {
        private Rigidbody2D mRig;
        private BoxCollider2D mBoxColl;
        private LayerMask mGroundLayer;

        private float mAccDelta = 0.6f;
        private float mDecDelta = 0.9f;
        private float mGroundMoveSpeed = 5f;
        private float mJumpForce = 12f;

        private bool mJumpInput;
        private int mFaceDir = 1;
        private bool isJumping;

        private IObjectPoolSystem objectPool;
        private IAudioMgrSystem audioMgr;

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
            mBoxColl = GetComponentInChildren<BoxCollider2D>();
            mGroundLayer = LayerMask.GetMask("Ground");
            this.GetSystem<ICameraSystem>().SetTarget(this.transform);

            objectPool = this.GetSystem<IObjectPoolSystem>();
            audioMgr = this.GetSystem<IAudioMgrSystem>();
            audioMgr.PlayBgm("黑色之翼");
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                audioMgr.PlaySound("竖琴");

                objectPool.Get("Item/Bullet", o =>
                {
                    o.transform.localPosition = transform.position;
                    o.GetComponent<Bullet>().InitDir(mFaceDir);
                });
            }

            var ground = Physics2D.OverlapBox(transform.position + mBoxColl.size.y * Vector3.down * 0.5f, new Vector2(mBoxColl.size.x * 0.8f, 0.1f), 0, mGroundLayer);

            if(Input.GetKeyDown(KeyCode.K))
            {
                if(ground)
                {
                    audioMgr.PlaySound("跳跃");
                    mJumpInput = true;
                    isJumping = true;
                }
                else if(ground && isJumping)
                {
                    audioMgr.PlaySound("落地2");
                    isJumping = false;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(mBoxColl == null) return;
            Gizmos.DrawWireCube(transform.position + mBoxColl.size.y * Vector3.down * 0.5f, new Vector2(mBoxColl.size.x * 0.9f, 0.1f));
        }

        private void FixedUpdate()
        {
            if (mJumpInput)
            {
                mJumpInput = false;
                mRig.velocity = new Vector2(mRig.velocity.x, mJumpForce);
            }
            float h = Input.GetAxisRaw("Horizontal");
            if (h != 0)
            {
                if(h != mFaceDir)
                {
                    Flip();
                }
                mRig.velocity = new Vector2(Mathf.Clamp(mRig.velocity.x + h * mAccDelta, -mGroundMoveSpeed, mGroundMoveSpeed), mRig.velocity.y);
            }
            else
            {
                mRig.velocity = new Vector2(Mathf.MoveTowards(mRig.velocity.x, 0, mDecDelta), mRig.velocity.y);
            }
        }

        private void Flip()
        {
            mFaceDir = -mFaceDir;
            transform.Rotate(0, 180, 0);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.gameObject.CompareTag("Reward"))
            {
                GameObject.Destroy(coll.gameObject);
                this.GetModel<IGameModel>().Score.Value++;
                audioMgr.PlaySound("拾取金币");
            }
            else if(coll.gameObject.CompareTag("Door"))
            {
                this.SendCommand(new NextLevelCommand("GamePassScene"));
                audioMgr.PlaySound("通关音效");
            }
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return PlatformShootGame.Interface;
        }
    }
}
