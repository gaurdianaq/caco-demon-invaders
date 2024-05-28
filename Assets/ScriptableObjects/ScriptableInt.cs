using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableInt", menuName = "ScriptableVar/ScriptableInt", order = 2)]
public class ScriptableInt : ScriptableObject
{
    [SerializeField]
    private int defaultValue;
    [NonSerialized]
    public int currentValue;

    private void Awake()
    {
        currentValue = defaultValue;
    }
}
