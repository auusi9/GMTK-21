using Configs;
using Plugs;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Services;

namespace Jacks
{
    public class Jack : MonoBehaviour
    {
        [SerializeField] private Image _light;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Transform _jackPosition;
        [SerializeField] private JacksConfiguration _jacksConfiguration;

        public CallService.Person Person => _person;
        private CallService.Person _person;
        private Plug _plug;
        
        public void Initalize(CallService.Person person)
        {
            _person = person;
            _text.text = person.Id;
        }
        
        public Vector3 JackPosition => _jackPosition.position;

        public void LightOn()
        {
            _light.color = _jacksConfiguration.LightOn;
        }

        public void LightOff()
        {
            _light.color = _jacksConfiguration.LightOff;
        }

        public void PlugConnected(Plug plug)
        {
            _person.JackConnected = this;
        }

        public void PlugDisconnected()
        {
            _person.JackConnected = null;
        }
    }
}
