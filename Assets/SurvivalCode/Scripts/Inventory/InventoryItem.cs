using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Platformers
{
    public class InventoryItem : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler,IPointerClickHandler
    {

        
        [Header("UI")]
        public Image image;
        public TextMeshProUGUI countText;
        public InventoryManager inventoryManager;

        // variables that is used for drag and drop functionality and item data and change health/stamina
        [HideInInspector]public Transform parentAfterDrag;
        [HideInInspector] public int count = 1;
        [HideInInspector] public Item item;
        public InputAction mouse;
        public GameObject healths;
        public GameObject hungers;
        private HealthManager healthManager;
        private StaminaManager staminaManager;
        private GameObject player;
        public GameObject droppedItemPrefab;
        private CharacterStatss characterStats;
        public Camera mainCam;

        public void Start()
        {
            mainCam = Camera.main;
        }
        // enables and disables input actions
        public void OnEnable()
        {
         
            mouse.Enable();
        
        }
        public void OnDisable()
        {
         
            mouse.Disable();

        }

        // finds necessary game objects and components on awake to define variables
        public void Awake()
        {
            healths = GameObject.Find("HealthManager");
            hungers = GameObject.Find("StaminaManager");
            healthManager = healths.GetComponent<HealthManager>();
            staminaManager = hungers.GetComponent<StaminaManager>();
            player = GameObject.Find("FPSController");
            characterStats = GameObject.Find("Characters").GetComponent<CharacterStatss>();

        }

        // initializes item into inventory item
        public void InitialiseItem(Item newItem)
        {
            item = newItem;
            item.weaponPrefab = newItem.weaponPrefab;
            image.sprite = newItem.image;
            image.color = new Color(9, 95, 154, 400);
            RefreshCount();


        }
        // updates the count display
        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool isActive = count > 1;
            countText.gameObject.SetActive(isActive);
        }

        // this section is the drag and drop functionality of the inventory item 
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent;

            EquipmentSlot startingSlot = parentAfterDrag.GetComponent<EquipmentSlot>();
            if (startingSlot != null)
            {
                startingSlot.UpdateEquipmentStats();
            }

            // --- FIX STARTS HERE ---
            // Find the canvas this item belongs to
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                transform.SetParent(canvas.transform);
            }
            else
            {
                // Fallback if canvas isn't found for some reason
                transform.SetParent(transform.root);
            }
            // --- FIX ENDS HERE ---

            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;

            transform.SetParent(parentAfterDrag);

            
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            if (eventData.button == PointerEventData.InputButton.Right)
            {

                if (FirstPersonController.isInventoryOpen) {
                    UseSelectedItem();
                }
                
                

            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {

                if (FirstPersonController.isInventoryOpen) {
                    DropItem();
                }
            }

        
        }
        

        public void DropItem(bool surviveSceneLoad = false)
        {

            Vector3 spawnPos = mainCam.transform.position + (mainCam.transform.forward * 3f);
            GameObject droppedObj = Instantiate(item.weaponPrefab, spawnPos, mainCam.transform.rotation);
            // Instantiate the dropped item (which should have the ItemObject script attached)
            

                Debug.Log("Instantiated Dropped Item: " + player.transform.position);
            // Get the ItemObject component from the instantiated GameObject
                ItemObjects itemObjectComponent = droppedObj.GetComponent<ItemObjects>();
            droppedObj.tag = "DroppedItem";
                droppedObj.layer = LayerMask.NameToLayer("DroppedItem");
                ItemObjects itemLogic = droppedObj.GetComponent<ItemObjects>() ?? droppedObj.AddComponent<ItemObjects>();
                itemLogic.item = item;
                foreach (BoxCollider box in droppedObj.GetComponentsInChildren<BoxCollider>())
                    box.isTrigger = false;




            // Check if ItemObject component exists, which it should if the prefab is set up correctly
            //if (droppedGameObject.TryGetComponent<ItemObjects>(out var itemObject))
            //        {
            //            // Pass the item data from this InventoryItem to the new ItemObject
            //            itemObjectComponent.SetItem(item,targetDropPosition);
            //            itemObjectComponent.position = targetDropPosition;
            //        // Use the new SetItem method

            //        if (droppedGameObject.TryGetComponent<Rigidbody>(out var rb))
            //        {
            //            // Add force relative to player's forward direction
            //           rb.AddForce(player.transform.forward * 5f, ForceMode.VelocityChange);; // Increased force for more noticeable effect
            //        }
            //    }
                
                count--;
                if (count <= 0)
                {
                    Destroy(gameObject); // Destroy the UI inventory item slot if empty
                }
                else
                {
                    RefreshCount();
                }
            
        }
       
        public void UseSelectedItem()
        {

            InventorySlot slot = transform.parent.GetComponent<InventorySlot>();
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                Item newItem = item.item;


                if (newItem.actionType == ActionType.Consumable) 
                {
                    Debug.Log(healthManager);

                    healthManager.Heal(newItem.healthBonus);
                    staminaManager.ChangeHunger(newItem.staminaBonus);
                    item.count--;
                    
                    if (item.count <= 0)
                    {
                        Destroy(item.gameObject);
                    }
                    else
                    {
                        item.RefreshCount();
                    }
                }

                
                

            }
           
        }



    }
}
