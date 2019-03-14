using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Signal : ScriptableObject {

    public List<SignalListener> listeners = new List<SignalListener>(); // a list of signal listener
    
    public void Raise()// loops through all the list and raises a signal
    {
        for (int i = listeners.Count - 1; i >= 0; i--)// starts from the end
        {
            listeners[i].OnSignalRaised();
        }
    }
    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener); // adds the particular listener
    }
    public void DeRegisterListener(SignalListener listener)
    {
        listeners.Remove(listener); // removes the particular listener
    }
}
