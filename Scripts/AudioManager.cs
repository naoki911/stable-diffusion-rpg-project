using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure that only one AudioManager instance exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Play the given clip
    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
