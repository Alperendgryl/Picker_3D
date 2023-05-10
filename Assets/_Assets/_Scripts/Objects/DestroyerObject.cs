using UnityEngine;

public class DestroyerObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Picker")) return;
        if (other.collider == null) return;
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
