using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour {

    public Signal signal; //the signal to listen
    public UnityEvent signalEvent; //zero argument persistent callback that can be saved with the Scene

    public void OnSignalRaised()
    {
        signalEvent.Invoke(); // calls the present event
    }

    private void OnEnable() //going to the signal and register
    {
        signal.RegisterListener(this);
    }
    private void OnDisable() //going to the signal and disables
    {
        signal.DeRegisterListener(this);
    }
}
