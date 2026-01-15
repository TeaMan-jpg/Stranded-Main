using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Popup : MonoBehaviour
{
    // Start is called before the first frame update
    public static Popup instance;
    public GameObject prefab;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Keyboard.current.f10Key.wasPressedThisFrame)
        {
            CreatePopUp(Vector3.one, Random.Range(0, 1000).ToString(), Color.red);
        }

    }

    

    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;
        Destroy(popup, 1f);

    }
}
