using System.Collections;
using Configs;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Score
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private float _timeToDeactivate;
        [SerializeField] private Image _image;
        [SerializeField] private Color _green;
        [SerializeField] private Color _yellow;
        [SerializeField] private Color _orange;
        [SerializeField] private Color _red;

        private void Awake()
        {
            CallService.NewScore += NewScore;
            _textMeshProUGUI.text = "0";
        }

        private void OnDestroy()
        {
            CallService.NewScore -= NewScore;
        }

        private void NewScore(int totalScore, int newScore)
        {
            StopAllCoroutines();
            if (totalScore == 0)
            {
                _textMeshProUGUI.text = "0";
            }
            else
            {
                _textMeshProUGUI.text = totalScore.ToString("N0");
            }

            if (newScore > _gameConfiguration.MaxScore * 0.75f)
            {
                _image.color = _green;
            }
            else if(newScore > _gameConfiguration.MaxScore * 0.45f)
            {
                _image.color = _yellow;
            }
            else if (newScore > 0)
            {
                _image.color = _orange;
            }
            else
            {
                _image.color = _red;
            }
            
            _image.gameObject.SetActive(true);
            StartCoroutine(DeactivateLightAfterTime());
        }

        private IEnumerator DeactivateLightAfterTime()
        {
            yield return new WaitForSeconds(_timeToDeactivate);
            _image.gameObject.SetActive(false);
        }
    }
}