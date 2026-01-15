using Platformers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitching : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Toolbar;
    public GameObject player;
    InventorySlot[] slots;
    InventoryManager inventoryManager;
    void Awake()
    {
        Toolbar = GameObject.Find("Toolbar");
        player = GameObject.FindWithTag("FPSController");
        slots = Toolbar.GetComponentsInChildren<InventorySlot>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryManager.selectedSlot != -1) {

            InventoryItem item = slots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                // Here you can implement the logic to switch the item in the player's hand
                // For example, you might want to instantiate the item model or enable it
                this.transform.parent = player.transform;


                GameObject itemModel = Instantiate(item.gameObject, player.transform.position, player.transform.rotation);
                itemModel.transform.SetParent(player.transform);
                

                itemModel.transform.localPosition = Vector3.zero;
                itemModel.transform.localRotation = Quaternion.identity;
            }
        }
        

        
    }
}
