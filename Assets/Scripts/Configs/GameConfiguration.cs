using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Config/GameConfiguration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public float GameDuration;
        public float TimeBetweenCalls;
        public float TimeToConnect;
        public float CallTime;
    }
}