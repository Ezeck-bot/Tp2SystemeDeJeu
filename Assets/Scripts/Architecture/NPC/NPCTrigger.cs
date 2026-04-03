using System;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{

    public Action<string> m_onNotifyMessage;

    public void SetDependencies(GameController gameController)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC_BLEU"))
        {
            NpcData npcData = other.GetComponent<NpcData>();
            m_onNotifyMessage?.Invoke(npcData.message);
            Debug.Log("NPC_BLEU");
        }
    }
}
