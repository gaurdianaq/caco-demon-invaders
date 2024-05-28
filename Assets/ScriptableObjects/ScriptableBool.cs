using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableBool", menuName = "ScriptableVar/ScriptableBool", order = 1)]
public class ScriptableBool : ScriptableObject
{
    [SerializeField]
    private bool defaultValue;
    [NonSerialized]
    public bool currentValue;

    private void Awake()
    {
        currentValue = defaultValue;
    }
}
