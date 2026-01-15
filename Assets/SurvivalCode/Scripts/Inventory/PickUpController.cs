//using Platformers;
//using UnityEngine;

//public class PickUpController : MonoBehaviour
//{
//    public WeaponController weaponController; // This script handles the shooting logic

//    public Rigidbody rb;
//    public BoxCollider boxCollider;
//    public SwordCollisionDetector weapon;

//    public Transform player, weaponContainer, fpsCam; // Added fpsCam for throwing direction
//    public float pickUpRange = 3f;
//    public float dropForwardForce, dropUpwardForce;
//    public bool equipped;
//    public static bool slotFull;

//    private void Start()
//    {
//        // Setup initial state
//        if (!equipped)
//        {
            
//            boxCollider.isTrigger = false;
//            //if (weaponController != null) weaponController.enabled = false;
//        }
//        else
//        {
            
//            boxCollider.isTrigger = true;
//            //if (weaponController != null) weaponController.enabled = true;
//            slotFull = true;
//        }
//    }

//    private void Update()
//    {
//        // Check distance to player
//        Vector3 distanceToPlayer = player.position - transform.position;

//        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.P) && !slotFull)
//        {
//            PickUp();
//        }

//        if (equipped && Input.GetKeyDown(KeyCode.B))
//        {
//            Debug.Log("Dropping weapon");
//            Drop();
//        }
//    }

//    private void PickUp()
//    {
//        equipped = true;
//        slotFull = true;

//        // 1. Make the weapon a child of the container on the player
//        transform.SetParent(weaponController.transform);

//        // 2. Reset position and rotation so it snaps to the hand correctly
//        transform.localPosition = Vector3.zero;
//        transform.localRotation = Quaternion.Euler(Vector3.zero);
//        transform.localScale = Vector3.one;

//        // 3. Disable physics so it doesn't fall out of your hand
        
//        boxCollider.isTrigger = true;

//        // 4. Enable the shooting script
//        weapon.enabled = true;
//    }

//    private void Drop()
//    {
//        equipped = false;
//        slotFull = false;

//        // 1. Detach from player
//        transform.SetParent(null);

//        // 2. Re-enable physics
        
//        boxCollider.isTrigger = false;

//        // 3. Give the gun the player's momentum (Realism)
//        // Note: Replace 'player' with the player's Rigidbody if available
//        //Rigidbody playerRb = player.GetComponent<Rigidbody>();
//        //if (playerRb != null) rb.velocity = playerRb.velocity;

//        //// 4. Add "Throw" Force
//        //// Uses the camera's forward direction to throw the gun
//        //rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
//        //rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

//        //// 5. Add random rotation so it looks like it's tumbling
//        //float random = Random.Range(-1f, 1f);
//        //rb.AddTorque(new Vector3(random, random, random) * 10);

//        // 6. Disable shooting
//        weapon.enabled = false;
//    }
//}