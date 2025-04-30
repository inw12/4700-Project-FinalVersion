using System;
using UnityEngine;

[CreateAssetMenu]   
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    public float value;
    [NonSerialized] public float runtimeValue;

    private void OnEnable() {
        ResetRuntimeValue();
    }

    public void ResetRuntimeValue() {
        runtimeValue = value;
    }

    public void OnAfterDeserialize() {
        ResetRuntimeValue();
    } 

    public void OnBeforeSerialize() {}
}
