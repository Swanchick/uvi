using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> Tasks;

    [SerializeField] private List<GameObject> TasksRegister;

    public List<Task> GetTasks()
    {
        return Tasks;
    }

    public void AddTaskByName(string name)
    {
        foreach (GameObject obj in TasksRegister)
        {
            Task task = obj.GetComponent<Task>();

            if (task == null) continue;

            if (task.TaskName == name)
                AddTask(obj);
        }
    }

    public void AddTask(GameObject task)
    {
        GameObject createdObj = Instantiate(task, Vector2.zero, Quaternion.identity, transform);
        Task createdTask = createdObj.GetComponent<Task>();
        Tasks.Add(createdTask);
        createdTask.Init(this);
    }

    public void SetScore(string taskName, int score)
    {
        if (!TaskExist(taskName)) return;

        foreach (Task task in Tasks)
        {
            if (task.TaskName != taskName) continue;

            task.SetScore(score);
        }

        StartCoroutine(CheckCompleted());
    }

    public void AddScore(string taskName)
    {
        if (!TaskExist(taskName)) return;
        
        foreach (Task task in Tasks)
        {
            if (task.TaskName != taskName) continue;

            task.AddScore();
        }

        StartCoroutine(CheckCompleted());
    }

    public bool TaskExist(string name)
    {
        foreach (Task task in Tasks)
        {
            if (task.TaskName == name)
                return true;
        }
        
        return false;
    }

    public IEnumerator CheckCompleted()
    {
        foreach (Task task in Tasks)
        {
            if (!task.Completed) continue;

            yield return new WaitForSeconds(3f);

            Tasks.Remove(task);
            Destroy(task.gameObject);
            break;
        }
    }

    public void LoadTasks(Save data)
    {
        List<Save.TaskData> tasks = data.Tasks;

        foreach (Save.TaskData task in tasks)
        {
            AddTaskByName(task.name);

            if (!TaskExist(task.name)) continue;

            SetScore(task.name, task.score);
        }

        Debug.Log("All task succefully loaded!");
    }
}