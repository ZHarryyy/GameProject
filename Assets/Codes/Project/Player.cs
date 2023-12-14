using UnityEngine;

namespace PlatformShoot
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D mRig;
        private float mGroundMoveSpeed = 5f;
        private float mJumpForce = 12f;

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                var bullet = Resources.Load<GameObject>("Bullet");
                GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
            }
        }

        private void FixedUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                mRig.velocity = new Vector2(mRig.velocity.x, mJumpForce);
            }
            mRig.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * mGroundMoveSpeed, mRig.velocity.y);
        }
    }
}
