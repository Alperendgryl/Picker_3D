using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;

    void Update()
    {
        if (!Application.isEditor && !Application.isPlaying) return; // check if in editor or play mode

        // move camera on z-axis based on arrow key input
        float z = transform.position.z;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            z += moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            z -= moveSpeed * Time.deltaTime;
        }

        // restrict movement to only z-axis
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
