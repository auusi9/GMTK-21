using System;
using Common;
using Jacks;
using UnityEngine;

namespace Plugs
{
    public class Plug : MonoBehaviour
    {
        [SerializeField] private PanelElement _panelElement;
        [SerializeField] private Transform _contactPosition;
        [SerializeField] private Transform _originPosition;
        [SerializeField] private PairPlug _pairPlug;
        
        public PairPlug PairPlug => _pairPlug;

        public Vector3 ContactPosition => _contactPosition.position;
        public event Action<Plug> JoinToJack;
        public event Action JackDisconnected;
        public Jack Jack;

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
            Jack?.PlugDisconnected();
            Jack = null;
            transform.position = _originPosition.position;
            JackDisconnected?.Invoke();
            _pairPlug.LightOff();
        }

        public void ConnectToJack(Jack closestJack)
        {
            Jack = closestJack;
            transform.position = closestJack.JackPosition;
            RectTransform rectTransform = (RectTransform) transform;
            rectTransform.anchoredPosition -= Vector2.up * 50;
        }
    }
}
