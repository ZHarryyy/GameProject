using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlatformShoot
{
    public class StickDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private RectTransform mStickBgTrans;
        private Transform mStickCtrlTrans;

        private Action<int, int> mStickChange;

        private float maxRange;

        void IDragHandler.OnDrag(PointerEventData data)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mStickBgTrans, data.position, data.pressEventCamera, out Vector2 localPos);
            Vector2 dir = localPos.normalized;
            mStickChange?.Invoke(dir.x > 0 ? 1 : -1, dir.y > 0 ? 1 : -1);
            mStickCtrlTrans.localPosition = localPos.magnitude > maxRange ? dir * maxRange : localPos;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            mStickCtrlTrans.localPosition = Vector2.zero;
            mStickChange?.Invoke(0, 0);
        }

        public void Register(Action<int, int> stickChange) => mStickChange += stickChange;

        private void Start()
        {
            mStickBgTrans = transform.Find("StickBg").GetComponent<RectTransform>();
            mStickCtrlTrans = mStickBgTrans.Find("StickCtrl");

            maxRange = mStickBgTrans.rect.width * 0.5f;
        }
    }
}
