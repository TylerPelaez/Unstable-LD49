using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("MISSING");
        }
        
    }

    public void PlayRandomClip()
    {
        if (clips == null || clips.Length == 0)
        {
            return;
        }
        int clipIndex = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[clipIndex]);
    }
}
