using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    //public float currentPlayerHealth = 100.0f;
    [SerializeField] private Entity player;
    [SerializeField] private Image redSplatterImage = null;
    [SerializeField] private Image hurtImage = null;
    [SerializeField] private float hurtTimer = 0.01f;

    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (player.hp / player.maxHP);
        redSplatterImage.color = splatterAlpha;
    }
    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
    }

    public void TakeDamage() 
    {
        if (player.hp >= 0)
        {
            StartCoroutine(HurtFlash());
            UpdateHealth();
        }
    }

}
