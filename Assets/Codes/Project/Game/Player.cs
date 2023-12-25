using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class Player : PlatformShootGameController, ICamTarget
    {
        private Rigidbody2D mRig;
        private BoxCollider2D mBoxColl;
        private LayerMask mGroundLayer;

        private float mAccDelta = 120f;
        private float mDecDelta = 180f;
        private float mGroundMoveSpeed = 5f;
        private float mJumpForce = 13f;

        private bool mJumpInput;
        private int mFaceDir = 1;
        private bool isJumping;
        private Vector2 mCurSpeed;

        [SerializeField]private int mInputX;
        private int mInputY;

        private bool mAttackInput;

        private bool mGround;

        private IObjectPoolSystem objectPool;
        private IAudioMgrSystem audioMgr;

        Vector2 ICamTarget.Pos => transform.position;

        private void Awake()
        {
            this.SendCommand<InitGameCommand>();
        }

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
            mBoxColl = GetComponentInChildren<BoxCollider2D>();
            mGroundLayer = LayerMask.GetMask("Ground");
            this.GetSystem<ICameraSystem>().SetTarget(this);

            objectPool = this.GetSystem<IObjectPoolSystem>();
            audioMgr = this.GetSystem<IAudioMgrSystem>();
            this.RegisterEvent<DirInputEvent>(e =>
            {
                mInputX = e.inputX;
                mInputY = e.inputY;
            });
            this.RegisterEvent<ShootInputEvent>(e =>
            {
                mAttackInput = e.isTrigger;
            });
            this.RegisterEvent<JumpInputEvent>(e =>
            {
                if(mGround)
                {
                    audioMgr.PlaySound("跳跃");
                    mJumpInput = true;
                    isJumping = true;
                }
            });
            // audioMgr.PlayBgm("黑色之翼");
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                mAttackInput = false;
                audioMgr.PlaySound("竖琴");

                objectPool.Get("Item/Bullet", o =>
                {
                    o.transform.localPosition = transform.position;
                    o.GetComponent<Bullet>().InitDir(mFaceDir);
                });
            }

            mGround = Physics2D.OverlapBox(transform.position + mBoxColl.size.y * Vector3.down * 0.5f, new Vector2(mBoxColl.size.x * 0.8f, 0.1f), 0, mGroundLayer);

            if(mGround && isJumping)
            {
                audioMgr.PlaySound("落地2");
                isJumping = false;
            }

            mCurSpeed = mRig.velocity;

            if (mInputX != 0)
            {
                if(mInputX != mFaceDir)
                {
                    Flip();
                }
                mCurSpeed.x = Mathf.Clamp(mCurSpeed.x + mInputX * mAccDelta * Time.deltaTime, -mGroundMoveSpeed, mGroundMoveSpeed);
            }
            else
            {
                mCurSpeed.x = Mathf.MoveTowards(mCurSpeed.x, 0, mDecDelta * Time.deltaTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(mBoxColl == null) return;
            // Gizmos.DrawWireCube(transform.position + mBoxColl.size.y * Vector3.down * 0.5f, new Vector2(mBoxColl.size.x * 0.9f, 0.1f));
        }

        private void FixedUpdate()
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.1f, Color.red, 10);
            if (mJumpInput)
            {
                mJumpInput = false;
                mCurSpeed.y = mJumpForce;
            }
            mRig.velocity = mCurSpeed;
        }

        private void Flip()
        {
            mFaceDir = -mFaceDir;
            transform.Rotate(0, 180, 0);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.CompareTag("Pitfall"))
            {
                this.SendCommand(new NextLevelCommand("GamePassScene"));
            }
            else if(coll.gameObject.CompareTag("Reward"))
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
    }
}
