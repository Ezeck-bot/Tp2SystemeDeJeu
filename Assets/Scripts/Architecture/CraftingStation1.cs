using UnityEngine;

public class CraftingStation1 : MonoBehaviour
{

    private CraftingController m_craftingController;

    private bool isInsideTrigger = false;

    public void Init(CraftingController craftingController)
    {
        m_craftingController = craftingController;
    }

    private void Update()
    {
        if (isInsideTrigger)
        {
            m_craftingController.OnPlayerEnterStation1();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInsideTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInsideTrigger = false;
        m_craftingController.OnPlayerExitStation1();
    }
}
