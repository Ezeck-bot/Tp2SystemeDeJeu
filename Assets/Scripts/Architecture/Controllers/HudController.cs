using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    private const float COUROUTINE_TIME = 1.5f;

    [SerializeField] private TextMeshProUGUI m_hunger;
    [SerializeField] private TextMeshProUGUI m_hp;
    [SerializeField] private TextMeshProUGUI m_experience;
    [SerializeField] private TextMeshProUGUI m_level;
    [SerializeField] private GameObject m_died;
    [SerializeField] private GameObject m_ItemQuete;
    [SerializeField] private GameObject m_ItemHunger;
    [SerializeField] private GameObject m_ItemDoneDictionnary;

    [SerializeField] private Slider m_sliderCrafting;
    [SerializeField] private GameObject m_noItemCraftingRequire;
    [SerializeField] private GameObject m_textPlayerCrafting;

    private ExperienceController m_experienceController;
    private HpController m_hpController;
    private HungerController m_hungerController;
    private ItemsController m_itemsController;
    private CraftingController m_craftingController;

    public void SetDependencies(GameController gameController)
    {
        m_experienceController = gameController.m_experienceController;
        m_hpController = gameController.m_hpController;
        m_hungerController = gameController.m_hungerController;
        m_itemsController = gameController.m_itemsController;
        m_craftingController = gameController.m_craftingController;

        m_experienceController.m_onExpGained += UpdateExperience;
        m_experienceController.m_onLevelUp += UpdateLevel;
        m_hpController.m_onHpChange += UpdateHp;
        m_hpController.m_onDied += UpdateDied;
        m_hungerController.m_onHungerChange += UpdateHunger;

        m_itemsController.m_OnNotifyItemQueteDone += OnNotifyDoneQuete;
        m_itemsController.m_OnNotifyItemHungerDone += OnNotifyDoneHunger;
        m_itemsController.m_OnNotifyDictionnary += OnNotifyDoneDictionnary;

        m_craftingController.m_OnNotifySliderCrafting += OnNotifySliderCrafting;
        m_craftingController.m_OnNotifyNoSliderCrafting += OnNotifyNoSliderCrafting;
        m_craftingController.m_OnNotifyNoCraftingItem += OnNotifyNoCraftingItem;
        m_craftingController.m_OnNotifyPressCPalyer += OnNotifyPressCPalyer;
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

        m_craftingController.m_OnNotifySliderCrafting -= OnNotifySliderCrafting;
        m_craftingController.m_OnNotifyNoSliderCrafting -= OnNotifyNoSliderCrafting;
        m_craftingController.m_OnNotifyNoCraftingItem -= OnNotifyNoCraftingItem;
        m_craftingController.m_OnNotifyPressCPalyer -= OnNotifyPressCPalyer;
    }



    //SECTION CRAFTING
    private void OnNotifySliderCrafting(float time)
    {
        m_sliderCrafting.gameObject.SetActive(true);
        m_sliderCrafting.value = time;
    }

    private void OnNotifyNoSliderCrafting()
    {
        m_sliderCrafting.gameObject.SetActive(false);
    }

    private void OnNotifyNoCraftingItem(bool isActive)
    {
        m_noItemCraftingRequire.SetActive(isActive);
    }

    private void OnNotifyPressCPalyer(bool isActive)
    {
        m_textPlayerCrafting.SetActive(isActive);
    }

    //SECTION NOTIFY ITEM

    private void OnNotifyDoneQuete()
    {
        StartCoroutine(NotifyDoneQuete());
    }

    private IEnumerator NotifyDoneQuete()
    {
        m_ItemQuete.SetActive(true);
        yield return new WaitForSeconds(COUROUTINE_TIME);
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
        yield return new WaitForSeconds(COUROUTINE_TIME);
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
        yield return new WaitForSeconds(COUROUTINE_TIME);
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
