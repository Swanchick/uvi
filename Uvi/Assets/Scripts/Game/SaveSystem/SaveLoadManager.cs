using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    protected string filePath;

    private void Start()
    {
        filePath = $@"{Application.persistentDataPath}/save.sav";

        // LoadGame();
    }

    public void SaveGameByTrigger(GameObject Trigger)
    {
        Save save = new Save(Trigger);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        save.SaveObjects(allGameObjects);

        binaryFormatter.Serialize(fileStream, save);

        fileStream.Close();
    }

    public void SaveGame()
    {
        Save save = new Save();
        
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        save.SaveObjects(allGameObjects);

        binaryFormatter.Serialize(fileStream, save);

        fileStream.Close();
    }

    public virtual void DeleteSaveTriggers(Save save)
    {
        foreach (string name in save.Triggers)
        {
            GameObject trigger = GameObject.Find(name);
            Destroy(trigger);
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(filePath)) return;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Open);

        Save save = (Save)binaryFormatter.Deserialize(fileStream);
        DeleteSaveTriggers(save);
        save.LoadPlayer();
        save.LoadWeapons();
        save.LoadDynamicObjects();
    }

    protected void LoadScene()
    {
        if (!File.Exists(filePath)) return;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Open);

        Save save = (Save)binaryFormatter.Deserialize(fileStream);

        if (SceneManager.GetActiveScene().buildIndex == save.LevelData) return;

        print(save.LevelData);

        SceneManager.LoadScene(save.LevelData);
    }
}

[Serializable]
public class Save
{
    public List<WeaponSaveData> WeaponsData = new List<WeaponSaveData>();
    public List<DynamicObjectData> DynamicsData = new List<DynamicObjectData>();
    public List<string> Triggers = new List<string>();
    public PlayerSaveData PlayerData;
    public int LevelData;
    public Save() { }

    public Save(GameObject Trigger)
    {
        LevelData = SceneManager.GetActiveScene().buildIndex;
        Triggers.Add(Trigger.name);
    }


    [Serializable]
    public struct Vec3
    {
        public float x, y, z;

        public Vec3(Vector3 Position)
        {
            this.x = Position.x;
            this.y = Position.y;
            this.z = Position.z;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public struct Quat
    {
        public float x, y, z, w;

        public Quat(Quaternion Rotation)
        {
            this.x = Rotation.x;
            this.y = Rotation.y;
            this.z = Rotation.z;
            this.w = Rotation.w;
        }

        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    [Serializable]
    public struct WeaponSaveData
    {
        public Vec3 Position;
        public Quat Rotation;
        public int Ammo;
        public string Name;

        public WeaponSaveData(Vec3 Position, Quat Rotation, string Name, int Ammo)
        {
            this.Position = Position;
            this.Rotation = Rotation;
            this.Name = Name;
            this.Ammo = Ammo;
        }
    }

    [Serializable]
    public struct PlayerSaveData
    {
        public Vec3 Position;
        public Quat Rotation;
        public string WeaponName;
        public float Health;

        public PlayerSaveData(Vec3 Position, Quat Rotation, string WeaponName, float Health)
        {
            this.Position = Position;
            this.Rotation = Rotation;
            this.WeaponName = WeaponName;
            this.Health = Health;
        }
    }

    [Serializable]
    public struct DynamicObjectData
    {
        public Vec3 Position;
        public Quat Rotation;
        public string Name;

        public DynamicObjectData(Vec3 Position, Quat Rotation, string Name)
        {
            this.Position = Position;
            this.Rotation = Rotation;
            this.Name = Name;
        }
    }

    public void SaveObjects(GameObject[] gameObjects)
    {   
        foreach (object _object in gameObjects)
        {
            GameObject ent = (GameObject)_object;

            if (ent.isStatic) continue;

            Vec3 pos = new Vec3(ent.transform.position);
            Quat rot = new Quat(ent.transform.rotation);

            if (ent.GetComponent<Player>())
            {
                Player ply = ent.GetComponent<Player>();

                string WeaponName = string.Empty;
                
                if (ply.Weapon)
                    WeaponName = ply.Weapon.gameObject.name;

                PlayerData = new PlayerSaveData(pos, rot, WeaponName, ply.Health);
            }
            else if (ent.GetComponent<WeaponBase>())
            {
                WeaponBase weaponBase = ent.GetComponent<WeaponBase>();

                WeaponsData.Add(new WeaponSaveData(pos, rot, ent.name, weaponBase.Ammo));
            }
            else
            {
                if (ent.tag == "Player" || ent.tag == "DontSave") continue;
                DynamicsData.Add(new DynamicObjectData(pos, rot, ent.name));
            }
        }
    }

    public void LoadPlayer()
    {
        GameObject[] ent = GameObject.FindGameObjectsWithTag("Player");

        Player ply = ent[0].GetComponent<Player>();

        ply.LoadSave(PlayerData);
    }

    public void LoadWeapons()
    {
        foreach (WeaponSaveData save in WeaponsData)
        {
            GameObject weaponObject = GameObject.Find(save.Name);

            weaponObject.transform.position = new Vector3(save.Position.x, save.Position.y, save.Position.z);
            weaponObject.transform.rotation = new Quaternion(save.Rotation.x, save.Rotation.y, save.Rotation.z, save.Rotation.w);
        }
    }

    public void LoadDynamicObjects()
    {
        foreach (DynamicObjectData save in DynamicsData)
        {
            GameObject ent = GameObject.Find(save.Name);

            ent.transform.position = new Vector3(save.Position.x, save.Position.y, save.Position.z);
            ent.transform.rotation = new Quaternion(save.Rotation.x, save.Rotation.y, save.Rotation.z, save.Rotation.w);
        }
    }
}