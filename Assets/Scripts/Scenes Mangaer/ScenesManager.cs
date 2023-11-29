using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public enum Scene
    {
        MenuScene,
        MapComplete
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(Scene.MapComplete.ToString());
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scene.MenuScene.ToString());
    }
}
