using UnityEngine;

public class GameController : MonoBehaviour
{

    public DialogueController m_dialogueController { get; private set; }

    public EnemyController m_enemyController { get; private set; }

    public ExperienceController m_experienceController { get; private set; }

    public HpController m_hpController { get; private set; }

    public HudController m_hudController { get; private set; }

    public HungerController m_hungerController { get; private set; }

    public ItemsController m_itemsController { get; private set; }

    public NpcController m_npcController { get; private set; }

    public PlayerController m_playerController { get; private set; }

    public PlayerInputController m_playerInputController { get; private set; }


    //-------
    [SerializeField] private PlayerTriggerItem m_playerTriggerItemRef;
    public PlayerTriggerItem m_playerTriggerItem { get; private set; }


    //-------
    [SerializeField] private NpcInterractable m_npcInterractableRef;
    public NpcInterractable m_npcInterractable { get; private set; }

    public void Awake()
    {
        //seul le game controller a un awake

        m_dialogueController = GetComponentInChildren<DialogueController>();

        m_enemyController = GetComponentInChildren<EnemyController>();

        m_experienceController = GetComponentInChildren<ExperienceController>();

        m_hpController = GetComponentInChildren<HpController>();

        m_hudController = GetComponentInChildren<HudController>();

        m_hungerController = GetComponentInChildren<HungerController>();

        m_itemsController = GetComponentInChildren<ItemsController>();

        m_npcController = GetComponentInChildren<NpcController>();

        m_playerController = GetComponentInChildren<PlayerController>();

        m_playerInputController = GetComponentInChildren<PlayerInputController>();

        m_playerTriggerItem = m_playerTriggerItemRef;

        m_npcInterractable = m_npcInterractableRef;

        SetDependencies();

        StartCoroutine(m_hungerController.IncrementHunger());
    }

    public void SetDependencies()
    {
        m_hungerController.SetDependencies(this);

        m_hpController.SetDependencies(this);

        m_experienceController.SetDependencies(this);

        m_hudController.SetDependencies(this);

        m_itemsController.SetDependencies(this);

        m_npcController.SetDependencies(this);

        m_playerController.SetDependencies(this);

        m_playerInputController.SetDependencies(this);

        //faire disparaître le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void InitControllers()
    {
        //que de l'initialisation
        //Exemple : m_currentHp = 120;
    }

    private void InternalStart()
    {
        //commencer les animations, afficher le texte
        // vrai logique commence ici
    }
}
