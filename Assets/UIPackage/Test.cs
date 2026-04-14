using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Window _hudWindow;
    [SerializeField] private Window _dialogueWindow;
    [SerializeField] private Window _loading;

    [ContextMenu("Open Hud")]
    private void OpenHud()
    {
        UIManager.Instance.OpenWindow(_hudWindow);
    }

    [ContextMenu("Open Dialogue")]
    private void OpenDialogue()
    {
        UIManager.Instance.OpenWindow(_dialogueWindow);
    }

    [ContextMenu("Open Loading")]
    private void OpenLoading()
    {
        UIManager.Instance.OpenWindow(_loading);
    }
}
