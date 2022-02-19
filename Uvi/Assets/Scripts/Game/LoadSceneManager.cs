using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Text LoadingText;
    [SerializeField] private RectTransform ProgressBar;

    private float BorderWide;

    public void Start()
    {
        BorderWide = ProgressBar.sizeDelta.x;
        ProgressBar.sizeDelta = new Vector2(0, ProgressBar.sizeDelta.y);
    }

    public void Setup(int levelId)
    {
        StartCoroutine(AsyncLoadScene(levelId));
    }

    private IEnumerator AsyncLoadScene(int Level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Level);
        
        while (!operation.isDone)
        {
            int progress = Mathf.RoundToInt(operation.progress / 0.9f);

            ProgressBar.sizeDelta = Vector2.Lerp(ProgressBar.sizeDelta, new Vector2(BorderWide * progress, ProgressBar.sizeDelta.y), 4f * Time.deltaTime);

            LoadingText.text = $"{progress * 100}%";

            yield return new WaitForSeconds(0.2f);
        }
    }
}
