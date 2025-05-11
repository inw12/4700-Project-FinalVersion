using UnityEngine;
using System;

[CreateAssetMenu]
public class PlayerWeapon : ScriptableObject, ISerializationCallbackReceiver, IResettable
{
    [SerializeField] private GameObject defaultWeapon;
    [NonSerialized] public GameObject currentWeapon;

    private void OnEnable() {
        Reset();
    }

    public void Reset() {
        currentWeapon = defaultWeapon;
    }

    public void OnAfterDeserialize() {  
        Reset();  
    } 

    public void OnBeforeSerialize() {}
}