using UnityEngine;

public class NPCData : MonoBehaviour
{

    [System.Serializable]
    public class DialogueData
    {
        public string m_initialQuestion; 
        public DialogueChoice[] m_choices;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string m_btnText;
        public string m_responseText;
    }
}
