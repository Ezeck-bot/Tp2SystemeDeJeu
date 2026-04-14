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
    [SerializeField] public int m_currentLife;
    [SerializeField] private int m_maxLife;

    private Coroutine m_losingHpCoroutine;

    public void SetDependencies(GameController gameController)
    {
        m_itemController = gameController.m_itemsController;

        m_itemController.m_onInventory += ShowInventory;

        m_experienceController = gameController.m_experienceController;
        m_experienceController.m_onLevelUp += IncrementMaxHp;

        //special item
        m_itemController.m_onHpLostItemGatered += DecrementMaxHp;
    }

    public void OnDestroy()
    {
        m_experienceController.m_onLevelUp -= IncrementMaxHp;

        m_itemController.m_onHpLostItemGatered += DecrementMaxHp;

        m_itemController.m_onInventory -= ShowInventory;
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
        m_currentLife += amout;

        m_currentLife = Mathf.Clamp(m_currentLife, 0, m_maxLife);

        m_onHpChange?.Invoke(m_currentLife); //publier
    }

    public IEnumerator LosingHpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_LifeTimeSpeed);
            CompileHp(-10);

            if (m_currentLife <= 0)
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
