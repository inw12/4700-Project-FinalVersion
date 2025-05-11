using System;
using UnityEngine;

[CreateAssetMenu]   
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver, IResettable
{
    public float value;
    [NonSerialized] public float runtimeValue;

    private void OnEnable() {
        Reset();
    }

    public void Reset() {
        runtimeValue = value;
    }

    public void OnAfterDeserialize() {
        Reset();
    } 

    public void OnBeforeSerialize() {}
}
