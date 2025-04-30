using System;
using UnityEngine;

[CreateAssetMenu]
public class MinMaxVectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private Vector2 defaultMax;
    [SerializeField] private Vector2 defaultMin;
    [NonSerialized] public Vector2 runtimeMax;
    [NonSerialized] public Vector2 runtimeMin;

    private void OnEnable() {
        ResetRuntimeValues();
    }

    public void ResetRuntimeValues() {
        runtimeMax = defaultMax;
        runtimeMin = defaultMin;
    }

    public void OnAfterDeserialize() {
        ResetRuntimeValues();
    }

    public void OnBeforeSerialize() {}
}