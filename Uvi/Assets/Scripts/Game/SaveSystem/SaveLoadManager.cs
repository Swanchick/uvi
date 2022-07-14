using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string filePath;
    [SerializeField] private GameManagerCollection GameManager;
    [SerializeField] private TaskManager TaskManager;

    private void Start()
    {
        filePath = $@"{Application.persistentDataPath}/game.json";

        if (!GameManager.LoadSavesState) return;

        LoadGame();

        GameManager.LoadSavesState = false;
    }

    public void SaveGame(int sceneId)
    {
        Save save = new Save();

        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task");

        save.SceneId = sceneId;
        save.SavePlayer(player);
        save.SaveTasks(tasks);

        File.WriteAllText(filePath, JsonUtility.ToJson(save));

        Debug.Log("All saved successfully!");
    }

    public int GetSavedLevel()
    {
        if (!SaveDataExist()) return 0;

        Save save = JsonUtility.FromJson<Save>(File.ReadAllText(filePath));

        return save.SceneId;
    }

    public void ChangeLevel(int indexLevel)
    {
        SaveGame(indexLevel);
        
        SceneManager.LoadScene(indexLevel);
        GameManager.LoadSavesState = true;
    }

    public bool SaveDataExist()
    {
        return File.Exists(filePath);
    }

    public void LoadGame()
    {
        if (!SaveDataExist()) return;

        Player player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        Save save = JsonUtility.FromJson<Save>(File.ReadAllText(filePath));

        player.LoadPlayer(save);
        TaskManager.LoadTasks(save);
    }
}

[Serializable]
public class Save
{
    public int SceneId;
    public PlayerData playerData;
    public List<TaskData> Tasks = new List<TaskData>();

    public void SavePlayer(GameObject player)
    {
        Player ply = player.GetComponent<Player>();
        Health health = player.GetComponent<Health>();

        string weaponClass = string.Empty;

        if (ply.Weapon != null)
            weaponClass = ply.Weapon.WeaponClass;

        playerData = new PlayerData(health.health);
    }

    public void SaveTasks(GameObject[] tasks)
    {
        foreach (GameObject task in tasks)
        {
            Task _task = task.GetComponent<Task>();

            Tasks.Add(new TaskData(_task.TaskName, _task.Score));
        }
    }

    [Serializable]
    public struct PlayerData
    {
        public float health;

        public PlayerData(float health)
        {
            this.health = health;
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