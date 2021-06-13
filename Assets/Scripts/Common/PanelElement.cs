using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common
{
    public class PanelElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private float _scale = 1.0f;

        public event Action RightClickEvent;
        public event Action PointerUpEvent;
        public event Action PointerDownEvent;

        private Canvas _canvas;
        private RectTransform _parent;

        public void Configure(Canvas canvas, RectTransform parent)
        {
            _canvas = canvas;
            _parent = parent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

            Vector3[] fourCornersArray = new Vector3[4];
            _parent.GetWorldCorners(fourCornersArray);

            Vector3 pos = _rectTransform.position;

            pos.x = Mathf.Clamp(pos.x, fourCornersArray[0].x, fourCornersArray[2].x);
            pos.y = Mathf.Clamp(pos.y, fourCornersArray[0].y, fourCornersArray[2].y);

            _rectTransform.position = pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            _shadow.enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClickEvent?.Invoke();
                return;
            }

            transform.localScale = new Vector3(_scale, _scale, 1.0f);
            _shadow.enabled = true;
            transform.SetAsLastSibling();
            PointerDownEvent?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _shadow.enabled = false;
            PointerUpEvent?.Invoke();
        }
    }
}