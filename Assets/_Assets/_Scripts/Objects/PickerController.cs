using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerController : MonoBehaviour, IPicker
{
    private AudioManager audioManager;
    private GameObject infTrigger;
    [SerializeField] private float moveSpeedX = 5f;
    [SerializeField] private float moveSpeedZ = 5f;
    [SerializeField] private float minX = -3.3f;
    [SerializeField] private float maxX = 3.3f;

    private Rigidbody pickerRB;
    private List<Rigidbody> collectedObjects = new List<Rigidbody>();
    private Vector3 pickerInitialPos;

    private bool PickerCanMove;
    private float forceValue = .25f;

    private Coroutine bonusCoroutine;
    private LevelDataHandler dataHandler;
    private GUIManager guiManager;

    private int tempCollectableCount = 0;
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
    }

    public void RestartPickerPos()
    {
        transform.position = pickerInitialPos;
        tempCollectableCount = 0;
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

            foreach (Rigidbody rb in collectedObjects)
            {
                if (rb != null)
                {
                    CollectableObjects collectable = rb.GetComponent<CollectableObjects>();
                    if (collectable != null)
                    {
                        rb.AddForce(new Vector3(0, 0, 1) * forceValue, ForceMode.Impulse);
                    }
                }
            }
            collectedObjects.Clear();
        }

        if (other.gameObject.CompareTag("Collectable"))
        {
            CollectableObjects collectable = other.gameObject.GetComponent<CollectableObjects>();
            if (collectable == null) Debug.Log("CollectableObjects component is null");
            else
            {
                if (!collectable.isPickedUp)
                {
                    collectable.isPickedUp = true;
                    tempCollectableCount++;
                    collectedObjects.Add(other.gameObject.GetComponent<Rigidbody>());
                    audioManager.PlayCollectableSFX();
                }
            }
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

    float waitForSec = 2f;
    private IEnumerator BonusReached(GameObject levelEndObject)
    {
        StopPicker();
        Debug.Log("Picker stopped.");
        yield return new WaitForSeconds(waitForSec);

        Vector3 targetPos = transform.position + new Vector3(0, 0, tempCollectableCount / 2);
        gameObject.transform.DOMove(targetPos, 2f);

        yield return new WaitForSeconds(waitForSec);
        LevelEndBonusObject bonus = null; // Search for grandchildren with the BonusObject tag
        foreach (Transform child in levelEndObject.transform)
        {
            foreach (Transform grandchild in child)
            {
                if (grandchild.CompareTag("BonusObject"))
                {
                    bonus = grandchild.GetComponent<LevelEndBonusObject>();
                    if (bonus != null)
                        break;
                }
            }
            if (bonus != null)
                break;
        }

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

}