using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_hunger;
    [SerializeField] private TextMeshProUGUI m_hp;
    [SerializeField] private TextMeshProUGUI m_experience;
    [SerializeField] private TextMeshProUGUI m_level;
    [SerializeField] private GameObject m_died;
    [SerializeField] private GameObject m_ItemQuete;
    [SerializeField] private GameObject m_ItemHunger;
    [SerializeField] private GameObject m_ItemDoneDictionnary;

    private ExperienceController m_experienceController;
    private HpController m_hpController;
    private HungerController m_hungerController;
    private ItemsController m_itemsController;

    public void SetDependencies(GameController gameController)
    {
        m_experienceController = gameController.m_experienceController;
        m_hpController = gameController.m_hpController;
        m_hungerController = gameController.m_hungerController;
        m_itemsController = gameController.m_itemsController;

        m_experienceController.m_onExpGained += UpdateExperience;
        m_experienceController.m_onLevelUp += UpdateLevel;
        m_hpController.m_onHpChange += UpdateHp;
        m_hpController.m_onDied += UpdateDied;
        m_hungerController.m_onHungerChange += UpdateHunger;

        m_itemsController.m_OnNotifyItemQueteDone += OnNotifyDoneQuete;
        m_itemsController.m_OnNotifyItemHungerDone += OnNotifyDoneHunger;
        m_itemsController.m_OnNotifyDictionnary += OnNotifyDoneDictionnary;
    }

    public void OnDestroy()
    {
        m_experienceController.m_onExpGained -= UpdateExperience;
        m_experienceController.m_onLevelUp -= UpdateLevel;
        m_hpController.m_onHpChange -= UpdateHp;
        m_hpController.m_onDied -= UpdateDied;
        m_hungerController.m_onHungerChange -= UpdateHunger;

        m_itemsController.m_OnNotifyItemQueteDone -= OnNotifyDoneQuete;
        m_itemsController.m_OnNotifyItemHungerDone -= OnNotifyDoneHunger;
        m_itemsController.m_OnNotifyDictionnary -= OnNotifyDoneDictionnary;
    }

    private void OnNotifyDoneQuete()
    {
        StartCoroutine(NotifyDoneQuete());
    }

    private IEnumerator NotifyDoneQuete()
    {
        m_ItemQuete.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_ItemQuete.SetActive(false);
    }

    //--

    private void OnNotifyDoneHunger()
    {
        StartCoroutine(NotifyDoneHunger());
    }

    private IEnumerator NotifyDoneHunger()
    {
        m_ItemHunger.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_ItemHunger.SetActive(false);
    }

    //---

    private void OnNotifyDoneDictionnary()
    {
        StartCoroutine(NotifyDoneDictionnary());
    }

    private IEnumerator NotifyDoneDictionnary()
    {
        m_ItemDoneDictionnary.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        m_ItemDoneDictionnary.SetActive(false);
    }

    //---

    private void UpdateExperience(int exp)
    {
        m_experience.text = "Experience : " + exp.ToString();
    }

    private void UpdateHp(int hp)
    {
        m_hp.text = "Life : " + hp.ToString();
    }

    private void UpdateHunger(int hung)
    {
        m_hunger.text = "Hunger : " + hung.ToString() + " %";
    }

    private void UpdateLevel(int level)
    {
        m_level.text = "Level : " + level.ToString();
    }

    private void UpdateDied(bool died)
    {
        m_died.SetActive(died);
    }
}
