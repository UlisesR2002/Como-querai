using TMPro;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static float Volume = 0.5f;
    public float volume;
    public AudioClip buttonClickSound;
    private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI musicText;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    private void Update()
    {
        volume = Volume;
        SetVolumeText();
    }

    public void VolumeChange(float volumeDifference)
    {
        audioSource.PlayOneShot(buttonClickSound);
        Volume = Mathf.Clamp((Volume + volumeDifference),0f,1f);
    }

    public void SetVolumeText()
    {
        musicText.text = "Volume %" + Mathf.Round(Volume * 100);
    }
}
