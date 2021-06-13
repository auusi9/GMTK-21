using System;
using Common;
using Jacks;
using UnityEngine;
using UnityEngine.UI;

namespace Plugs
{
    public class Plug : MonoBehaviour
    {
        [SerializeField] private PanelElement _panelElement;
        [SerializeField] private Transform _contactPosition;
        [SerializeField] private Transform _originPosition;
        [SerializeField] private PairPlug _pairPlug;
        [SerializeField] private Image _plugImage;
        [SerializeField] private Sprite _plugOut;
        [SerializeField] private Sprite _plugIn;
        
        public PairPlug PairPlug => _pairPlug;

        public Vector3 ContactPosition => _contactPosition.position;
        public event Action<Plug> JoinToJack;
        public event Action JackDisconnected;
        public Jack Jack;

        private void Start()
        {
            _panelElement.PointerUpEvent += JoinJack;
            _panelElement.PointerDownEvent += PointerDownImage;
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
            if (Jack != closestJack)
            {
                Jack?.PlugDisconnected(); 
                Jack = null;
            }
            
            Jack = closestJack;
            transform.position = closestJack.JackPosition;
            RectTransform rectTransform = (RectTransform) transform;
            rectTransform.anchoredPosition -= Vector2.up * 42;
            _plugImage.sprite = _plugIn;
            _plugImage.SetNativeSize();
        }

        private void PointerDownImage()
        {
            _plugImage.sprite = _plugOut;
            _plugImage.SetNativeSize();
        }
    }
}
