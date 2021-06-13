using System;
using Services;
using TMPro;
using UnityEngine;

namespace Score
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            CallService.NewScore += NewScore;
        }

        private void OnDestroy()
        {
            CallService.NewScore -= NewScore;
        }

        private void NewScore(int totalScore, int newScore)
        {
            _textMeshProUGUI.text = totalScore.ToString();
        }
    }
}