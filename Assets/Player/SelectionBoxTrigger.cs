using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxTrigger : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;
    private SelectionHandler SH;
    // Start is called before the first frame update
    void Start()
    {
        SH = MainCamera.GetComponent<SelectionHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
        {
            Debug.Log("Enter");
            Unit_Master UnitToHover = other.gameObject.GetComponent<Unit_Master>();
            SH.HoverUnit(UnitToHover, false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Unit")
        {
            Debug.Log("Leave");
            Unit_Master UnitToUnhover = other.gameObject.GetComponent<Unit_Master>();
            SH.UnhoverUnit(UnitToUnhover, false);
        }
    }
}
