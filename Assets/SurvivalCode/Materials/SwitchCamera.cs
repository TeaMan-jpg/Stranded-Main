using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // <--- 1. REQUIRED FOR NEW INPUT SYSTEM

public class SwitchCamera : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public int Manager;

    // 2. Add Update to listen for key press
    void Update()
    {
        // Check if 'C' key was pressed this frame
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            // Triggers the animation (which I assume calls ManageCamera via an Event)
            ChangeCamera();

            
        }
    }

    public void ManageCamera()
    {

        if (Manager == 0)
        {
            Cam_2();
            Manager = 1;
        }
        else if (Manager == 1)
        {
            Cam_1();
            Manager = 0;
        }
    }

    public void ChangeCamera()
    {
        // Triggers the animation state
        GetComponent<Animator>().SetTrigger("Change");
    }

    public void Cam_1()
    {
        camera1.SetActive(true);
        camera2.SetActive(false);

    }
    public void Cam_2()
    {
        camera1.SetActive(false);
        camera2.SetActive(true);
    }
}