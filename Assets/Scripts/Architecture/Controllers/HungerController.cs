using System;
using System.Collections;
using UnityEngine;

public class HungerController : MonoBehaviour
{
    public Action<int> m_onHungerChange;

    private const int MAX_HUNGER = 100;

    private int m_currentHunger;

    private float m_currentHungerTimeSpeed;

    private HpController m_hpController;
    private ItemsController m_itemsController;
    private PlayerController m_playerController;

    [SerializeField] private PlayerScriptableObject m_playerScriptableObject;
    [SerializeField] private HungerScriptableObject m_hungerScriptableObject;

    public void SetDependencies(GameController gameController)
    {
        //initilaiser
        m_currentHungerTimeSpeed = m_hungerScriptableObject.m_hungerTimeSpeed;

        m_hpController = gameController.m_hpController;
        m_itemsController = gameController.m_itemsController;
        m_playerController = gameController.m_playerController;


        m_itemsController.m_onFoodItemGatered += DiscreaseHunger;

        m_playerController.m_onSprint += ChangeTimeSpeedHunger;
    }

    public void OnDestroy()
    {
        m_itemsController.m_onFoodItemGatered -= DiscreaseHunger;

        m_playerController.m_onSprint -= ChangeTimeSpeedHunger;
    }

    public void ChangeTimeSpeedHunger(float m_timeSpeed)
    {
        m_currentHungerTimeSpeed = m_timeSpeed;
    }

    public void CompileHunger(int hung)
    {

        //toute la logique sur la compilation d'hunger se passe ici
        m_currentHunger += hung;

        m_currentHunger = Mathf.Clamp(m_currentHunger, 0, MAX_HUNGER);

        m_onHungerChange?.Invoke(m_currentHunger);
    }

    public IEnumerator IncrementHunger()
    {
        //logique de faim
        while (true)
        {
            yield return new WaitForSeconds(m_currentHungerTimeSpeed);
            CompileHunger(+1);

            if (m_currentHunger >= MAX_HUNGER) { 
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

        if (m_currentHunger < 100)
        {
            m_hpController.StopLosingHp();

            StartCoroutine(IncrementHunger());
        }

    }
}
