using UnityEngine;

[CreateAssetMenu(fileName = "New Rectte Crafting", menuName = "Scriptable Objects/CraftingData")]
public class RecetteCraftingData : ScriptableObject
{
    [field: SerializeField] public ItemData m_item1 {  get; private set; }

    [field: SerializeField] public ItemData m_item2 { get; private set; }

    [field: SerializeField] public string m_resultCrafting { get; private set; }

    [field: SerializeField] public float m_timeCrafting { get; private set; }
}
