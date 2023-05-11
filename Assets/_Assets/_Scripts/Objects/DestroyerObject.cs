using System.Collections;
using UnityEngine;

public class DestroyerObject : MonoBehaviour
{
    [SerializeField] private GameObject particleEffectPrefab;
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            Vector3 contactPoint = other.contacts[0].point;
            GameObject particleEffectInstance = Instantiate(particleEffectPrefab, contactPoint, Quaternion.identity);

            ParticleSystem particleSystem = particleEffectInstance.GetComponent<ParticleSystem>();
            particleSystem.Play();

            audioManager.PlayDestroyerSFX();

            StartCoroutine(DestroyObjectsAfterDelay(other.gameObject, gameObject, particleEffectInstance, particleSystem.main.duration / 2));
        }
    }

    private IEnumerator DestroyObjectsAfterDelay(GameObject otherObject, GameObject destroyerObject, GameObject particleEffectInstance, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(otherObject);
        Destroy(destroyerObject);
        Destroy(particleEffectInstance);
    }
}
