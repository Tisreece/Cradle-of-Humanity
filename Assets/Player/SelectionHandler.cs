using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    private float RayLength = 1000f;
    private Camera Camera;

    public List<Unit_Master> UnitsSelected = new List<Unit_Master>();
    [SerializeField] private List<Unit_Master> UnitsHovered = new List<Unit_Master>();
    private List<Unit_Master> UnitsReadyToHover = new List<Unit_Master>();
    [SerializeField] private GameObject SelectionBoxGO;
    private Vector3 MoveLocation;
    private Vector3 Position1;
    private Vector3 Position2;
    private bool DragSelect = false;
    MeshCollider SelectionBox;
    Mesh SelectionMesh;
    Vector2[] SelectionCorners;
    Vector3[] SelectionVerts;
    private bool HasBox = false;

    //Variables for the Selection Box Visual//
    [SerializeField] private RectTransform BoxVisual;
    private Vector2 BoxStartPosition = Vector2.zero;
    private Vector2 BoxEndPosition = Vector2.zero;
    //=====================================//

    // Start is called before the first frame update
    void Start()
    {
        Camera = GetComponent<Camera>();
        DrawBoxVisual();
    }

    void Update()
    {
       
    }

    //Functions for input
    //===================================================================================================================================================================================//
    public void StartSelecting()
    {
        Position1 = Input.mousePosition;
        BoxStartPosition = Input.mousePosition;
    }

    public void IsDragSelecting()
    {
        if ((Position1 - Input.mousePosition).magnitude > 5)
        {
            DragSelect = true;
            BoxEndPosition = Input.mousePosition;
            DrawBox();
            DrawBoxVisual();
        }
    }

    public void SelectUnits()
    {
        if (DragSelect == false) //Single clicking units
        {
            Ray ray = Camera.main.ScreenPointToRay(Position1);
            RaycastHit RayHit;
            if (Physics.Raycast(ray, out RayHit, RayLength))
            {
                if (RayHit.collider.tag == "Unit")
                {
                    Unit_Master UnitToSelect = RayHit.collider.gameObject.GetComponent<Unit_Master>();
                    SelectUnit(UnitToSelect);
                }
            }
        }
        else //Drag Selection
        {

        }
        
        DragSelect = false;
        BoxStartPosition = Vector2.zero;
        BoxEndPosition = Vector2.zero;
        DrawBoxVisual();
    }
    public void SelectUnit(Unit_Master UnitToSelect)
    {
        UnitToSelect.Selection(true);
        UnitsSelected.Add(UnitToSelect);
    }

    //Creating the Selection Bounding Box
    //==================================================================================================================================================================================//
    Vector2[] GetBoundingBox(Vector2 Position1, Vector2 Position2)
    {
        Vector2 newPosition1;
        Vector2 newPosition2;
        Vector2 newPosition3;
        Vector2 newPosition4;

        if (Position1.x < Position2.x) //If Position1 is to the left of Position2
        {
            if (Position1.y > Position2.y) //If Position 1 is above Position2
            {
                newPosition1 = Position1;
                newPosition2 = new Vector2(Position2.x, Position1.y);
                newPosition3 = new Vector2(Position1.x, Position2.y);
                newPosition4 = Position2;
            }
            else //If Position1 is below Position2
            {
                newPosition1 = new Vector2(Position1.x, Position2.y);
                newPosition2 = Position2;
                newPosition3 = Position1;
                newPosition4 = new Vector2(Position2.x, Position1.y);
            }
        }
        else //If Position1 is to the right of Position2
        {
            if (Position1.y > Position2.y) //If Position1 is above Position2
            {
                newPosition1 = new Vector2(Position2.x, Position1.y);
                newPosition2 = Position1;
                newPosition3 = Position2;
                newPosition4 = new Vector2(Position1.x, Position2.y);
            }
            else //If Position1 is below Position2
            {
                newPosition1 = Position2;
                newPosition2 = new Vector2(Position1.x, Position2.y);
                newPosition3 = new Vector2(Position2.x, Position1.y);
                newPosition4 = Position1;
            }
        }

        Vector2[] corners = { newPosition1, newPosition2, newPosition3, newPosition4 };
        return corners;
    }

    //Generate Mesh from the 4 points
    Mesh GenerateSelectionMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //mapping the tris of the cube

        for(int i = 0; i < 4; i++) //Bottom Rectangle
        {
            verts[i] = corners[i];
        }

        for(int j = 4; j < 8; j++) //TopRectangle
        {
            verts[j] = corners[j - 4] + Vector3.up * 20f;
        }

        Mesh SelectionMesh = new Mesh();
        SelectionMesh.vertices = verts;
        SelectionMesh.triangles = tris;

        return SelectionMesh;
    }

    //====================================================================================================================================================================================//

    public void MoveUnits(Vector3 MoveLocation)
    {
        if (UnitsSelected.Count != 0)
        {
            for (int i = 0; i < UnitsSelected.Count; i++)
            {
                Unit_Master UnitToMove = UnitsSelected[i];
                UnitToMove.MoveUnit(MoveLocation);
            }
        }
    }

    private void DrawBox()
    {
        if (HasBox == true)
        {
            DestroyImmediate(SelectionBox);
        }
        SelectionVerts = new Vector3[4];
        int i = 0;
        Position2 = Input.mousePosition;
        SelectionCorners = GetBoundingBox(Position1, Position2);

        foreach (Vector2 corner in SelectionCorners)
        {
            Ray ray = Camera.main.ScreenPointToRay(corner);
            RaycastHit RayHit;

            if (Physics.Raycast(ray, out RayHit, RayLength))
            {
                SelectionVerts[i] = new Vector3(RayHit.point.x, 0, RayHit.point.z);
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), RayHit.point, Color.red, 1.0f);
            }
            i++;
        }
        //Generate Mesh
        SelectionMesh = GenerateSelectionMesh(SelectionVerts);SelectionBox = SelectionBoxGO.AddComponent<MeshCollider>();
        SelectionBox.sharedMesh = SelectionMesh;
        SelectionBox.convex = true;
        SelectionBox.isTrigger = true;
        HasBox = true;
    }
    private void DrawBoxVisual() //Not to be consued with DrawBox - this iss UI
    {
        Vector2 BoxStart = BoxStartPosition;
        Vector2 BoxEnd = BoxEndPosition;
        Vector2 BoxCentre = (BoxStart + BoxEnd) / 2;

        BoxVisual.position = BoxCentre;

        Vector2 BoxSize = new Vector2(Mathf.Abs(BoxStart.x - BoxEnd.x), Mathf.Abs(BoxStart.y - BoxEnd.y));
        BoxVisual.sizeDelta = BoxSize;
    }

    public void HoverUnit(Unit_Master UnitToHover, bool FromController)
    {
        if (FromController == true && DragSelect == false || FromController == false && DragSelect == true)
        {
            if (!UnitsHovered.Contains(UnitToHover))
            {
                UnitToHover.SetHovered();
                UnitsHovered.Add(UnitToHover);
            }
        }
    }
    public void UnhoverUnit(Unit_Master UnitToUnhover, bool FromController)
    {
        if (FromController == true && DragSelect == false || FromController == false && DragSelect == true)
        {
            if (UnitsHovered.Contains(UnitToUnhover))
            {
                UnitToUnhover.SetUnhovered();
                UnitsHovered.Remove(UnitToUnhover);
            }
        }
    }
    public void UnhoverAllUnits(bool FromController)
    {
        if (FromController == true && DragSelect == false || FromController == false && DragSelect == true)
        {
            for (int i = 0; i < UnitsHovered.Count; i++)
            {
                UnitsHovered[i].SetUnhovered();
            }
            UnitsHovered.Clear();
        }
    }
}
