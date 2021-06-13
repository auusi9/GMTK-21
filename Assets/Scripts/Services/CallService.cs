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
        public float CurrentGameTime => _gameTime;

        private List<Call> _onGoingCalls = new List<Call>();
        private float _lastCallTime;
        private float _gameTime;

        public static event Action<Call> NewCall;
        public static event Action<Call> CallEnded;
        public static event Action<Call> CallMissed;
        public static event Action<Call> CallInterrupted;
        public static event Action<int, int> NewScore;

        private float _nextCall = 0f;
        private int _score;
        
        private void Awake()
        {
            _people = PeopleGeneratorService.GeneratePeople();
            _nextCall = 1f;
        }

        private void Update()
        {
            if (_nextCall < _lastCallTime)
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
            _gameTime += Time.deltaTime;
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
            List<Person> freePeople = _people.Where(x => !x.InCall && !x.AwaitingToBeCalled && !x.PlugConnected).ToList();

            if (freePeople.Count < 2)
            {
                return;
            }
            
            Person newInputPerson = freePeople[Random.Range(0, freePeople.Count)];
            newInputPerson.InCall = true;
            freePeople.Remove(newInputPerson);
            Person newOutputPerson = freePeople[Random.Range(0, freePeople.Count)];
            newOutputPerson.AwaitingToBeCalled = true;
            
            Call call = new Call(newInputPerson, newOutputPerson, _gameConfiguration.TimeToConnect, Random.Range(_gameConfiguration.CallTime.x, _gameConfiguration.CallTime.y));
            _onGoingCalls.Add(call);
            NewCall?.Invoke(call);
            call.CallInterrupted += CallInterruptedHandler;
            _lastCallTime = 0f;
            _nextCall = Random.Range(_gameConfiguration.TimeBetweenCalls.x, _gameConfiguration.TimeBetweenCalls.y);
            Debug.Log(" NEW CALL: From: " + call.InputPerson.Id + " To: " + call.OutputPerson.Id);
        }

        private void CallInterruptedHandler(Call call)
        {
            call.CallInterrupted -= CallInterruptedHandler;
            CallInterrupted?.Invoke(call);
            _onGoingCalls.Remove(call);
            int pen = (int) (call.Score * 0.15f);
            _score -= pen;
            NewScore?.Invoke(_score, pen);
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
            public bool IsCity;
            public Plug PlugConnected;
        }

        public void CallConnected(Call call)
        {
            float percentage = call.TimeToConnect / _gameConfiguration.TimeToConnect;
            call.Score = (int) (percentage > 2/3f ? _gameConfiguration.MaxScore : percentage > 1/3f ? _gameConfiguration.MaxScore * 0.75f : _gameConfiguration.MaxScore * 0.45f);
            _score += call.Score;
            NewScore?.Invoke(_score, call.Score);
        }
    }
}