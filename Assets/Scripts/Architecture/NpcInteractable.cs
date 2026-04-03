using System;
using UnityEngine;
using static NPCData;

public class NpcInterractable : MonoBehaviour
{
    [SerializeField] private DialogueData m_dialogueData;

    private bool m_isPlayerNear = false;
    public Action<bool> m_onNotifyNear;

    private const string PLAYER = "Player";

    public DialogueData GetDialogueData() => m_dialogueData;
    public bool IsPlayerNear() => m_isPlayerNear;


    public void SetDependencies(GameController gameController)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER))
            m_isPlayerNear = true;
            m_onNotifyNear?.Invoke(m_isPlayerNear);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER))
            m_isPlayerNear = false;
            m_onNotifyNear?.Invoke(m_isPlayerNear);
    }
}
