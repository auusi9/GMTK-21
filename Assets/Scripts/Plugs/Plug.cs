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
        [SerializeField] private AudioClip _plugInAudio;
        [SerializeField] private AudioClip _plugOutAudio;
        [SerializeField] private AudioClip _goToOriginAudio;

        public PairPlug PairPlug => _pairPlug;
        AudioSource audio;

        public Vector3 ContactPosition => _contactPosition.position;
        public event Action<Plug> JoinToJack;
        public event Action JackDisconnected;
        public Jack Jack;

        private void Start()
        {
            audio = GetComponent<AudioSource>();
            _panelElement.PointerUpEvent += JoinJack;
            _panelElement.PointerDownEvent += PointerDownImage;
            _panelElement.RightClickEvent += RightClick;
        }

        private void OnDestroy()
        {
            _panelElement.PointerUpEvent -= JoinJack;
            _panelElement.PointerDownEvent -= PointerDownImage;
            _panelElement.RightClickEvent -= RightClick;
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
            audio.clip = _goToOriginAudio;
            audio.Play();
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
                JackDisconnected?.Invoke();
            }

           
            Jack = closestJack;
            transform.position = closestJack.JackPosition;
            RectTransform rectTransform = (RectTransform) transform;
            rectTransform.anchoredPosition -= Vector2.up * 42;
            _plugImage.sprite = _plugIn;
            _plugImage.SetNativeSize();
            audio.clip = _plugInAudio;
            audio.Play();
        }

        private void RightClick()
        {
            GoToOrigin();
            _plugImage.sprite = _plugOut;
            _plugImage.SetNativeSize();
        }
        
        private void PointerDownImage()
        {
            if (Jack != null)
            {
                audio.clip = _plugOutAudio;
                audio.Play();
            }

            _plugImage.sprite = _plugOut;
            _plugImage.SetNativeSize();
        }
    }
}
