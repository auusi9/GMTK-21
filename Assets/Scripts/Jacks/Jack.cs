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
        [SerializeField] private Image _lightGlow;
        [SerializeField] private Transform _jackPosition;

        public CallService.Person Person => _person;
        private CallService.Person _person;
        private Plug _plug;
        
        public void Initalize(CallService.Person person)
        {
            _person = person;
        }
        
        public Vector3 JackPosition => _jackPosition.position;

        public void LightOn()
        {
            _lightGlow.gameObject.SetActive(true);
        }

        public void LightOff()
        {
            _lightGlow.gameObject.SetActive(false);
        }

        public void PlugConnected(Plug plug)
        {
            _person.PlugConnected = plug;
        }

        public void PlugDisconnected()
        {
            _person.PlugConnected = null;
        }
    }
}
