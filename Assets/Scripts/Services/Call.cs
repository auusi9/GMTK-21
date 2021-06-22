using System;
using Plugs;
using UnityEngine;

namespace Services
{
    public class Call
    {
        public CallService.Person InputPerson;
        public CallService.Person OutputPerson;
        public float TimeToConnect;
        public float CallTime;
        public bool CallConnected;
        public int Score;

        public Plug InputPlug { get; private set; }
        public Plug OutputPlug { get; private set; }
        public event Action<Call> CallInterrupted;

        public Call(CallService.Person inputPerson, CallService.Person outputPerson, float timeToConnect, float callTime)
        {
            InputPerson = inputPerson;
            OutputPerson = outputPerson;
            TimeToConnect = timeToConnect;
            CallTime = callTime;
            Score = 0;
        }

        public void ConnectCall()
        {
            CallConnected = true;
            InputPerson.InCall = true;
            InputPerson.AwaitingToBeCalled = false;
                
            OutputPerson.InCall = true;
            OutputPerson.AwaitingToBeCalled = false;
        }

        public void ConnectInputPlug(Plug plug)
        {
            InputPlug = plug;
            InputPlug.JackDisconnected += InputPlugDisconnected;
        }
        
        public void ConnectOutputPlug(Plug plug)
        {
            OutputPlug = plug;
            OutputPlug.JackDisconnected += OutputPlugDisconnected;
        }

        private void InputPlugDisconnected()
        {
            if (InputPlug == null)
            {
                return;
            }
            
            InputPlug.JackDisconnected -= InputPlugDisconnected;
            InputPlug = null;
            IsCallInterrupted();
        }
        
        private void OutputPlugDisconnected()
        {
            if (OutputPlug == null)
            {
                return;
            }
            
            OutputPlug.JackDisconnected -= OutputPlugDisconnected;
            OutputPlug = null;
            IsCallInterrupted();
        }

        private void IsCallInterrupted()
        {
            if (CallConnected)
            {
                CallInterrupted?.Invoke(this);
                CallConnected = false;
            }
        }

        public void UpdateTimers()
        {
            if (CallConnected)
            {
                CallTime -= Time.deltaTime;
                return;
            }

            TimeToConnect -= Time.deltaTime;
        }

        public void CallEnded()
        {
            CallConnected = false;
            InputPerson.InCall = false;
            InputPerson.AwaitingToBeCalled = false;
                
            OutputPerson.InCall = false;
            OutputPerson.AwaitingToBeCalled = false;
        }
    }
}