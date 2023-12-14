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
        private int mFaceDir = 1;

        private MainPanel mMainPanel;
        private GameObject mGamePass;

        private void Start()
        {
            mRig = GetComponent<Rigidbody2D>();
            mGamePass = GameObject.Find("GamePass");
            mGamePass.SetActive(false);
            mMainPanel = GameObject.Find("MainPanel").GetComponent<MainPanel>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                var obj = Resources.Load<GameObject>("Bullet");
                obj = GameObject.Instantiate(obj, transform.position, Quaternion.identity);
                Bullet bullet = obj.GetComponent<Bullet>();
                bullet.GetGamePass(mGamePass);
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
