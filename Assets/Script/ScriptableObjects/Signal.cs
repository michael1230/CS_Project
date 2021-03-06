﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Signal : ScriptableObject/*A ScriptableObject is a data container that you can use to save large amounts of data*/
{

    public List<SignalListener> listeners = new List<SignalListener>();//a list of signal listener
    
    public void Raise()//loops through all the list and raises a signal
    {
        for (int i = listeners.Count - 1; i >= 0; i--)//starts from the end to make sure that in case the listener remove itself from the list it wont cause range exception
        {
            listeners[i].OnSignalRaised();//do what needs to do for the signal
        }
    }
    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener);//adds the particular listener
    }
    public void DeRegisterListener(SignalListener listener)
    {
        listeners.Remove(listener);//removes the particular listener
    }
}
