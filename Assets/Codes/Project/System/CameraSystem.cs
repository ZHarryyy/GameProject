using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    public interface ICameraSystem : ISystem
    {
        void SetTarget(ICamTarget target);
    }

    public interface ICamTarget
    {
        Vector2 Pos { get; }
    }

    public class AdvancedCameraSystem : AbstractSystem, ICameraSystem
    {
        private ICamTarget mTarget;
        private Vector3 mTargetPos;
        private float mSmoothTime = 2;
        private float minX = -100f, minY = -100f, maxX = 100f, maxY = 100f;
        private float mSmoothSpeed = 5f;

        protected override void OnInit()
        {
            PublicMono.Instance.OnFixedUpdate += Update;
            mTargetPos.z = -10;
        }

        void ICameraSystem.SetTarget(ICamTarget target)
        {
            mTarget = target;
        }

        private void Update()
        {
            if(mTarget.Equals(null)) return;
            mTargetPos.x = Mathf.Clamp(mTarget.Pos.x, minX, maxX);
            mTargetPos.y = Mathf.Clamp(mTarget.Pos.y, minY, maxY);

            var cam = Camera.main.transform;
            if((cam.position - mTargetPos).sqrMagnitude < 0.01f) return;
            cam.localPosition = Vector3.Lerp(cam.position, mTargetPos, mSmoothSpeed * Time.deltaTime);
        }
    }
}