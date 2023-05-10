using UnityEngine;
using DG.Tweening;

public class CannonObject : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private float throwForce = 500f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector3 offScreenPosition = new Vector3(0, -15, 0);
    [SerializeField] private float offScreenAnimationDuration = 1f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private string playerTag = "Picker";
    [SerializeField] private float throwOffset = 10f;

    private int maxInstantiations;
    private int instantiationCount = 0;
    private bool playerInRange = false;
    private Transform playerTransform;

    private void Start()
    {
        maxInstantiations = Random.Range(5, 16);
        SphereCollider detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;
        playerTransform = GameObject.FindGameObjectWithTag(playerTag)?.transform;
    }

    private void Update()
    {
        if (playerInRange)
        {
            InstantiateAndThrowBall();
            MoveCannon();
            CheckInstantiationLimit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
        }
    }

    private void InstantiateAndThrowBall()
    {
        GameObject ball = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = ball.AddComponent<Rigidbody>();
        }
        Vector3 targetPosition = playerTransform.position + playerTransform.forward * throwOffset;
        Vector3 direction = (targetPosition - ballSpawnPoint.position).normalized;
        rb.AddForce(direction * throwForce);
    }

    private void MoveCannon()
    {
        instantiationCount++;
        transform.position += new Vector3(moveSpeed, 0, 0);
    }

    private void CheckInstantiationLimit()
    {
        if (instantiationCount >= maxInstantiations)
        {
            MoveCannonOffScreenAndDestroy();
        }
    }

    private void MoveCannonOffScreenAndDestroy()
    {
        transform.DOMove(offScreenPosition, offScreenAnimationDuration).OnComplete(() => Destroy(gameObject));
    }
}
