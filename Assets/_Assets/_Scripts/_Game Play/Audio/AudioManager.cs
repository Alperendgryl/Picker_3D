using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip collectableSFX, poolGateSFX, DestroyerSFX;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayPoolGateSFX()
    {
        audioSource.PlayOneShot(poolGateSFX);
    }

    public void PlayCollectableSFX()
    {
        audioSource.PlayOneShot(collectableSFX);
    }

    public void PlayDestroyerSFX()
    {
        audioSource.PlayOneShot(DestroyerSFX);
    }
}
