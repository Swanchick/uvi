using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> Tasks;

    public List<Task> GetTasks()
    {
        return Tasks;
    }

    public void AddTask(GameObject task)
    {
        GameObject createdObj = Instantiate(task, Vector2.zero, Quaternion.identity, transform);
        Task createdTask = createdObj.GetComponent<Task>();
        Tasks.Add(createdTask);
        createdTask.Init(this);
    }

    public void AddScore(string taskName)
    {   
        foreach (Task task in Tasks)
        {
            if (task.TaskName != taskName) continue;

            task.AddScore();
        }

        StartCoroutine(CheckCompleted());
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
}