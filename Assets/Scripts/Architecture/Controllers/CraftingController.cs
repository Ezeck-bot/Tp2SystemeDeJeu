using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    [SerializeField] private RecetteCraftingData m_recetteCraftingStation;

    [SerializeField] private RecetteCraftingData m_recetteCraftingStation2;

    //
    [SerializeField] private CraftingStation1 m_sations;

    [SerializeField] private CraftingStation2 m_sations2;

    //
    private ItemsController m_itemController;

    public Action<float> m_OnNotifySliderCrafting;
    public Action m_OnNotifyNoSliderCrafting;

    public Action<bool> m_OnNotifyNoCraftingItem;

    public Action<bool> m_OnNotifyPressCPalyer;

    private bool m_couroutineIsStart = false;
    private bool m_isCrafting = false;

    public void SetDependencies(GameController gameController)
    {
        m_sations.Init(this);
        m_sations2.Init(this);

        m_itemController = gameController.m_itemsController;
    }

    public void ActiveCrafting(bool active)
    {
        m_couroutineIsStart = active;

        if (!m_couroutineIsStart)
        {
            AudioManager.Instance.StopAudio(SoundID.CRAFTING);
            StopAllCoroutines();
            m_isCrafting = false;
            m_OnNotifyNoSliderCrafting?.Invoke();
        }
    }

    public void OnPlayerEnterStation1()
    {
        if (m_itemController.HasItem(m_recetteCraftingStation.m_item1.name) && m_itemController.HasItem(m_recetteCraftingStation.m_item2.name))
        {
            //Debug.Log("Press C");
            m_OnNotifyPressCPalyer?.Invoke(true);
            if (m_couroutineIsStart && !m_isCrafting)
            {
                m_isCrafting = true;

                //--
                AudioManager.Instance.PlayAudio(SoundID.CRAFTING);
                //lancer le crafting
                StartCoroutine(CraftingCoroutine(m_recetteCraftingStation.m_timeCrafting, m_recetteCraftingStation.m_item1.name, m_recetteCraftingStation.m_item2.name, m_recetteCraftingStation.m_resultCrafting));

            }
        } else
        {
            m_OnNotifyNoCraftingItem?.Invoke(true);
        }
    }


    public void OnPlayerEnterStation2()
    {
        if (m_itemController.HasItem(m_recetteCraftingStation2.m_item1.name) && m_itemController.HasItem(m_recetteCraftingStation2.m_item2.name))
        {

            
            m_OnNotifyPressCPalyer?.Invoke(true);
            if (m_couroutineIsStart && !m_isCrafting)
            {
                m_isCrafting = true;

                AudioManager.Instance.PlayAudio(SoundID.CRAFTING);
                //lancer le crafting
                StartCoroutine(CraftingCoroutine(m_recetteCraftingStation.m_timeCrafting, m_recetteCraftingStation.m_item1.name, m_recetteCraftingStation.m_item2.name, m_recetteCraftingStation.m_resultCrafting));

            }
        }
        else
        {
            m_OnNotifyNoCraftingItem?.Invoke(true);
        }
    }

    public void OnPlayerExitStation1()
    {
        m_OnNotifyPressCPalyer?.Invoke(false);
        m_OnNotifyNoCraftingItem?.Invoke(false);
    }

    public void OnPlayerExitStation2()
    {
        //desac press c
        m_OnNotifyPressCPalyer?.Invoke(false);
        m_OnNotifyNoCraftingItem?.Invoke(false);
    }

    public IEnumerator CraftingCoroutine(float time, string item1, string item2, string result)
    {
        float seconds = 0f;
        while (time >= seconds)
        {
            m_OnNotifySliderCrafting?.Invoke(seconds);

            yield return new WaitForSeconds(1f);
            seconds++;
        }

        m_isCrafting = false;

        m_OnNotifyNoSliderCrafting?.Invoke();

        m_itemController.HasDeleteItem(item1);

        m_itemController.HasDeleteItem(item2);

        m_itemController.CompileItemDictionnary(result, 0);
    }
}
