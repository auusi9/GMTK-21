using System.Linq;
using UnityEngine;

namespace Machine
{
    public class PhonePlugs : MonoBehaviour
    {
        [SerializeField] private Plug.Plug[] _plugs;
        [SerializeField] private Transform[] _jacks;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _minimumContactDistance;

        private void Start()
        {
            foreach (var plug in _plugs)
            {
                plug.Configure(_canvas, (RectTransform) transform);
                plug.JoinToJack += LookForJack;
            }
        }

        private void LookForJack(Plug.Plug plug)
        {
            Transform closestJack = GetClosestJack(plug.ContactPosition);

            if (closestJack == null)
            {
                plug.GoToOrigin();
                return;
            }

            plug.ConnectToJack(closestJack);
        }
        
        Transform GetClosestJack(Vector3 position)
        {
            Transform closestJack = null;
            float minDist = _minimumContactDistance;
            Vector3 currentPos = position;
            foreach (Transform t in _jacks)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    closestJack = t;
                    minDist = dist;
                }
            }
            return closestJack;
        }
    }
}
