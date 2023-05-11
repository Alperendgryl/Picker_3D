using UnityEngine;

public class PickerController : MonoBehaviour
{
    [SerializeField] private float moveSpeedX = 5f;
    [SerializeField] private float moveSpeedZ = 5f;
    [SerializeField] private float minX = -3.3f;
    [SerializeField] private float maxX = 3.3f;

    private Rigidbody rb;
    private bool PickerCanMove;

    private Vector3 pickerInitialPos;
    private void Start()
    {
        pickerInitialPos = transform.position;
        rb = GetComponent<Rigidbody>();
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
        Vector3 targetPosition = rb.position + moveDirection;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

        rb.MovePosition(targetPosition);
    }

    #region Picker States
    public void MovePicker()
    {
        PickerCanMove = true;
    }

    public void StopPicker()
    {
        PickerCanMove = false;
    }

    public void RestartPickerPos()
    {
        //if new level, level restart, levelend etc...
        transform.position = pickerInitialPos;
    }

    public void ChangePickerColor()
    {
        //Store Logic
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StageArea"))
        {
            StopPicker();
        }
    }

    #region Events
    private void OnEnable()
    {
        var gameEventHandler = FindObjectOfType<GameManager>().GameEventHandler;
        gameEventHandler.OnPoolAnimationsFinished += MovePicker;
    }

    private void OnDisable()
    {
        var gameEventHandler = FindObjectOfType<GameManager>().GameEventHandler;
        gameEventHandler.OnPoolAnimationsFinished -= MovePicker;
    }
    #endregion
}
