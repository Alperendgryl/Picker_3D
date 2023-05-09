using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private PoolObject activePool;
    private List<PoolObject> pools = new List<PoolObject>();

    [SerializeField] private TMP_Text CCValueTXT;
    private void Start()
    {
        FindPoolsInScene();
    }

    public void FindPoolsInScene()
    {
        PoolObject[] foundPools = FindObjectsOfType<PoolObject>();
        pools.Clear();
        pools.AddRange(foundPools);
    }

    public void SetActivePool(PoolObject newActivePool)
    {
        activePool = newActivePool;
        UpdateText();
    }

    public void IncrementActivePoolValue()
    {
        activePool.IncrementPoolValue();
        UpdateText();
    }

    public void DecrementActivePoolValue()
    {
        activePool.DecrementPoolValue();
        UpdateText();
    }

    private void UpdateText()
    {
        CCValueTXT.text = $"0/{activePool.ccValue}";
    }
}
