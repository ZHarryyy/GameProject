using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    public interface ICameraSystem : ISystem
    {
        void SetTarget(Transform target);
        void Update();
    }

    public class CameraSystem : AbstractSystem, ICameraSystem
    {
        private Transform mTarget;

        protected override void OnInit()
        {
            
        }

        void ICameraSystem.SetTarget(Transform target)
        {
            mTarget = target;
        }

        void ICameraSystem.Update()
        {
            Camera.main.transform.localPosition = new Vector3(mTarget.position.x, mTarget.position.y, -10);
        }
    }
}