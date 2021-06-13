using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Config/GameConfiguration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public float GameDuration;
        public Vector2 TimeBetweenCalls;
        public float TimeToConnect;
        public Vector2 CallTime;
        public int MaxScore = 100;
    }
}