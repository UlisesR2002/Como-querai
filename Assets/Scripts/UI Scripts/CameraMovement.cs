using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float speed = 100;
    public float maxSpeed = 50;
    public Transform playerTransform;
    public Vector3 distToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        distToPlayer = playerTransform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //distToPlayer = Quaternion.Euler(0, speed * Time.deltaTime, 0) * distToPlayer;
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //distToPlayer = Quaternion.Euler(0, speed * Time.deltaTime, 0) * distToPlayer;
            transform.Rotate(0, -speed * Time.deltaTime, 0);
        }
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position + distToPlayer, maxSpeed * Time.deltaTime);
        //transform.position = playerTransform.position + distToPlayer;
    }
}
