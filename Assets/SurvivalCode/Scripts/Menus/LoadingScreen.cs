using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformers
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("Optional UI")]
        public Slider progressBar;

        private void Start()
        {
            //store target scene before loading
            StartCoroutine(LoadAsync(LoadingData.TargetScene));
        }

        private System.Collections.IEnumerator LoadAsync(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("LoadingData.TargetScene is empty!");
                yield break;
            }

            //start loading the next scene in the background
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

            op.allowSceneActivation = false;

            while (!op.isDone)
            {
                float progress = Mathf.Clamp01(op.progress / 0.9f);

                if (progressBar != null)
                    progressBar.value = progress;

                //when it's basically loaded, allow activation
                if (op.progress >= 0.9f)
                {
                    op.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }

    //static holder
    public static class LoadingData
    {
        public static string TargetScene;
    }
}
