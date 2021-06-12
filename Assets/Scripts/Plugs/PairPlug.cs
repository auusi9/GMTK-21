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
        [SerializeField] private Image _light;
        [SerializeField] private JacksConfiguration _jacksConfiguration;

        public Plug InputPlug => _inputPlug;

        public Plug OutputPlug => _outputPlug;

        public void LightOn()
        {
            _light.color = _jacksConfiguration.LightOn;
        }

        public void LightOff()
        {
            _light.color = _jacksConfiguration.LightOff;
        }
    }
}