using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData m_itemData;
    private ItemsController m_itemsController;

    public void Init(ItemsController itemsController)
    {
        m_itemsController = itemsController; 
    }

    private void OnTriggerEnter(Collider other)
    {
        m_itemsController.GatherItem(gameObject.name, m_itemData.m_itemType, m_itemData.m_amout);
        Destroy(gameObject);
    }
}
