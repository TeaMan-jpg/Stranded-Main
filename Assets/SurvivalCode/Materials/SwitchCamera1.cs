using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera1 : MonoBehaviour
{
    [Header("Camera Setup")]
    public GameObject camera1;
    public GameObject camera2;

    [Header("Animation Setup")]
    public Animator playerAnimator; // Drag your Player's Animator here
    public int targetLayerIndex = 1; // 0 is Default. 1 is usually your new layer.

    public int Manager;

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
        GetComponent<Animator>().SetTrigger("Change1");
    }

    public void Cam_1()
    {
        camera1.SetActive(true);
        camera2.SetActive(false);

        // Turn OFF the special layer (Weight = 0)
        if (playerAnimator != null)
        {
            playerAnimator.SetLayerWeight(targetLayerIndex, 0f);
        }
    }

    public void Cam_2()
    {
        camera1.SetActive(false);
        camera2.SetActive(true);

        // Turn ON the special layer (Weight = 1)
        if (playerAnimator != null)
        {
            playerAnimator.SetLayerWeight(targetLayerIndex, 1f);
        }
    }
}