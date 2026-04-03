using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class ItemsController : MonoBehaviour
{

    //création d'action
    public Action<int> m_onHpGained;
    public Action<int> m_onExpGained;
    public Action<int> m_onHungerGained;

    private PlayerController m_playerController;

    private PlayerTriggerItem m_playerTriggerItem;

    //
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

    public void SetDependencies(GameController gameController)
    {
        m_playerController = gameController.m_playerController;

        m_playerTriggerItem = gameController.m_playerTriggerItem;
        m_playerTriggerItem.m_onItemExpName += CompileNameItemExp;

        m_playerTriggerItem.m_onItemHungerName += CompileNameItemHunger;

        m_playerTriggerItem.m_addDictionnary += CompileItemHungerValue;
    }

    public void OnDestroy()
    {
        m_playerTriggerItem.m_onItemExpName -= CompileNameItemExp;

        m_playerTriggerItem.m_onItemHungerName -= CompileNameItemHunger;

        m_playerTriggerItem.m_addDictionnary -= CompileItemHungerValue;
    }

    private void CompileNameItemExp(string name)
    {
        if (m_NameItemExp.Add(name))
        {
            m_OnNotifyItemQueteDone?.Invoke();
        } else
        {
            Debug.Log("Error");
        }
    }


    private void CompileNameItemHunger(string name)
    {
        m_NameItemHunger.Add(name);
        m_OnNotifyItemHungerDone?.Invoke();
    }

    private void CompileItemHungerValue(string name, int value)
    {
        if (m_dic.TryAdd(name, value))
        {
            m_OnNotifyDictionnary?.Invoke();
        } else
        {
            Debug.Log("Error");
        }
    }

    public void PublishInventory()
    {
        m_onInventory?.Invoke(m_dic);
    }

    public void FilterItemType()
    {
        //filter

        m_playerController.ReceivedItem();

        // choisir la bonne  action
    }
}
