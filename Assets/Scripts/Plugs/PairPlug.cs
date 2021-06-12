using Configs;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Plugs
{
    public class PairPlug : MonoBehaviour
    {
        [SerializeField] private CallService _callService;
        [SerializeField] private Plug _inputPlug;
        [SerializeField] private Plug _outputPlug;
        [SerializeField] private Image _lightGlow;

        public Plug InputPlug => _inputPlug;

        public Plug OutputPlug => _outputPlug;

        public void LightOn()
        {
            _lightGlow.gameObject.SetActive(true);
        }

        public void LightOff()
        {
            _lightGlow.gameObject.SetActive(false);
        }
    }
}