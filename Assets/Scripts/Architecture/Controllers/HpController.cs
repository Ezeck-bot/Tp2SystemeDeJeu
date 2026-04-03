using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    public Action<int> m_onHpChange;
    public Action<bool> m_onDied;

    private ItemsController m_itemController;
    private ExperienceController m_experienceController;
    //quand un item est rammassé et quand on a level up

    [SerializeField] private float m_LifeTimeSpeed;
    [SerializeField] private int m_life;

    private PlayerTriggerItem m_playerTriggerItem;

    private Coroutine m_losingHpCoroutine;

    public void SetDependencies(GameController gameController)
    {
        m_itemController = gameController.m_itemsController;
        m_itemController.m_onHpGained += CompileHp;

        m_itemController.m_onInventory += ShowInventory;

        m_experienceController = gameController.m_experienceController;
        m_experienceController.m_onLevelUp += IncrementMaxHp;

        m_playerTriggerItem = gameController.m_playerTriggerItem;
        m_playerTriggerItem.m_onItemLostLife += DecrementMaxHp;
    }

    public void OnDestroy()
    {
        m_itemController.m_onHpGained -= CompileHp;
        m_experienceController.m_onLevelUp -= IncrementMaxHp;

        m_itemController.m_onInventory -= ShowInventory;

        m_playerTriggerItem.m_onItemLostLife -= DecrementMaxHp;
    }

    public void StartLosingHp()
    {
        if (m_losingHpCoroutine == null)
        {
            m_losingHpCoroutine = StartCoroutine(LosingHpRoutine());
        }
    }

    public void StopLosingHp()
    {
        if (m_losingHpCoroutine != null)
        {
            StopCoroutine(m_losingHpCoroutine);
            m_losingHpCoroutine = null;
        }
    }

    public void IncrementMaxHp(int life)
    {
        CompileHp(+10);
    }

    public void DecrementMaxHp(int life)
    {
        CompileHp(-life);
    }

    public void CompileHp(int amout)
    {
        //toute la compilation des hp se passe ici

        //calculer les nouveaux hp
        m_life += amout;
        m_life = Mathf.Clamp(m_life, 0, 130);
        m_onHpChange?.Invoke(m_life); //publier
    }

    public IEnumerator LosingHpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_LifeTimeSpeed);
            CompileHp(-10);

            if (m_life <= 0)
            {
                onDied();
                m_onDied?.Invoke(true);
                yield break;
            }
        }
    }

    private void ShowInventory(Dictionary<string, int> inventory)
    {
        foreach (var item in inventory)
        {
            Debug.Log($"Item: {item.Key} | Quantité: {item.Value}");
        }
    }

    public void onDied()
    {
        m_itemController.PublishInventory();
    }


}
