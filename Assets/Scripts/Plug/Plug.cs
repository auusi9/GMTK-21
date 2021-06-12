using System;
using Common;
using UnityEngine;

namespace Plug
{
    public class Plug : MonoBehaviour
    {
        [SerializeField] private PanelElement _panelElement;
        [SerializeField] private Transform _contactPosition;
        [SerializeField] private Transform _originPosition;

        public Vector3 ContactPosition => _contactPosition.position;

        public event Action<Plug> JoinToJack;
        private void Start()
        {
            _panelElement.PointerUpEvent += JoinJack;
        }

        private void OnDestroy()
        {
            _panelElement.PointerUpEvent -= JoinJack;
        }

        public void Configure(Canvas canvas, RectTransform rectTransform)
        {
            _panelElement.Configure(canvas, rectTransform);
        }
        
        private void JoinJack()
        {
            JoinToJack?.Invoke(this);
        }

        public void GoToOrigin()
        {
            transform.position = _originPosition.position;
        }

        public void ConnectToJack(Transform closestJack)
        {
            transform.position = closestJack.position;
        }
    }
}
