using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public int poolPassed = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
