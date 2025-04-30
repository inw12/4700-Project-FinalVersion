using System;
using UnityEngine;

[CreateAssetMenu]
public class Vector2Value : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private Vector2 defaultValue;
    [NonSerialized] public Vector2 runtimeValue;

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
