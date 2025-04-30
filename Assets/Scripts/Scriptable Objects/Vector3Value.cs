using System;
using UnityEngine;

[CreateAssetMenu]
public class Vector3Value : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private Vector3 defaultValue;
    [NonSerialized] public Vector3 runtimeValue;

    private void OnEnable() {
        ResetRuntimeValue();        
    }

    public void ResetRuntimeValue() {
        runtimeValue = defaultValue;
    }

    public void OnAfterDeserialize() {
        ResetRuntimeValue();
    }

    public void OnBeforeSerialize() {}
}