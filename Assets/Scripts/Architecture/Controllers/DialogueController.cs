using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static NPCData;

public class DialogueController : MonoBehaviour
{

    [SerializeField] private GameObject m_dialogue;
    [SerializeField] private TextMeshProUGUI m_dialogueText;
    [SerializeField] private Button[] m_choiceButtons;

    private DialogueData m_currentDialogue;

    public bool IsDialogueOpen() => m_dialogue.activeSelf;

    public void StartDialogue(DialogueData data)
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_dialogue.SetActive(true);

        m_currentDialogue = data;

        
        StartCoroutine(TypewriterEffect(data.m_initialQuestion));

        for (int i = 0; i < m_choiceButtons.Length; i++)
        {
            int index = i;
            m_choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = data.m_choices[i].m_btnText;
            m_choiceButtons[i].onClick.RemoveAllListeners();
            m_choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(index));
        }

    }

    private void OnChoiceSelected(int choiceIndex)
    {
        string response = m_currentDialogue.m_choices[choiceIndex].m_responseText;
        StartCoroutine(TypewriterEffect(response));
    }

    public void CloseDialogue()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_dialogueText.text = "";
        m_dialogue.SetActive(false);
        StopAllCoroutines();
    }

    private IEnumerator TypewriterEffect(string text)
    {
        m_dialogueText.text = "";
        foreach (char c in text)
        {
            m_dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
