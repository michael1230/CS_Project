using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// can be active through different sences
[CreateAssetMenu] //allows to create this as an object 
public class FloatValue : ScriptableObject/*can be read in multiple scenes*/, ISerializationCallbackReceiver/*Interface to receive callbacks upon serialization and deserialization*/
{

   public float initialValue;

    [HideInInspector]
    public float RuntimeValue;

    public void OnAfterDeserialize()//when deserialize
    { 
        RuntimeValue = initialValue; 
    }

    public void OnBeforeSerialize() {}
}
