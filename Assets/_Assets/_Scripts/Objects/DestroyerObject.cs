using System.Collections;
using UnityEngine;

public class DestroyerObject : MonoBehaviour
{
    [SerializeField] private GameObject particleEffectPrefab;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Picker")) return;
        if (other.collider == null) return;

        Vector3 contactPoint = other.contacts[0].point;
        GameObject particleEffectInstance = Instantiate(particleEffectPrefab, contactPoint, Quaternion.identity);

        StartCoroutine(DestroyObjectsAfterDelay(other.gameObject, gameObject, particleEffectInstance));
    }

    private IEnumerator DestroyObjectsAfterDelay(GameObject otherObject, GameObject destroyerObject, GameObject particleEffectInstance)
    {
        float delay = 1.5f;
        yield return new WaitForSeconds(delay);

        Destroy(otherObject);
        Destroy(destroyerObject);
        Destroy(particleEffectInstance);
    }
}
