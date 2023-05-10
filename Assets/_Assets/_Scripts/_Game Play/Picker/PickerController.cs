using UnityEngine;
using static UnityEngine.Video.VideoPlayer;

public class PickerController : MonoBehaviour
{
    [SerializeField] private float moveSpeedX = 5f;
    [SerializeField] private float moveSpeedZ = 5f;
    [SerializeField] private float minX = -3.3f;
    [SerializeField] private float maxX = 3.3f;

    private Rigidbody rb;
    GameEventHandler gameEventHandler;
    private bool PickerCanMove;
    private void Start()
    {
        gameEventHandler = FindObjectOfType<GameEventHandler>();
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

        //wait for UI
    }

    public void RestartPickerPos()
    {
        //Restart the pos of picker when the game restarts
    }

    public void ChangePickerColor()
    {
        //Store Logic
    }
    #endregion
}
