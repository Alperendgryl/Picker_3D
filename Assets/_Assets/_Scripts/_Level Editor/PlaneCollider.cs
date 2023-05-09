using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other != null)
        {
            GameObject parent = other.transform.parent.gameObject;
            if (parent != null) Destroy(parent);
            else return;
        }
    }
}
