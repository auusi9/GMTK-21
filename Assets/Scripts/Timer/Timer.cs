using System;
using Configs;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Timer
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private CallService _callService;
        [SerializeField] private GameConfiguration _gameConfiguration;

        private void Update()
        {
            float time = _callService.CurrentGameTime / _gameConfiguration.GameDuration;

            _fillImage.fillAmount = 1f - time;
            _handle.transform.rotation = Quaternion.Euler(0, 0, -time * 360);
            
        }
    }
}