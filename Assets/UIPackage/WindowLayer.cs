using UnityEngine;

public class WindowLayer : MonoBehaviour
{
    public UIManager.WindowLayers _layer { get; private set; }

    public void SetLayer(UIManager.WindowLayers layer)
    {
        _layer = layer;
    }
}
