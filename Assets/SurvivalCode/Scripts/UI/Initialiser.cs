//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Initialiser : MonoBehaviour
//{
//    // AfterSceneLoad is better for checking the BuildIndex
//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//    public static void Execute()
//    {
//        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

//        // 1. If we are in the Menu (Index 0), do NOT spawn persistence
//        if (currentBuildIndex == 0) return;

//        // 2. If Persistence ALREADY exists, do NOT spawn another one
//        // (This prevents duplicates when moving between Level 1 and Level 2)
//        if (GameObject.Find("Persistence") != null || GameObject.Find("Persistence(Clone)") != null)
//        {
//            return;
//        }

//        if (currentBuildIndex == 1) {
//            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Persistence")));

//        }
//        // 3. Spawn the Persistence prefab from the Resources folder
//        //GameObject prefab = Resources.Load<GameObject>("Persistence");
//        //if (prefab != null)
//        //{
//        //    GameObject instance = Object.Instantiate(prefab);
//        //    instance.name = "Persistence"; // Rename to keep it clean
//        //    Object.DontDestroyOnLoad(instance);
//        //}
//    }
//}