using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other != null) Destroy(other.gameObject);
    }
}
