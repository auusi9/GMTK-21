using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "JacksConfiguration", menuName = "Config/JacksConfiguration", order = 0)]
    public class JacksConfiguration : ScriptableObject
    {
        [SerializeField] private Color _lightOn;
        [SerializeField] private Color _lightOff;

        public Color LightOn => _lightOn;
        public Color LightOff => _lightOff;
    }
}