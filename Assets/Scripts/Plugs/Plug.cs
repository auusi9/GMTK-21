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
        [SerializeField] private float _speed = 100;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _plugInAudio;
        [SerializeField] private AudioClip _plugOutAudio;
        [SerializeField] private AudioClip _goToOriginAudio;

        public PairPlug PairPlug => _pairPlug;

        public Vector3 ContactPosition => _contactPosition.position;
        public event Action<Plug> JoinToJack;
        public event Action JackDisconnected;
        public Jack Jack;
        private bool _goToOrigin;

        private void Start()
        {
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
            _goToOrigin = true;
            _audioSource.clip = _goToOriginAudio;
            _audioSource.Play();
            DisconnectPlug();
        }

        private void DisconnectPlug()
        {
            Jack?.PlugDisconnected();
            Jack = null;
            JackDisconnected?.Invoke();
            _pairPlug.LightOff();
        }

        private void Update()
        {
            if (_goToOrigin)
            {
                transform.position = Vector3.MoveTowards(transform.position, _originPosition.position, _speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, _originPosition.position) < 0.4f)
                {
                    _goToOrigin = false;
                    transform.position = _originPosition.position;
                }
            }
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
            _audioSource.clip = _plugInAudio;
            _audioSource.Play();
        }

        private void RightClick()
        {
            _audioSource.PlayOneShot(_plugOutAudio);
            GoToOrigin();
            _plugImage.sprite = _plugOut;
            _plugImage.SetNativeSize();
        }
        
        private void PointerDownImage()
        {
            if (Jack != null)
            {
                _audioSource.clip = _plugOutAudio;
                _audioSource.Play();
            }

            _goToOrigin = false;
            
            _plugImage.sprite = _plugOut;
            _plugImage.SetNativeSize();
        }
    }
}
