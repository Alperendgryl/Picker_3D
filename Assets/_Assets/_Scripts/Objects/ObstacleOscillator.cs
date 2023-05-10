using UnityEngine;

[DisallowMultipleComponent]
public class ObstacleOscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector = new Vector3(0, 0, 0);
    [SerializeField] private float movementFactor, period = 2f; //0 not moving, 1 full movement

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        StartOscillator();
    }

    private void StartOscillator()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cylces = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cylces * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Picker")) ApplyPushBackForce(other);
        if (other.collider == null) return;
        Destroy(other.gameObject);
    }

    private void ApplyPushBackForce(Collision collision)
    {
        Rigidbody otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            Vector3 pushBackDirection = (collision.transform.position - transform.position).normalized;
            float pushBackForce = 5f;
            otherRigidbody.AddForce(pushBackDirection * pushBackForce, ForceMode.Impulse);
        }
    }
}
