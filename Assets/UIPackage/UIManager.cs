using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum WindowLayers
    {
        Background,
        MiddleGround,
        Foreground,
        Loading,
        Popup
    }

    public static UIManager Instance { get; private set; }

    private Dictionary<Window, WindowLayers> _windowToLayer = new();
    private Dictionary<WindowLayers, Transform> _transformToLayer = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InstantiateLayers();
    }

    private void InstantiateLayers()
    {
        System.Collections.IList list = System.Enum.GetValues(typeof(WindowLayers));
        for (int i = 0; i < list.Count; i++)
        {
            WindowLayers layer = (WindowLayers)list[i];
            GameObject go = new GameObject(layer.ToString());
            WindowLayer windowLayer = go.AddComponent<WindowLayer>();
            go.transform.parent = transform;
            windowLayer.SetLayer(layer);
            _transformToLayer.Add(layer, go.transform);
        }
    }

    public void OpenWindow(Window window)
    {
        if (_transformToLayer.TryGetValue(window._layer, out Transform transform))
        {
            Window instanciatedWindow = Instantiate(window, transform);
            _windowToLayer.TryAdd(instanciatedWindow, window._layer);
        }
    }

    public void CloseWindow(Window window)
    {
        if (_windowToLayer.Remove(window))
        {
            Destroy(window.gameObject);
        }
    }
}
