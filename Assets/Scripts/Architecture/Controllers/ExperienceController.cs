using System;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    //scriptable object pour les levels d'exprérience
    public Action<int> m_onLevelUp;
    public Action<int> m_onExpGained;

    private HpController m_hpController;
    private PlayerTriggerItem m_playerTriggerItem;

    [SerializeField] private int m_currentLevel; //level actuel
    [SerializeField] private int m_currentExp;

    public void SetDependencies(GameController gameController)
    {

        m_hpController = gameController.m_hpController;

        m_playerTriggerItem = gameController.m_playerTriggerItem;
        m_playerTriggerItem.m_onItemExp += ExpGained;

    }

    public void OnDestroy()
    {

        m_playerTriggerItem.m_onItemExp -= ExpGained;
    }

    public void CompileExp(int exp)
    {
        //toute la logique sur la compilation d'experience se passera ici

        m_currentExp += exp;

        if (m_currentExp == 3)
        {
            m_currentExp = 0;
            Levelup();
        }

        m_onExpGained?.Invoke(m_currentExp);

        //est ce qu'on levelup ?

    }

    public void Levelup() {
        //on levelup
        m_currentLevel++;
        m_onLevelUp?.Invoke(m_currentLevel);
    }

    public void ExpGained(int xpValue) {
        CompileExp(xpValue);
    }

}
