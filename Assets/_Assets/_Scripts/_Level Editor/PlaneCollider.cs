using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other != null) Destroy(other.gameObject);
    }
}
