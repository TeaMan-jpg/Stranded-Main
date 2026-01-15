using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    //void Start()
    //{
    //    SelectWeapon();
    //}

    //void Update()
    //{
    //    var keyboard = Keyboard.current;
    //    if (keyboard == null) return;

    //    int previousSelectedWeapon = selectedWeapon;

    //    if (!keyboard.uKey.wasPressedThisFrame)
    //    {
    //        return;
    //    }
    //    if (selectedWeapon >= transform.childCount - 1)
    //        selectedWeapon = 0;
    //    else
    //        selectedWeapon++;

    //    if (keyboard.iKey.wasPressedThisFrame)
    //    {
    //        if (selectedWeapon <= 0)
    //            selectedWeapon = transform.childCount - 1;
    //        else
    //            selectedWeapon--;
    //    }

    //    if (keyboard.digit1Key.wasPressedThisFrame) selectedWeapon = 0;
    //    if (keyboard.digit2Key.wasPressedThisFrame) selectedWeapon = 1;
    //    if (keyboard.digit3Key.wasPressedThisFrame) selectedWeapon = 2;
    //    if (keyboard.digit4Key.wasPressedThisFrame) selectedWeapon = 3;
    //    if (keyboard.digit5Key.wasPressedThisFrame) selectedWeapon = 4;
    //    if (keyboard.digit6Key.wasPressedThisFrame) selectedWeapon = 5;

    //                    if (previousSelectedWeapon != selectedWeapon)
    //        SelectWeapon();
    //}

    //private void SelectWeapon()
    //{
    //    int i = 0;
    //    foreach (Transform weapon in transform)
    //    {
    //        weapon.GetChild(0).gameObject.SetActive(i == selectedWeapon);
    //        i++;
    //    }
    //}
}