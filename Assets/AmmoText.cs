using TMPro;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Ammo: " + player.activeGun.GetAmmoText();
    }
}
