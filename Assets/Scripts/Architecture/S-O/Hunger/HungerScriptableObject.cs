using UnityEngine;

[CreateAssetMenu(fileName = "Hunger Data", menuName = "Scriptable Objects/HungerData")]
public class HungerScriptableObject : ScriptableObject
{
    [field: SerializeField] public float m_hungerTimeSpeed { get; private set; }

    [field: SerializeField] public float m_newTimeDescreaseHunger { get; private set; }
}
