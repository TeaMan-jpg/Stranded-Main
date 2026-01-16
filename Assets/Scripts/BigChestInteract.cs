using UnityEngine;

public class BigChestInteract : MonoBehaviour
{
    public float interactRadius = 3f;
    public KeyCode interactKey = KeyCode.E;

    [TextArea] public string winMessage = "You beat Level 1!\n\nPress E to dismiss";
    public float popupSeconds = 0f; // 0 = stay until dismissed

    bool used;

    void Update()
    {
        if (used) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= interactRadius && Input.GetKeyDown(interactKey))
        {
            used = true;

            if (Level1Objectives.Instance != null)
                Level1Objectives.Instance.ShowPopupMessage(winMessage, popupSeconds);
            else
                Debug.Log(winMessage);
        }
    }
}
