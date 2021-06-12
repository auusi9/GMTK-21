﻿using System;
using Configs;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Notifications
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _notificationText;
        [SerializeField] private Image _barFill;
        [SerializeField] private GameConfiguration _gameConfiguration;

        private Call _call;

        public void SetCall(Call call)
        {
            _call = call;
        }

        private void Update()
        {
            if (_call == null)
            {
                return;
            }

            if (_call.CallConnected ||  _call.TimeToConnect < 0)
            {
                Destroy(gameObject);
                return;
            }
            
            if (_call.InputPlug == null)
            {
                _notificationText.text = "...";
            }
            else
            {
                _notificationText.text = _call.OutputPerson.Id;
            }

            _timeText.text = _call.TimeToConnect.ToString("N0");
            _barFill.fillAmount = _call.TimeToConnect / _gameConfiguration.TimeToConnect;
        }
    }
}