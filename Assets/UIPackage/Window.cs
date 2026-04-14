using UnityEngine;

public class Window : MonoBehaviour
{
    [field: SerializeField] public UIManager.WindowLayers _layer { get; private set; }

    public virtual void Close()
    {
        UIManager.Instance.CloseWindow(this);
    }


    [ContextMenu("Close")]
    private void CloseTest()
    {
        Close();
    }

}
