using System;
using System.Collections;
using UnityEngine;

public class HungerController : MonoBehaviour
{
    public Action<int> m_onHungerChange;

    private int m_Hunger;

    [SerializeField] private float m_hungerTimeSpeed;

    private HpController m_hpController;
    private ItemsController m_itemsController;
    private PlayerController m_playerController;

    private PlayerTriggerItem m_playerTriggerItem;

    public void SetDependencies(GameController gameController)
    {

        m_hpController = gameController.m_hpController;
        m_itemsController = gameController.m_itemsController;
        m_playerController = gameController.m_playerController;


        m_itemsController.m_onHungerGained += CompileHunger;
        m_playerController.m_onSprint += ChangeTimeSpeedHunger;

        m_playerTriggerItem = gameController.m_playerTriggerItem;
        m_playerTriggerItem.m_onItemDecreaseHunger += DiscreaseHunger;
    }

    public void OnDestroy()
    {
        m_itemsController.m_onHungerGained -= CompileHunger;
        m_playerController.m_onSprint -= ChangeTimeSpeedHunger;

        m_playerTriggerItem.m_onItemDecreaseHunger -= DiscreaseHunger;
    }

    public void ChangeTimeSpeedHunger(float m_timeSpeed)
    {
        m_hungerTimeSpeed = m_timeSpeed;
    }

    public void CompileHunger(int hung)
    {

        //toute la logique sur la compilation d'hunger se passe ici
        m_Hunger += hung;
        m_Hunger = Mathf.Clamp(m_Hunger, 0, 100);
        m_onHungerChange?.Invoke(m_Hunger);
    }

    public IEnumerator IncrementHunger()
    {
        //logique de faim
        while (true)
        {
            yield return new WaitForSeconds(m_hungerTimeSpeed);
            CompileHunger(+1);

            if (m_Hunger >= 100) { 
                IsHungry();
                yield break;
            }
        }

    }

    private void IsHungry()
    {
        m_hpController.StartLosingHp();
    }

    public void DiscreaseHunger(int discreaseHunger)
    {        
        CompileHunger(-discreaseHunger);

        if (m_Hunger < 100)
        {
            m_hpController.StopLosingHp();

            StartCoroutine(IncrementHunger());
        }

    }
}
