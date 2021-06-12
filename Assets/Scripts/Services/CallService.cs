using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Jacks;
using Plugs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class CallService : MonoBehaviour
    {
        [SerializeField] private GameConfiguration _gameConfiguration;
        private Person[] _people;
        public Person[] People => _people;

        private List<Call> _onGoingCalls = new List<Call>();
        private float _lastCallTime;
        
        public static event Action<Call> NewCall;
        public static event Action<Call> CallEnded;
        public static event Action<Call> CallMissed;
        public static event Action<Call> CallInterrupted;

        private void Awake()
        {
            _people = PeopleGeneratorService.GeneratePeople();
        }

        private void Update()
        {
            if (_gameConfiguration.TimeBetweenCalls < _lastCallTime)
            {
                GenerateCall();
            }

            List<Call> _callsToRemove = new List<Call>();
            foreach (var call in _onGoingCalls)
            {
                Call callToRemove = UpdateCall(call);
                if (callToRemove != null)
                {
                    _callsToRemove.Add(callToRemove);
                }
            }

            foreach (var call in _callsToRemove)
            {
                call.CallInterrupted -= CallInterruptedHandler;
                _onGoingCalls.Remove(call);
            }
            
            _lastCallTime += Time.deltaTime;
        }

        private Call UpdateCall(Call call)
        {
            call.UpdateTimers();

            if (call.TimeToConnect < 0)
            {
                CallMissed?.Invoke(call);
                return call;
            }

            if (call.CallTime < 0)
            {
                CallEnded?.Invoke(call);
                return call;
            }

            return null;
        }

        private void GenerateCall()
        {
            List<Person> freePeople = _people.Where(x => !x.InCall && !x.AwaitingToBeCalled).ToList();

            if (freePeople.Count < 2)
            {
                return;
            }
            
            Person newInputPerson = freePeople[Random.Range(0, freePeople.Count)];
            newInputPerson.InCall = true;
            freePeople.Remove(newInputPerson);
            Person newOutputPerson = freePeople[Random.Range(0, freePeople.Count)];
            newOutputPerson.AwaitingToBeCalled = true;
            
            Call call = new Call(newInputPerson, newOutputPerson, _gameConfiguration.TimeToConnect, _gameConfiguration.CallTime);
            _onGoingCalls.Add(call);
            NewCall?.Invoke(call);
            call.CallInterrupted += CallInterruptedHandler;
            _lastCallTime = 0f;
            
            Debug.Log(" NEW CALL: From: " + call.InputPerson.Id + " To: " + call.OutputPerson.Id);
        }

        private void CallInterruptedHandler(Call call)
        {
            call.CallInterrupted -= CallInterruptedHandler;
            CallInterrupted?.Invoke(call);
            _onGoingCalls.Remove(call);
        }

        public Call GetPersonCall(Person closestJackPerson)
        {
            return _onGoingCalls.FirstOrDefault(x =>
                x.InputPerson == closestJackPerson || x.OutputPerson == closestJackPerson);
        }
        
        public class Person
        {
            public string Id;
            public string Name;
            public string Surname;
            public string Address;
            public bool InCall;
            public bool AwaitingToBeCalled;
            public Jack JackConnected;
        }
    }
}