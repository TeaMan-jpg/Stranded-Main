using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    public int goldReward = 100;
    public float interactRadius = 2.0f;
    public KeyCode interactKey = KeyCode.E;

    bool opened;

    void Update()
    {
        if (opened) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= interactRadius && Input.GetKeyDown(interactKey))
        {
            opened = true;

            FindObjectOfType<PlayerHUD>()?.AddGold(goldReward);

            Level1Objectives.Instance?.ChestOpened();

            Destroy(gameObject);
        }
    }
}
