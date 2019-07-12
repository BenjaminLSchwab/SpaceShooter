using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float nextLevelDelay = 2f;
    public void LoadStartMenu()
    {
        var gameSession = FindObjectOfType<GameSession>();
        gameSession.ResetScore();
        SceneManager.LoadScene(0);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }


    public void LoadGame()
    {
        SceneManager.LoadScene("Level_01");
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad(nextLevelDelay));
    }

    IEnumerator WaitAndLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {

        StartCoroutine(WaitAndLoadNextLevel());
    }

    IEnumerator WaitAndLoadNextLevel()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
