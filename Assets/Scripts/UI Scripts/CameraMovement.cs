using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float speed = 100;
    public float maxSpeed = 50;

    public Transform playerTransform;

    public Transform cameraTransform;
    public float cameraSensibility;
    public float cameraDistance;

    public Vector3 distToPlayer;
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        distToPlayer = playerTransform.position - transform.position;
        cameraDistance = cameraTransform.localPosition.magnitude;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float MouseAxisX = Input.GetAxis("Mouse X");

        transform.Rotate(0, MouseAxisX * cameraSensibility * Time.deltaTime, 0);

        float MouseScroll = Input.mouseScrollDelta.y;
        cameraDistance -= MouseScroll;
        cameraDistance = Mathf.Clamp(cameraDistance,4,12);
        cameraTransform.localPosition = cameraTransform.localPosition.normalized * cameraDistance;


        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, maxSpeed * Time.deltaTime);
        //transform.position = playerTransform.position + distToPlayer;
    }
}
