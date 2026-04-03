using System;
using System.Globalization;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class PlayerTriggerItem : MonoBehaviour
{
    public Action<int> m_onItemExp;
    public Action<int> m_onItemDecreaseHunger;
    public Action<int> m_onItemLostLife;

    public Action<string> m_onItemExpName;
    public Action<int> m_onItemExpValue;

    public Action<string> m_onItemHungerName;

    public Action<string, int> m_addDictionnary;

    public void SetDependencies(GameController gameController)
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemExp"))
        {
            ItemsData item = other.GetComponent<ItemsData>();
            m_onItemExp?.Invoke(item.m_addExperience);
            m_onItemExpName?.Invoke(other.gameObject.name);

            m_addDictionnary?.Invoke(other.gameObject.name, item.m_addExperience);

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ItemConsumable"))
        {
            ItemsData item = other.GetComponent<ItemsData>();
            m_onItemDecreaseHunger?.Invoke(item.m_decreaseHunger);

            m_onItemHungerName?.Invoke(other.gameObject.name);

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ItemSpecial"))
        {
            ItemsData item = other.GetComponent<ItemsData>();
            m_onItemExp?.Invoke(item.m_addExperience);
            m_onItemDecreaseHunger?.Invoke(item.m_decreaseHunger);
            m_onItemLostLife?.Invoke(item.m_lostLife);
            Destroy(other.gameObject);
        }
    }
}
