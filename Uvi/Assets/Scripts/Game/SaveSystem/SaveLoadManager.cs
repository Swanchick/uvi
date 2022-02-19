using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string filePath;
    [SerializeField] private GameManagerCollection GameManager;
    [SerializeField] private TaskManager TaskManager;

    private void Start()
    {
        filePath = $@"{Application.persistentDataPath}/save.sav";
    }

    public void SaveGame(int sceneId)
    {
        Save save = new Save();

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        Player ply = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        save.SaveScene(sceneId);
        save.SavePlayer(ply);
        save.SaveTasks(TaskManager.GetTasks());

        binaryFormatter.Serialize(fileStream, save);
        fileStream.Close();
    }

    public bool SaveDataExist()
    {
        return File.Exists(filePath);
    }

    public void LoadGame()
    {
        if (!SaveDataExist()) return;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Open);

        Save save = (Save)binaryFormatter.Deserialize(fileStream);
    }
}

[Serializable]
public class Save
{
    public int SceneId;
    public PlayerData playerData;
    public List<TaskData> Tasks;

    public void SaveScene(int index)
    {
        SceneId = index;
    }

    public void SavePlayer(Player ply)
    {
        Health health = ply.GetComponent<Health>();

        playerData = new PlayerData(health.GetHealth(), ply.Weapon.WeaponClass);
    }

    public void SaveTasks(List<Task> tasks)
    {
        foreach (Task task in tasks)
        {
            TaskData taskData = new TaskData(task.TaskName, task.GetScore());

            Tasks.Add(taskData);
        }
    }

    [Serializable]
    public struct PlayerData
    {
        public float health;
        public string weapon;

        public PlayerData(float health, string weapon)
        {
            this.health = health;
            this.weapon = weapon;
        }
    }

    [Serializable]
    public struct TaskData
    {
        public string name;
        public int score;

        public TaskData(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}