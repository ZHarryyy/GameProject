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

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
            mBoxColl = GetComponentInChildren<BoxCollider2D>();
            mGroundLayer = LayerMask.GetMask("Ground");
            this.GetSystem<ICameraSystem>().SetTarget(this.transform);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                AudioPlay.Instance.PlaySound("竖琴");

                ResHelper.AsyncLoad<GameObject>("Item/Bullet", o =>
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
                    AudioPlay.Instance.PlaySound("跳跃");
                    mJumpInput = true;
                    isJumping = true;
                }
                else if(ground && isJumping)
                {
                    AudioPlay.Instance.PlaySound("落地2");
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
                mRig.velocity = new Vector2(Mathf.Clamp(mRig.velocity.x + h * mAccDelta, -mGroundMoveSpeed, mGroundMoveSpeed), mRig.velocity.y);
            }
            else
            {
                mRig.velocity = new Vector2(Mathf.MoveTowards(mRig.velocity.x, 0, mDecDelta), mRig.velocity.y);
            }
            Flip(h);
        }

        private void Flip(float h)
        {
            if (h != 0 && h != mFaceDir)
            {
                mFaceDir = -mFaceDir;
                transform.Rotate(0, 180, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.gameObject.CompareTag("Reward"))
            {
                GameObject.Destroy(coll.gameObject);
                this.GetModel<IGameModel>().Score.Value++;
                AudioPlay.Instance.PlaySound("拾取金币");
            }
            if(coll.gameObject.CompareTag("Door"))
            {
                this.SendCommand(new NextLevelCommand("GamePassScene"));
                AudioPlay.Instance.PlaySound("通关音效");
            }
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return PlatformShootGame.Interface;
        }
    }
}
