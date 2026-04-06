using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Scriptable Objects/PlayerScriptableObject")]
public class PlayerScriptableObject: ScriptableObject
{

    [field: SerializeField] public float m_moveBoostSpeed { get; private set; }

    [field: SerializeField] public float m_walkSpeed { get; private set; }
}
