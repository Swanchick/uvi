using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameManagerCollection GameManager;
    [SerializeField] private GameObject LoadingMenu;
    [SerializeField] private SaveLoadManager LoadManager;
    [SerializeField] private GameObject Canvas;
    
    private void LoadLevel(int level)
    {
        GameObject menu = Instantiate(LoadingMenu, Vector2.zero, Quaternion.identity, Canvas.transform);

        LoadSceneManager manager = menu.GetComponent<LoadSceneManager>();

        manager.Setup(level);
    }

    public void NewGame()
    {
        GameManager.LoadSavesState = false;
        gameObject.SetActive(false);

        LoadLevel(1);
    }

    public void Continue()
    {
        
    }

    public void Settings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
