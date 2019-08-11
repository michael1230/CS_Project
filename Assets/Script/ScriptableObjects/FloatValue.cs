using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// can be active through different scenes
[CreateAssetMenu]//allows to create this as an object 
public class FloatValue : ScriptableObject/*can be read in multiple scenes*/, ISerializationCallbackReceiver/*Interface to receive callbacks upon serialization and deserialization*/
{
   public float initialValue;

    [HideInInspector]
    public float RuntimeValue;

    public void OnAfterDeserialize()//for when it loads objects from memory to start with the value for the whole game
    { 
        RuntimeValue = initialValue; 
    }
    public void OnBeforeSerialize() {}//after everything unloads from the game
}
