using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformShoot
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D mRig;
        private float mGroundMoveSpeed = 5f;
        private float mJumpForce = 12f;
        private bool mJumpInput;
        private MainPanel mMainPanel;

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
            mMainPanel = GameObject.Find("MainPanel").GetComponent<MainPanel>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                var bullet = Resources.Load<GameObject>("Bullet");
                GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
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
            mRig.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * mGroundMoveSpeed, mRig.velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if(coll.gameObject.CompareTag("Reward"))
            {
                GameObject.Destroy(coll.gameObject);
                mMainPanel.UpdateScoreText(1);
            }
            if(coll.gameObject.CompareTag("Door"))
            {
                SceneManager.LoadScene("GamePassScene");
            }
        }
    }
}
