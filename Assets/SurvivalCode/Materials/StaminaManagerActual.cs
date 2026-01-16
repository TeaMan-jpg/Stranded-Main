using UnityEngine;
using UnityEngine.UI; // Required for the Slider component

public class StaminaManagerActual : MonoBehaviour
{
    [Header("Stamina Values")]
    public float maxStamina = 100f;
    public float currentStamina;

    [SerializeField] public Slider hunger;

    [Header("Rates")]
    [Tooltip("Stamina lost per second.")]
    public float drainRate = 15f;
    [Tooltip("Stamina recovered per second.")]
    public float regenRate = 1f;

    [Header("UI Reference")]
    [Tooltip("Drag your UI Slider component here.")]
    public Slider staminaSlider;

    private bool isDraining = false;

    void Start()
    {
        currentStamina = maxStamina;

        // Initial setup of the slider
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        // 1. Check if Stamina should be drained or regenerated
        if (isDraining && currentStamina > 0)
        {
            // Drain stamina
            currentStamina -= drainRate * Time.deltaTime;
        }
        else if (currentStamina < maxStamina)
        {
            // Regenerate stamina
            currentStamina += regenRate * Time.deltaTime;
        }

        // 2. Clamp the value to ensure it stays between 0 and Max
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // 3. Update the UI Slider (THE CORE GUI LOGIC)
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }

        // Example: If Stamina runs out, force isDraining to false (stop sprinting)
        if (currentStamina <= 0)
        {
            isDraining = false;
        }

        
    }

    // --- PUBLIC INTERFACE FUNCTIONS ---

    // Called by the PlayerController when an action starts (e.g., sprinting)
    public void StartStaminaDrain()
    {
        isDraining = true;
    }

    // Called by the PlayerController when an action stops
    public void StopStaminaDrain()
    {
        isDraining = false;
    }

    // Check if the player has enough stamina for an action
    public bool CanPerformAction()
    {
        return currentStamina > 0;
    }
}