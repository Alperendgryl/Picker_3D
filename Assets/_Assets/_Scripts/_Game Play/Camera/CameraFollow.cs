using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followDuration = 0.5f;
    [SerializeField] private bool lookAtTarget = true;

    private void Awake()
    {
        target = FindObjectOfType<PickerController>().gameObject.transform;
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, target.position.y + offset.y, target.position.z + offset.z);
            transform.DOMove(targetPosition, followDuration);

            if (lookAtTarget)
            {
                transform.DOLookAt(target.position, followDuration);
            }
        }
    }
}
