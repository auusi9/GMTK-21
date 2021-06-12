using System.Linq;
using Jacks;
using Plugs;
using Services;
using UnityEngine;

namespace Machine
{
    public class PhonePlugs : MonoBehaviour
    {
        [SerializeField] private Plug[] _plugs;
        [SerializeField] private Jack[] _jacks;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _minimumContactDistance;
        [SerializeField] private CallService _callService;
        
        private void Start()
        {
            CallService.NewCall += NewCall;
            CallService.CallEnded += CallEnded;
            CallService.CallMissed += CallMissed;
            CallService.CallInterrupted += CallInterrputed;
            
            foreach (var plug in _plugs)
            {
                plug.Configure(_canvas, (RectTransform) transform);
                plug.JoinToJack += LookForJack;
            }

            CallService.Person[] people = _callService.People;
            for (var i = 0; i < people.Length; i++)
            {
                var jack = _jacks[i];
                jack.Initalize(people[i]);
                jack.LightOff();
            }
        }

        private void OnDestroy()
        {
            CallService.NewCall -= NewCall;
            CallService.CallEnded -= CallEnded;
            CallService.CallMissed -= CallMissed;
            
            foreach (var plug in _plugs)
            {
                plug.Configure(_canvas, (RectTransform) transform);
                plug.JoinToJack -= LookForJack;
            }
        }

        private void LookForJack(Plugs.Plug plug)
        {
            Jack closestJack = GetClosestJack(plug.ContactPosition);

            if (closestJack == null)
            {
                plug.GoToOrigin();
                plug.PairPlug.LightOff();
                return;
            }

            plug.ConnectToJack(closestJack);
            closestJack.PlugConnected(plug);

            Call call = _callService.GetPersonCall(closestJack.Person);

            if (call == null)
            {
                return;
            }

            if (call.InputPerson == closestJack.Person)
            {
                //Show message
                call.ConnectInputPlug(plug);
            }
            else
            {
                call.ConnectOutputPlug(plug);
            }

            if (call.InputPlug != null && call.OutputPlug != null)
            {
                call.ConnectCall();
                plug.PairPlug.LightOn();
                call.OutputPlug.Jack.LightOn();
            }
        }
        
        private Jack GetClosestJack(Vector3 position)
        {
            Jack closestJack = null;
            float minDist = _minimumContactDistance;
            Vector3 currentPos = position;
            foreach (Jack t in _jacks)
            {
                float dist = Vector3.Distance(t.JackPosition, currentPos);
                if (dist < minDist && !t.Person.JackConnected)
                {
                    closestJack = t;
                    minDist = dist;
                }
            }
            return closestJack;
        }
        
        private void NewCall(Call call)
        {
            Jack inputJack = _jacks.FirstOrDefault(x => x.Person == call.InputPerson);
            inputJack?.LightOn();
        }

        private void CallEnded(Call call)
        {
            call.InputPlug.Jack.LightOff();
            call.OutputPlug.Jack.LightOff();
            call.InputPlug.PairPlug.LightOff();
            call.CallEnded();
        }
        
        private void CallMissed(Call call)
        {
            Jack inputJack = _jacks.FirstOrDefault(x => x.Person == call.InputPerson);
            inputJack?.LightOff(); 
            call.CallEnded();
        }

        private void CallInterrputed(Call call)
        {
            Jack inputJack = _jacks.FirstOrDefault(x => x.Person == call.InputPerson);
            inputJack?.LightOff();
            Jack outputJack = _jacks.FirstOrDefault(x => x.Person == call.OutputPerson);
            outputJack?.LightOff(); 
            call.CallEnded();
        }
    }
}
