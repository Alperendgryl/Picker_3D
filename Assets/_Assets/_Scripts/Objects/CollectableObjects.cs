using System.Collections;
using UnityEngine;

public class CollectableObjects : MonoBehaviour
{
    [SerializeField] private GameObject particleEffectPrefab;
    private Vector3 initialPos;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        ResetPositionIfAboveInitial();
    }

    private void ResetPositionIfAboveInitial()
    {
        if (transform.position.y > initialPos.y + 0.1f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = initialPos.y;
            rb.MovePosition(newPosition);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pool"))
        {
            StartCoroutine(DestroyCollectable());
        }
    }

    private IEnumerator DestroyCollectable()
    {
        yield return new WaitForSeconds(1.5f);

        GameObject particleEffectInstance = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        DisableObjectComponents();

        PlayParticleEffect(particleEffectInstance);
        float particleEffectDuration = particleEffectInstance.GetComponent<ParticleSystem>().main.duration;
        yield return new WaitForSeconds(particleEffectDuration);

        Destroy(particleEffectInstance);
        Destroy(gameObject);
    }

    private void DisableObjectComponents()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void PlayParticleEffect(GameObject particleEffectInstance)
    {
        ParticleSystem particleSystem = particleEffectInstance.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }
}
