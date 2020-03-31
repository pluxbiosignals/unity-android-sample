using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

/**
 * <summary>Level manager class that should be used by all modules in the application.</summary>
 */
public class LevelManager : MonoBehaviour
{
    /**
     * Method used to load a new scene, for example, when a specific button is pressed.
     *
     * <param name="scene">Scene name</param>
     */
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    /**
     * Method used to load a new scene, for example, when a specific button is pressed.
     *
     * <param name="scene">Scene identifier</param>
     */
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    /**
     * This method triggers the close of the application, forcing the shutdown.
     */
    public void quitApplication()
    {
        Application.Quit();
    }

    /**
     * Through this method, the next scene in the sequence will be automatically loaded.
     */
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /**
     * Through this method, the previous scene of the overall sequence will be automatically loaded.
     * (pointer will be moved from the current to the previous scene)
     */
    public void LoadPreviousLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}