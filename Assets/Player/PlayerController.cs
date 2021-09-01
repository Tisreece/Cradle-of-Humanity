using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float RayLength = 1000f;
    private Camera Camera;
    private SelectionHandler SelectHandler;

    //Variables handling unit selection
    //=====================================================================================================//
    private Vector3 MoveLocation;
    //====================================================================================================//

    private void Start()
    {
        Camera = GetComponent<Camera>();
        SelectHandler = GetComponent<SelectionHandler>();
    }

    private void Update()
    {
        RayToMap();

        if (Input.GetButtonDown("Select")) //Left Click
        {
            SelectHandler.StartSelecting();
        }

        if (Input.GetButton("Select")) //Held down
        {
            SelectHandler.IsDragSelecting();
        }

        if (Input.GetButtonUp("Select")) //Release Select
        {
            SelectHandler.SelectUnits();
        }

        if (Input.GetButtonDown("Move"))
        {
            SelectHandler.MoveUnits(MoveLocation);
        }
    }
    private void RayToMap()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit RayHit;
        if (Physics.Raycast(ray.origin, ray.direction, out RayHit, RayLength))
        {
           if (RayHit.collider.tag == "Unit")
            {
                Unit_Master UnitToHover = RayHit.collider.gameObject.GetComponent<Unit_Master>();
                SelectHandler.HoverUnit(UnitToHover, true);
            }
            else
            {
                SelectHandler.UnhoverAllUnits(true);
            }

            //Get the move location when right-clicking
            if (Input.GetButtonDown("Move"))
            {
                MoveLocation = RayHit.point;
            }
        }
    }
}
