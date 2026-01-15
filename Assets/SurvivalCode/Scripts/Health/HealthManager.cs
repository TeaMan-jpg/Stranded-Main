using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
namespace Platformers
{
    public class HealthManager : MonoBehaviour
    {
        // Manages player health, damage, and healing mechanics
        public Slider healthSlider;
        public float health = 100f;
        public InputAction Heals;
        public InputAction TakeDamages;
        public static event Action PlayerDead;
        public InventorySlot[] inventorySlots;
        public float maxHealth;
        public CharacterStatss CharacterStats;

        // sets initial max health value
        void Start()
        {
            CharacterStats = GameObject.Find("Characters").GetComponent<CharacterStatss>();
            maxHealth = CharacterStats.MaxHealth;
            health = maxHealth;
            health = 100;
            maxHealth = 100;
            healthSlider.maxValue = 1;
            healthSlider.value = health / maxHealth;

        }


        // this section is for taking damage and healing

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0f) {
                health = 0f;
                // Trigger player death event
                PlayerDead?.Invoke();
                healthSlider.value = 0f / maxHealth;
            }

            healthSlider.value = health / maxHealth;


        }
        // heals the player by a specified amount
        public void Heal(int amount)
        {
            health += amount;
            Debug.Log("Player health: " + health);
            if (health > maxHealth) health = maxHealth;
            healthSlider.value = health / maxHealth;

            Debug.Log("Updated health: " + health);
        }
    }
}
