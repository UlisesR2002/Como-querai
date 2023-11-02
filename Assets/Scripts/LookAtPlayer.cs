using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform playerTransform;

    private void Start()
    {
        playerTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(playerTransform.position);
        transform.Rotate(0, 180, 0);
    }
}
