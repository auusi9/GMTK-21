using System;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Score
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _wellDoneMessage;
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private Button _restartButton;

        private void Start()
        {
            _restartButton.onClick.AddListener(RestarGame);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(RestarGame);
        }

        private void RestarGame()
        {
            SceneManager.LoadScene(0);
        }

        public void SetScore(int score)
        {
            if (score == 0)
            {
                _scoreText.text = "0";
            }
            else
            {
                _scoreText.text = score.ToString("N0");
            }

            if (score >= _gameConfiguration.ExpectedScore)
            {
                _wellDoneMessage.text = "Well done!";
            }
            else
            {
                _wellDoneMessage.text = "Better luck next time! loser..";
            }
        }
    }
}