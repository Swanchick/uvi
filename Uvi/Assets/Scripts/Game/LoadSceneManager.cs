using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Text LoadingText;

    private void OnEnable()
    {
        StartCoroutine(AsyncLoadScene(1));
    }

    private IEnumerator AsyncLoadScene(int Level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Level);
        while (!operation.isDone)
        {
            int progress = Mathf.RoundToInt(operation.progress / 0.9f);

            // Debug.Log(progress * 100);

            LoadingText.text = $"{progress * 100}%";

            yield return null;
        }
        
    }
}
