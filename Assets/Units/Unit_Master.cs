using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Master : MonoBehaviour
{
    public Tribe_Enum Tribe;
    private SpriteRenderer SelectArrow;
    private SpriteRenderer[] HoverCorners = new SpriteRenderer[4];
    private NavMeshAgent NMA;
    // Start is called before the first frame update
    void Start()
    {
        SelectArrow = transform.Find("SelectArrow").gameObject.GetComponent<SpriteRenderer>();
        SetHoverCorners();

        NMA = transform.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Functions for the Hover Corners
    //===========================================================================================================================//
    private void SetHoverCorners()
    {
        SpriteRenderer HoverCorner1 = transform.Find("HoverCorner1").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer HoverCorner2 = transform.Find("HoverCorner2").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer HoverCorner3 = transform.Find("HoverCorner3").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer HoverCorner4 = transform.Find("HoverCorner4").gameObject.GetComponent<SpriteRenderer>();

        HoverCorners[0] = HoverCorner1;
        HoverCorners[1] = HoverCorner2;
        HoverCorners[2] = HoverCorner3;
        HoverCorners[3] = HoverCorner4;
    }

    public void SetHovered()
    {
        for (int i = 0; i < 4; i++)
        {
            HoverCorners[i].enabled = true;
        }
    }

    public void SetUnhovered()
    {
        for (int i = 0; i < 4; i++)
        {
            HoverCorners[i].enabled = false;
        }
    }

    //==========================================================================================================================//

    public void Selection(bool Select)
    {
        if (Select == true)
        {
            SelectArrow.enabled = true;
        }
        else
        {
            SelectArrow.enabled = false;
        }
    }

    public void MoveUnit(Vector3 Destination)
    {
        NMA.destination = Destination;
    }
}
