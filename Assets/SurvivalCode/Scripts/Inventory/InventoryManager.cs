using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformers
{
    public class InventoryManager : MonoBehaviour
    {
        public InventorySlot[] inventorySlots;
        public GameObject inventoryPrefab;
        public int maxItemCount = 1;
        public int selectedSlot = -1;

        [Header("References")]
        public GameObject weaponController; // This is the "Containers" object
        public GameObject toolbars;
        public Camera mainCam;

        [Header("Input")]
        [SerializeField] private InputActionAsset Hotbar;
        private InputAction[] hotbarActions = new InputAction[5];

        public void Awake()
        {
            var uiMap = Hotbar.FindActionMap("UI");
            hotbarActions[0] = uiMap.FindAction("Hotbar");
            hotbarActions[1] = uiMap.FindAction("Hotbar1");
            hotbarActions[2] = uiMap.FindAction("Hotbar2");
            hotbarActions[3] = uiMap.FindAction("Hotbar3");
            hotbarActions[4] = uiMap.FindAction("Hotbar4");
        }

        private void Start()
        {
            weaponController = GameObject.Find("WeaponHolder");
            // Start at a default slot
            ChangeSelectedSlot(0);
        }

        public void Update()
        {
            // Drop item with 'G'
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                DropSelectedItem();
            }
            if (weaponController == null) return;
            if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length) return;

            InventoryItem itemInUI = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

            foreach (Transform weapon in weaponController.transform)
            {
                if (weapon.TryGetComponent<ItemHand>(out var handScript))
                {
                    if (itemInUI != null && handScript.item == itemInUI.item)
                    {
                        // ANIMATION FIX: Only set position/rotation IF the weapon was off.
                        // This prevents the script from overriding the Animator every frame.
                        if (!weapon.gameObject.activeSelf)
                        {
                            weapon.gameObject.SetActive(true);
                            //weapon.localPosition = Vector3.zero;
                            //weapon.localRotation = Quaternion.identity;
                        }
                    }
                    else
                    {
                        weapon.gameObject.SetActive(false);
                    }
                }
            }


            // NOTE: I removed the nested for-loops from Update.
            // Forcing weapon.localPosition = zero every frame was what killed your animations.
            // Position is now only set once inside RefreshHandVisuals().
        }

        // ------------------- //
        // SELECTION & VISUALS //
        // ------------------- //

        public void ChangeSelectedSlot(int newValue)
        {
            if (newValue < 0 || newValue >= inventorySlots.Length) return;
            if (selectedSlot == newValue) return;

            if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
                inventorySlots[selectedSlot].Deselect();

            selectedSlot = newValue;
            inventorySlots[selectedSlot].Select();

            RefreshHandVisuals();
        }

        public void RefreshHandVisuals()
        {
            if (weaponController == null) return;
            if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length) return;

            InventoryItem itemInUI = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

            foreach (Transform weapon in weaponController.transform)
            {
                if (weapon.TryGetComponent<ItemHand>(out var handScript))
                {
                    if (itemInUI != null && handScript.item == itemInUI.item)
                    {
                        // ANIMATION FIX: Only set position/rotation IF the weapon was off.
                        // This prevents the script from overriding the Animator every frame.
                        if (!weapon.gameObject.activeSelf)
                        {
                            weapon.gameObject.SetActive(true);
                            weapon.localPosition = Vector3.zero;
                            weapon.localRotation = Quaternion.identity;
                        }
                    }
                    else
                    {
                        weapon.gameObject.SetActive(false);
                    }
                }
            }
        }

        // KEPT AS REQUESTED: Handles hiding a specific item's visual
        public void UpdateHandVisuals(InventoryItem items)
        {
            if (weaponController == null || items == null) return;

            for (int i = 0; i < weaponController.transform.childCount; i++)
            {
                if (weaponController.transform.GetChild(i).GetComponent<ItemHand>().item == items.item)
                {
                    weaponController.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        // ------------------- //
        // ITEM LOGIC          //
        // ------------------- //

        // KEPT AS REQUESTED: Gets data and optionally "uses" the item (decrements count)
        public Item GetSelectedItem(bool use)
        {
            if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length) return null;

            InventorySlot itemSlot = inventorySlots[selectedSlot];
            InventoryItem itemInUI = itemSlot.GetComponentInChildren<InventoryItem>();

            if (itemInUI != null)
            {
                Item itemData = itemInUI.item;
                if (use)
                {
                    itemInUI.count--;
                    if (itemInUI.count <= 0)
                    {
                        Destroy(itemInUI.gameObject);
                        Invoke(nameof(RefreshHandVisuals), 0.05f);
                    }
                    else
                    {
                        itemInUI.RefreshCount();
                    }
                }
                return itemData;
            }
            return null;
        }

        public bool AddItems(Item item)
        {
            // Try stack
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventoryItem itemInUI = inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (itemInUI != null && itemInUI.item == item && itemInUI.count < maxItemCount && item.stackable)
                {
                    itemInUI.count++;
                    itemInUI.RefreshCount();
                    return true;
                }
            }

            // Find empty
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].GetComponentInChildren<InventoryItem>() == null)
                {
                    SpawnNewItem(item, inventorySlots[i]);
                    return true;
                }
            }
            return false;
        }

        public void SpawnNewItem(Item item, InventorySlot slot)
        {
            GameObject newItem = Instantiate(inventoryPrefab, slot.transform);
            InventoryItem itemComp = newItem.GetComponent<InventoryItem>();
            itemComp.InitialiseItem(item);

            // If this is our active slot, show the model
            if (slot == inventorySlots[selectedSlot]) RefreshHandVisuals();
        }

        public void DropSelectedItem()
        {
            Debug.Log(selectedSlot);
            if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length) return;
            InventorySlot slot = inventorySlots[selectedSlot];
            InventoryItem itemInUI = slot.GetComponentInChildren<InventoryItem>();

            if (itemInUI != null)
            {
                Vector3 spawnPos = mainCam.transform.position + (mainCam.transform.forward * 3f);
                GameObject droppedObj = Instantiate(itemInUI.item.weaponPrefab, spawnPos, mainCam.transform.rotation);

                // Setup Dropped Object
                droppedObj.tag = "DroppedItem";
                droppedObj.layer = LayerMask.NameToLayer("DroppedItem");
                ItemObjects itemLogic = droppedObj.GetComponent<ItemObjects>() ?? droppedObj.AddComponent<ItemObjects>();
                itemLogic.item = itemInUI.item;

                // Make colliders solid
                foreach (BoxCollider box in droppedObj.GetComponentsInChildren<BoxCollider>())
                    box.isTrigger = false;

                // Update UI
                itemInUI.count--;
                if (itemInUI.count <= 0)
                {
                    Destroy(itemInUI.gameObject);
                    Invoke(nameof(RefreshHandVisuals), 0.05f);
                }
                else
                {
                    itemInUI.RefreshCount();
                }
            }
        }

        // ------------------- //
        // INPUT SUBSCRIPTION  //
        // ------------------- //

        public void OnEnable()
        {
            for (int i = 0; i < hotbarActions.Length; i++)
            {
                if (hotbarActions[i] != null)
                {
                    hotbarActions[i].Enable();
                    int index = i;
                    hotbarActions[i].performed += _ => ChangeSelectedSlot(index);
                }
            }
        }

        public void OnDisable()
        {
            foreach (var action in hotbarActions) action?.Disable();
        }
    }
}