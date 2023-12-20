using TMPro;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GetMusic();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = VolumeManager.GetVolume();
        if (!audioSource.isPlaying)
            Start();
    }

    AudioClip GetMusic()
    {
        int i = Random.Range(0, musicClip.Length);
        return musicClip[i];
    }
}
