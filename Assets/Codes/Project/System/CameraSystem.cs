using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    public interface ICameraSystem : ISystem
    {
        void SetTarget(Transform target);
    }

    public class CameraSystem : AbstractSystem, ICameraSystem
    {
        private Transform mTarget;

        protected override void OnInit()
        {
            PublicMono.Instance.OnLateUpdate += Update;
        }

        void ICameraSystem.SetTarget(Transform target)
        {
            mTarget = target;
        }

        private void Update()
        {
            if(mTarget == null) return;
            Camera.main.transform.localPosition = new Vector3(mTarget.position.x, mTarget.position.y, -10);
        }
    }
}