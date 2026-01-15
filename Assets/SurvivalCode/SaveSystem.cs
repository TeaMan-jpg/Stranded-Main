using Platformers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static void SavePlayer(FirstPersonController player)
    {
        //BinaryFormatter formatter = new BinaryFormatter();
        //string path = Application.persistentDataPath + "/player.sav";
        //using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
        //{
        //    PlayerData data = new PlayerData(player);
        //    formatter.Serialize(stream, data);
        //}

        Console.WriteLine("SavePlayer method called.");

    }

    // Update is called once per frame
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.sav";
        if (System.IO.File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                return data;
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
