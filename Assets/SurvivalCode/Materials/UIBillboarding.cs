using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    // If you drag your camera here in the Inspector, it will never be null
    [SerializeField] private Camera main;
    private Transform camTransform;

    void Start()
    {
        // 1. If you didn't drag a camera into the Inspector, try to find the MainCamera
        if (main == null)
        {
            main = Camera.main;
        }

        // 2. If we found a camera, store its transform for speed
        if (main != null)
        {
            camTransform = main.transform;
        }
    }

    private void Update()
    {
        // 3. Safety check: Only run if the camera exists
        if (camTransform != null)
        {
            transform.forward = camTransform.forward;
        }
        else
        {
            // Optional: Try to re-find the camera if it was missing at Start
            if (Camera.main != null) camTransform = Camera.main.transform;
        }
    }
}