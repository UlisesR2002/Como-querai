using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static Assets.Scripts.EnemyController;

public class Pickup : MonoBehaviour
{
    [Header("Pick up Type")]
    [SerializeField] private PickupType type;
    [SerializeField] private int amount;
    [SerializeField] private GunController gunController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 30.0f * Time.deltaTime, 0.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController p))
        {
            switch(type)
            {
                case PickupType.RifleAmmo:
                    gunController.ammo += amount;
                    break;
            }

            Destroy(gameObject);
            return;
        }
    }
    public enum PickupType
    {
        RifleAmmo
    }
}
