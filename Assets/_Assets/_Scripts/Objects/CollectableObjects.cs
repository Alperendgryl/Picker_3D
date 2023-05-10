using UnityEngine;

public class CollectableObjects : MonoBehaviour
{
    private Vector3 initialPos;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        if (transform.position.y > initialPos.y + 0.1f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = initialPos.y;
            rb.MovePosition(newPosition);
        }
    }
}
