using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerController : MonoBehaviour, IPicker
{
    [Header("Picker Speed Settings")]
    [SerializeField] private float moveSpeedX = 5f;
    [SerializeField] private float moveSpeedZ = 5f;
    [SerializeField] private float minX = -3.3f;
    [SerializeField] private float maxX = 3.3f;

    private GameObject infTrigger;
    private Rigidbody pickerRB;
    private Vector3 pickerInitialPos;
    private bool PickerCanMove;
    private float forceValue = .25f;
    private Coroutine bonusCoroutine;
    private int tempCollectableCount = 0;

    [Header("Dependencies")]
    private AudioManager audioManager;
    private LevelDataHandler dataHandler;
    private GUIManager guiManager;

    private List<Rigidbody> collectedObjects = new List<Rigidbody>();

    private void Awake()
    {
        dataHandler = LevelDataHandler.Instance;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        guiManager = FindObjectOfType<GUIManager>();
        pickerInitialPos = transform.position;
        pickerRB = GetComponent<Rigidbody>();
        infTrigger = GameObject.FindGameObjectWithTag("InfTrigger");
        if (infTrigger == null) Debug.LogError("No GameObject found with InfTrigger tag on Awake.");
    }

    private void FixedUpdate()
    {
        if (PickerCanMove)
        {
            Movement();
        }
    }
    #region MouseMovement
    //public void MouseMovement()
    //{
    //    Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeedX, 0, moveSpeedZ) * Time.fixedDeltaTime;
    //    Vector3 targetPosition = pickerRB.position + moveDirection;
    //    targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

    //    pickerRB.MovePosition(targetPosition);
    //}
    #endregion 
    public void Movement()
    {
        float moveX = 0;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            moveX = touch.deltaPosition.x * moveSpeedX * Time.deltaTime;
        }
        float targetX = Mathf.Clamp(pickerRB.position.x + moveX, minX, maxX);
        Vector3 targetPosition = new Vector3(targetX, pickerRB.position.y, pickerRB.position.z + moveSpeedZ * Time.deltaTime);
        pickerRB.MovePosition(targetPosition);
    }

    public void MovePicker() => PickerCanMove = true;

    public void StopPicker() => PickerCanMove = false;

    public void RestartPickerPos()
    {
        transform.position = pickerInitialPos;
        tempCollectableCount = 0;
    }

    public void ChangePickerColor(Color color)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Renderer>(out Renderer childRenderer))
            {
                childRenderer.material.color = color;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StageArea"))
        {
            StartCoroutine(CheckIfAnyCollected());
            StopPicker();
            ClearCollectedObjects();
        }

        if (other.gameObject.CompareTag("Collectable"))
        {
            HandleCollectable(other.gameObject);
        }

        if (other.gameObject.CompareTag("LevelEnd"))
        {
            if (bonusCoroutine != null)
            {
                StopCoroutine(bonusCoroutine);
            }
            bonusCoroutine = StartCoroutine(BonusReached(other.gameObject));
        }
    }

    private void ClearCollectedObjects()
    {
        foreach (Rigidbody rb in collectedObjects)
        {
            if (rb != null && rb.TryGetComponent<CollectableObjects>(out CollectableObjects collectable))
            {
                rb.AddForce(new Vector3(0, 0, 1) * forceValue, ForceMode.Impulse);
            }
        }
        collectedObjects.Clear();
    }

    private void HandleCollectable(GameObject collectableGameObject)
    {
        if (collectableGameObject.TryGetComponent<CollectableObjects>(out CollectableObjects collectable) && !collectable.isPickedUp)
        {
            collectable.isPickedUp = true;
            tempCollectableCount++;
            collectedObjects.Add(collectableGameObject.GetComponent<Rigidbody>());
            audioManager.PlayCollectableSFX();
        }
    }
    private IEnumerator BonusReached(GameObject levelEndObject)
    {
        StopPicker();
        Debug.Log("Picker stopped.");
        yield return new WaitForSeconds(2f);

        Vector3 targetPos = transform.position + new Vector3(0, 0, tempCollectableCount / 2);
        gameObject.transform.DOMove(targetPos, 2f);

        yield return new WaitForSeconds(2f);
        LevelEndBonusObject bonus = FindBonusObject(levelEndObject);

        if (bonus == null)
        {
            Debug.LogError("No LevelEndBonusObject component found on " + levelEndObject.name);
            yield break;
        }

        if (dataHandler == null)
        {
            Debug.LogError("dataHandler is null");
            yield break;
        }

        StopPicker();

        int oldDiamondValue = dataHandler.diamond;
        dataHandler.UpdateDiamond(bonus.diamondValue);
        guiManager.UpdateDiamondText();
        Debug.Log("Diamond value updated from " + oldDiamondValue + " to " + dataHandler.diamond);

        GameManager.Instance.GameEventHandler.TriggerLevelWin();
        Debug.Log("Level win triggered.");
    }

    private LevelEndBonusObject FindBonusObject(GameObject levelEndObject)
    {
        foreach (Transform child in levelEndObject.transform)
        {
            foreach (Transform grandchild in child)
            {
                if (grandchild.CompareTag("BonusObject") && grandchild.TryGetComponent<LevelEndBonusObject>(out LevelEndBonusObject bonus))
                {
                    return bonus;
                }
            }
        }
        return null;
    }
    private IEnumerator CheckIfAnyCollected()
    {
        yield return new WaitForSeconds(2f);
        if (tempCollectableCount == 0) GameManager.Instance.GameEventHandler.TriggerLevelFailed();
    }
}