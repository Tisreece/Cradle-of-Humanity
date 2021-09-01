using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 10f;
    float HorizontalMove = 0f;
    float VerticalMove = 0f;
    Camera Camera;

    private void Start()
    {
        Camera = GetComponent<Camera>();
    }
    private void Update()
    {
        HorizontalMove = Input.GetAxisRaw("HorizontalMove") * MoveSpeed;
        VerticalMove = Input.GetAxisRaw("VerticalMove") * MoveSpeed;
        if (Input.GetAxis ("Mouse ScrollWheel") > 0 && Camera.orthographicSize > 4)
        {
            Camera.orthographicSize -= 2 ;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.orthographicSize < 198)
        {
            Camera.orthographicSize += 2;
        }
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(HorizontalMove, 0, -HorizontalMove) * Time.deltaTime;
        transform.position += new Vector3(VerticalMove, 0, VerticalMove) * Time.deltaTime;
    }
}
