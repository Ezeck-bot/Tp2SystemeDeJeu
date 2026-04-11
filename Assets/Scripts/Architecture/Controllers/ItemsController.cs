using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    public enum ItemType
    {
        Quest,
        Food,
        Special,
        Craftable
    }

    public Action<int> m_onExperienceItemGatered;
    public Action<int> m_onFoodItemGatered;
    public Action<int> m_onHpLostItemGatered;

    [SerializeField] private Item[] m_items;

    //list
    private HashSet<string> m_NameItemExp = new HashSet<string> { };

    //
    private List<string> m_NameItemHunger = new();

    //
    private Dictionary<string, int> m_dic = new Dictionary<string, int>();
    public Action<Dictionary<string, int>> m_onInventory;

    //
    public Action m_OnNotifyItemQueteDone;
    public Action m_OnNotifyItemHungerDone;
    public Action m_OnNotifyDictionnary;

    public void SetDependencies()
    {
        foreach (Item item in m_items)
        {
            item.Init(this);
        }
    }

    public void GatherItem(string name, ItemType itemType, int amout)
    {
        switch (itemType)
        {
            case ItemType.Quest:
                AudioManager.Instance.PlayAudio(SoundID.ITEMEXP);

                //--
                m_onExperienceItemGatered?.Invoke(amout);

                //--
                CompileNameItemExp(name);

                //--
                CompileItemDictionnary(name, amout);

                //--

                break;
            case ItemType.Food:
                AudioManager.Instance.PlayAudio(SoundID.ITEMLIFE);

                //--
                m_onFoodItemGatered?.Invoke(amout);

                //--
                CompileNameItemHunger(name);

                //--
                CompileItemDictionnary(name, amout);

                //--

                break;
            case ItemType.Special:
                //--
                AudioManager.Instance.PlayAudio(SoundID.ITEMSPECIAL);

                m_onExperienceItemGatered?.Invoke(amout);
                m_onFoodItemGatered?.Invoke(amout);
                m_onHpLostItemGatered?.Invoke(amout);


                break;
            case ItemType.Craftable:
                //--
                AudioManager.Instance.PlayAudio(SoundID.ITEMCRAFTING);

                CompileItemDictionnary(name, amout);

                break;
            default:
                break;

        }

    }

    private void CompileNameItemExp(string name)
    {
        if (m_NameItemExp.Add(name))
        {
            m_OnNotifyItemQueteDone?.Invoke();
        }
        else
        {
            Debug.Log("Error hashset");
        }
    }


    private void CompileNameItemHunger(string name)
    {
        m_NameItemHunger.Add(name);
        m_OnNotifyItemHungerDone?.Invoke();
    }

    public void CompileItemDictionnary(string name, int value)
    {
        if (m_dic.TryAdd(name, value))
        {
            m_OnNotifyDictionnary?.Invoke();
        }
        else
        {
            Debug.Log("Error dic");
        }
    }

    public void PublishInventory()
    {
        m_onInventory?.Invoke(m_dic);
    }

    //SECTION CRAFTING
    //Savoir si l'item est dans le dictionnaire
    public bool HasItem(string name)
    {
        return m_dic.ContainsKey(name);
    }

    //Supprimer l'item
    public bool HasDeleteItem(string name)
    {
        return m_dic.Remove(name);
    }
}
