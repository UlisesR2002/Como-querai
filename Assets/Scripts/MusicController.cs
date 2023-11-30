using TMPro;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    private float musicVolume;
    [SerializeField] private TextMeshProUGUI musicText;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        musicVolume = Mathf.Clamp(musicVolume, 0f, 1f);
        audioSource.volume = musicVolume;
        setVolumeText();
    }

    public void VolumeChange(float volumeDifferences)
    {
        musicVolume += volumeDifferences;
    }

    public void setVolumeText()
    {
        musicText.text = "Volume %" + Mathf.Round(audioSource.volume*100);
    }

    
}
