using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Task : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] public string TaskName = string.Empty;
    [SerializeField] protected string Title = string.Empty;
    [SerializeField] protected string Mission = string.Empty;
    [SerializeField] protected Color MainColor;
    [SerializeField] protected Color ChangeColor;
    
    [Header("Options")]
    [SerializeField] public bool Completed;
    [SerializeField] public int Score;
    [SerializeField] protected float MaxScore = 10f;
    [SerializeField] protected float Speed = 4f;
    [SerializeField] protected float ChangeColorSpeed = 4f;

    [Header("Components")]
    [SerializeField] private Text TitleText;
    [SerializeField] public Text MissionText;
    [SerializeField] protected RectTransform ProgressBar;
    [SerializeField] protected TaskManager TaskManager;
    [SerializeField] protected Animator Animator;

    private float ScoreLength;

    private Image ProgressImage;

    public void Start()
    {
        ScoreLength = ProgressBar.sizeDelta.x / MaxScore;
        Animator = GetComponent<Animator>();
        ProgressImage = ProgressBar.GetComponent<Image>();

        MissionText.text = Mission;
        TitleText.text = Title;
    }

    public virtual void Init(TaskManager taskManager)
    {
        TaskManager = taskManager;
    }

    private void Update()
    {
        ProgressBar.sizeDelta = Vector2.Lerp(ProgressBar.sizeDelta, new Vector2(ScoreLength * Score, ProgressBar.sizeDelta.y), Speed * Time.deltaTime);

        TitleText.color = MainColor;

        ProgressImage.color = MainColor;

        if (Completed)
            MainColor = Color.Lerp(MainColor, ChangeColor, ChangeColorSpeed * Time.deltaTime);
    }

    public int GetScore()
    {
        return Score;
    }

    public virtual void SetScore(int score)
    {
        if (Completed) return;

        Score = score;

        if (Score >= MaxScore)
            EndTask();
    }

    public virtual void AddScore()
    {
        if (Completed) return;
        
        Score++;

        if (Score == MaxScore)
            EndTask();
    }

    public virtual void EndTask()
    {
        Completed = true;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        Animator.SetTrigger("Destroy");
    }
}