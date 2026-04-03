using UnityEngine;
using static NPCData;

public class NpcController : MonoBehaviour
{
    ///reference sur nos trois npc
    /// <summary>
    /// reference sur nos trois npc
    /// </summary>
    /// 
    private DialogueController m_dialogueController;

    [SerializeField] private NpcInterractable[] m_npcs;

    public void SetDependencies(GameController gameController)
    {
        m_dialogueController = gameController.m_dialogueController;

        //m_npcInterract.m_onNotifyNear += TryInteract;
    }

    private void OnDestroy()
    {
        //m_npcInterract.m_onNotifyNear -= TryInteract;
    }

    public void TryInteract()
    {

        if (m_dialogueController.IsDialogueOpen())
        {
            m_dialogueController.CloseDialogue();
            return;
        }

        foreach (NpcInterractable npc in m_npcs)
        {
            if (npc.IsPlayerNear())
            {
                m_dialogueController.StartDialogue(npc.GetDialogueData());
                return;
            }
        }
    }


}
