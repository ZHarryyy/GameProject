using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class Player : PlatformShootGameController, ICamTarget
    {
        private Rigidbody2D mRig;
        private PlayerWeapon mWeapon;
        private PlayerInputHandle inputHandle;
        private BoxCollider2D mBoxColl;
        private LayerMask mGroundLayer;

        private float mAccDelta = 120f;
        private float mDecDelta = 180f;
        private float mGroundMoveSpeed = 5f;
        private float mJumpForce = 13f;

        [SerializeField] private int mJumpCount;
        [SerializeField] private int maxJumpCount = 2;
        private int mFaceDir = 1;
        private bool isJumping;
        private Vector2 mCurSpeed;

        private bool mGround;

        private IObjectPoolSystem objectPool;
        private IAudioMgrSystem audioMgr;

        Vector2 ICamTarget.Pos => transform.position;

        private void Awake()
        {
            this.SendCommand<InitGameCommand>();
            mWeapon = GetComponentInChildren<PlayerWeapon>();
            inputHandle = GetComponent<PlayerInputHandle>();
        }

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
            mBoxColl = GetComponentInChildren<BoxCollider2D>();
            mGroundLayer = LayerMask.GetMask("Ground");
            this.GetSystem<ICameraSystem>().SetTarget(this);

            objectPool = this.GetSystem<IObjectPoolSystem>();
            audioMgr = this.GetSystem<IAudioMgrSystem>();

            audioMgr.PlayBgm("黑色之翼");
        }

        private void Update()
        {
            mGround = Physics2D.OverlapBox(transform.position + mBoxColl.size.y * Vector3.down * 0.5f, new Vector2(mBoxColl.size.x * 0.8f, 0.1f), 0, mGroundLayer);

            if(inputHandle.AttackInput)
            {
                mWeapon.Shoot(mFaceDir);
            }

            if(mGround)
            {
                mJumpCount = maxJumpCount;
            }

            if(mGround && isJumping)
            {
                audioMgr.PlaySound("落地2");
                isJumping = false;
            }

            mCurSpeed = mRig.velocity;

            if (inputHandle.InputX != 0)
            {
                if(inputHandle.InputX != mFaceDir)
                {
                    Flip();
                }
                mCurSpeed.x = Mathf.Clamp(mCurSpeed.x + inputHandle.InputX * mAccDelta * Time.deltaTime, -mGroundMoveSpeed, mGroundMoveSpeed);
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
            Gizmos.DrawWireCube(transform.position + mBoxColl.size.y * Vector3.down * 0.5f, new Vector2(mBoxColl.size.x * 0.9f, 0.1f));
        }

        private void FixedUpdate()
        {
            // Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.1f, Color.red, 10);
            if(mJumpCount == 0)
            {
                inputHandle.JumpInput = false;
            }
            else if(inputHandle.JumpInput)
            {
                mJumpCount--;
                isJumping = true;
                inputHandle.JumpInput = false;
                mCurSpeed.y = mJumpForce;
                audioMgr.PlaySound("跳跃");
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
            if(coll.gameObject.CompareTag("Interactive"))
            {
                coll.GetComponent<IInteractiveItem>()?.Trigger();
            }
        }
    }
}
