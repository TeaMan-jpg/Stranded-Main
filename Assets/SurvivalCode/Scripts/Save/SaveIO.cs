using System;
using UnityEngine;

namespace Platformers
{
    //data stored as JSON in PlayerPrefs
    [Serializable]
    public class GameSave
    {
        public string sceneName;
        public float masterVolume = 1f;

        public float health = 100f;
        public float maxHealth = 100f;

        //inventory snapshot
        public InventorySave inventory;
    }

    //writes/reads the save data from browser storage
    public static class SaveIO
    {
        private const string KEY = "SAVE_V1";

        //save GameSave to JSON then to PlayerPrefs
        public static void Save(GameSave data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(KEY, json);
            PlayerPrefs.Save(); // important on WebGL
        }

        //load JSON from PlayerPrefs to GameSave
        public static GameSave LoadOrNew()
        {

            if (!PlayerPrefs.HasKey(KEY))
                return new GameSave();

            string json = PlayerPrefs.GetString(KEY);
            if (string.IsNullOrEmpty(json))
                return new GameSave();

            try
            {
                return JsonUtility.FromJson<GameSave>(json) ?? new GameSave();
            }
            catch
            {
                //catches corrupt JSON
                return new GameSave();
            }
        }

        //delete save
        public static void Wipe()
        {
            PlayerPrefs.DeleteKey(KEY);
            PlayerPrefs.Save();
        }
    }
}
