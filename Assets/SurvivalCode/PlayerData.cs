using Platformers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Start is called before the first frame update

    public int health = 100;
    private FirstPersonController player;

    public PlayerData(FirstPersonController player)
    {
        this.player = player;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
