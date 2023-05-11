using System.Collections.Generic;
using UnityEngine;

public class PickerController : MonoBehaviour, IPicker
{
    private AudioManager audioManager;

    [SerializeField] private float moveSpeedX = 5f;
    [SerializeField] private float moveSpeedZ = 5f;
    [SerializeField] private float minX = -3.3f;
    [SerializeField] private float maxX = 3.3f;

    private Rigidbody pickerRB;
    [SerializeField] private List<Rigidbody> collectedObjects = new List<Rigidbody>();
    private Vector3 pickerInitialPos;

    private bool PickerCanMove;
    private float forceValue = 60f;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        pickerInitialPos = transform.position;
        pickerRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (PickerCanMove)
        {
            Movement();
        }
    }

    public void Movement()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeedX, 0, moveSpeedZ) * Time.fixedDeltaTime;
        Vector3 targetPosition = pickerRB.position + moveDirection;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

        pickerRB.MovePosition(targetPosition);
    }

    public void MovePicker()
    {
        PickerCanMove = true;
    }

    public void StopPicker()
    {
        PickerCanMove = false;
        foreach (Rigidbody rb in collectedObjects)
        {
            if (rb != null)
            {
                CollectableObjects collectable = rb.GetComponent<CollectableObjects>();
                if (collectable != null)
                {
                    rb.AddForce(new Vector3(0, 0, 1) * forceValue, ForceMode.Impulse);
                    Debug.Log("Force added");
                    Destroy(rb.gameObject, 0.1f);
                }
            }
        }
        collectedObjects.Clear(); 
    }


    public void RestartPickerPos()
    {
        transform.position = pickerInitialPos;
    }

    public void ChangePickerColor()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Renderer childRenderer = transform.GetChild(i).GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.material.color = FindObjectOfType<Store>().GetPickerColor();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StageArea"))
        {
            StopPicker();
        }

        if (other.gameObject.CompareTag("Collectable"))
        {
            CollectableObjects collectable = other.gameObject.GetComponent<CollectableObjects>();
            if (!collectable.isPickedUp)
            {
                collectable.isPickedUp = true;
                collectedObjects.Add(other.gameObject.GetComponent<Rigidbody>());
                audioManager.PlayCollectableSFX();
            }
        }

        if (other.gameObject.CompareTag("LevelEnd"))
        {
            GameManager.Instance.GameEventHandler.TriggerLevelWin();
        }
    }
}