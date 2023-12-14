using UnityEngine;

namespace PlatformShoot
{
    public class CameraCtrl : MonoBehaviour
    {
        private Transform mTarget;

        private void Start()
        {
            mTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void LateUpdate()
        {
            transform.localPosition = new Vector3(mTarget.position.x, mTarget.position.y, -10);
        }
    }
}
