using UnityEngine;

public class RT_Camera : MonoBehaviour
{
    private void OnValidate()
    {
        Camera camera = GetComponent<Camera>();

        if (camera)
        {
            camera.clearFlags = CameraClearFlags.Nothing;
            Debug.Log(camera.clearFlags);
        }
    }
}
