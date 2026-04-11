using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public ItemsController.ItemType m_itemType { get; private set; }
    [field: SerializeField] public int m_amout { get; private set; }
}
