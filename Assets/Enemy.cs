using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D mRig;
    private int mFaceDir = 1;
    private float speed = 5;

    private void Start()
    {
        mRig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var wallHit = Physics2D.Raycast(transform.position, new Vector2(mFaceDir, 0), 1, LayerMask.GetMask("Ground"));
        if(wallHit)
        {
            Flip();
        }

        var hit = Physics2D.Raycast(transform.position + new Vector3(mFaceDir, 0), Vector2.down, 1, LayerMask.GetMask("Ground"));
        Debug.Log(hit.collider);
        Debug.DrawLine(transform.position + new Vector3(mFaceDir, 0), transform.position + new Vector3(mFaceDir, 0) + Vector3.down, hit.collider ? Color.green : Color.red);
        if(hit.collider)
        {
            Debug.Log("检测到地面");
            mRig.velocity = new Vector2(mFaceDir * speed, mRig.velocity.y);
        }
        else
        {
            Flip();
        }
    }

    private void Flip()
    {
        int dir = mRig.velocity.x > 0 ? -1 : 1;
        if (dir != mFaceDir)
        {
            mFaceDir = dir;
            transform.Rotate(0, 180, 0);
        }
    }
}
