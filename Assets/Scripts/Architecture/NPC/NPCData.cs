using UnityEngine;

public class NpcData : MonoBehaviour
{
    [SerializeField] public string message = "";
    [Header("First Option")]
    [SerializeField] public string button1 = "";
    [SerializeField] public string response1 = "";
    [Header("Second Option")]
    [SerializeField] public string button2 = "";
    [SerializeField] public string response2 = "";
}
