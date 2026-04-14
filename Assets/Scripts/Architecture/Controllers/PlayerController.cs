using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float NEW_TIME_DESCREASE_HUNGER = 0.2f;

    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private float m_moveSpeed;

    [SerializeField] private float m_jumpForce;

    [SerializeField] private float m_groundCheckDistance;
    [SerializeField] private LayerMask m_groundLayer;

    [SerializeField] private Camera m_ditrectionCamera;

    [SerializeField] private GameObject m_player;

    [SerializeField] private Animator m_animator;

    [SerializeField] private PlayerScriptableObject m_playerScriptableObject;
    [SerializeField] private HungerScriptableObject m_hungerScriptableObject;

    private bool m_isGround = true;


    //inventory


    private PlayerInputController m_input;
    private NpcController m_npc;
    private CraftingController m_crafting;

    public Action<float> m_onSprint;
    private float m_timeSpeed;

    //public bool m_isMoving => m_rigidBody.linearVelocity != Vector3.zero;

    public void SetDependencies(GameController gameController)
    {
        //m_animator = GetComponent<Animator>();

        m_input = gameController.m_playerInputController;

        m_npc = gameController.m_npcController;

        m_crafting = gameController.m_craftingController;

        m_input.m_onChangeMovement += Move;
        m_input.m_onChangeJump += Jump;
        m_input.m_onChangeRunning += Sprint;
        m_input.m_onChangeInteract += Interactable;
        m_input.m_onChangeCrafting += ActiveCrafting;
    }

    public void OnDestroy()
    {
        m_input.m_onChangeMovement -= Move;
        m_input.m_onChangeJump -= Jump;
        m_input.m_onChangeRunning -= Sprint;
        m_input.m_onChangeInteract -= Interactable;
        m_input.m_onChangeCrafting -= ActiveCrafting;
    }

    private void FixedUpdate()
    {
        //checker si je suis le sol
        m_isGround = Physics.Raycast(
            m_player.transform.position + Vector3.up * 0.1f,
            Vector3.down,
            m_groundCheckDistance,
            m_groundLayer
        );

        m_animator.SetBool("isJump", !m_isGround);

        //tourner le joueur en fonction de la rotation de la caméra
        m_player.transform.rotation = Quaternion.Euler(0, m_ditrectionCamera.transform.eulerAngles.y, 0);
    }

    public void ReceivedItem()
    {
        //give to inventory
    }

    public void Idle()
    {

    }

    public void Move(Vector2 movDirection)
    {
        if (m_isGround)
        {
            m_animator.SetFloat("Horizontale", movDirection.x);
            m_animator.SetFloat("Verticale", movDirection.y);

            Vector3 move = m_player .transform.right * movDirection.x + m_player.transform.forward * movDirection.y;
            Vector3 velocity = move * m_moveSpeed;

            velocity.y = m_rigidBody.linearVelocity.y;

            m_rigidBody.linearVelocity = velocity;

            if (m_rigidBody.linearVelocity != Vector3.zero)
            {
                //s'il bouge on annule toujours
                m_crafting.ActiveCrafting(false);
            }
        }

    }

    public void Jump(bool is_jump)
    {
        if (is_jump && m_isGround)
        {
            m_isGround = false;
            m_animator.SetBool("isJump", true);
            AudioManager.Instance.PlayAudio(SoundID.JUMP);
            m_rigidBody.linearVelocity = new Vector3(m_rigidBody.linearVelocity.x, m_jumpForce, m_rigidBody.linearVelocity.z);
        }
    }

    public void Sprint(bool is_sprint)
    {
        if (m_isGround && m_rigidBody.linearVelocity != Vector3.zero)
        {
            m_animator.SetBool("isRunning", is_sprint);

            if (is_sprint)
            {
                AudioManager.Instance.PlayAudio(SoundID.RUN);

                m_moveSpeed = m_playerScriptableObject.m_moveBoostSpeed;

                m_timeSpeed = m_hungerScriptableObject.m_newTimeDescreaseHunger;
            }
            else
            {
                m_timeSpeed = m_hungerScriptableObject.m_hungerTimeSpeed;

                m_moveSpeed = m_playerScriptableObject.m_walkSpeed;
            }
            m_onSprint?.Invoke(m_timeSpeed);
        }
    }

    public void Interactable(bool is_interactable)
    {
        if (m_isGround)
        {
            m_npc.TryInteract();
        }
    }

    public void ActiveCrafting(bool is_crafting)
    {
        if (m_isGround && m_rigidBody.linearVelocity == Vector3.zero)
        {
            m_crafting.ActiveCrafting(is_crafting);
        }
    }
}
