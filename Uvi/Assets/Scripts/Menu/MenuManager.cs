using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MenuManager : SaveLoadManager
{
    [SerializeField] private GameManager GameManager;
    [SerializeField] private GameObject LoadingMenu;
    [SerializeField] private SaveLoadManager LoadManager;
    

    public void NewGame()
    {
        GameManager.LoadSavesState = false;

        LoadingMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Continue()
    {
        GameManager.LoadSavesState = true;
        
        LoadScene();
    }

    public void Settings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
