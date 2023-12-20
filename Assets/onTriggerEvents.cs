using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTriggerEvents : MonoBehaviour
{
    [SerializeField] private CompanionScript companionScript;
    [SerializeField] private string Message;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Collider")
        {
            companionScript.ChangeMessage(Message);
            Destroy(gameObject);
        }
    }
}
