using UnityEngine;

public class LevelTransitionOnInteract : MonoBehaviour
{
    [Header("Level Objects")]
    [SerializeField] private GameObject level1Root;
    [SerializeField] private GameObject level2Root;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;

    [Header("Interact")]
    [SerializeField] private float interactRadius = 20f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool used;

    void Start()
    {
        // Ensure level 2 is off at start
        if (level2Root != null) level2Root.SetActive(false);
    }

    void Update()
    {
        if (used) return;

        Transform p = player;
        if (p == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) p = go.transform;
        }
        if (p == null) return;

        float dist = Vector3.Distance(p.position, transform.position);

        if (dist <= interactRadius && Input.GetKeyDown(interactKey))
        {
            used = true;
            DoTransition(p);
        }
    }

    private void DoTransition(Transform p)
    {
        // Disable Level 1
        if (level1Root != null) level1Root.SetActive(false);

        // Enable Level 2
        if (level2Root != null) level2Root.SetActive(true);

        // 3) Teleport player to spawn
        var cc = p.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        p.position = spawnPosition;

        // Reset velocity
        var rb = p.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (cc != null) cc.enabled = true;
    }
}
