using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager 
{

    public static Level CurrentLevel { get; private set; }

    public static void StartLevel(Level level)
    {
        CurrentLevel = level;
        SceneManager.LoadScene(level.Scene);
    }

    public static void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public static void FinishScene() => FinishLevel();

    private static void FinishLevel()
    {
        Debug.Log("Level finished!");
        SceneManager.LoadScene(0);
    }

}

/*
 public static class GameManager // More like Level Manager, but who cares lol.
{

    public static Level currentLevel { get; private set; }
    public static int currentScene { get; private set; }

    public static void StartLevel(Level level)
    {
        currentLevel = level;
        currentScene = 0;
        NextScene();
    }

    public static void FinishScene() => NextScene();

    private static void NextScene()
    {

        if(currentScene + 1 < currentLevel.scenes.Length - 1)
        {
            // Load Next Scene
        }
        else
        {
            FinishLevel();
        }
  
    }

    private static void FinishLevel()
{
    // Load results and all that stuff
    Debug.Log("Level finished!");
}

}

*/