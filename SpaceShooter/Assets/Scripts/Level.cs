using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    public void LoadStartMenu()
    {
        var gameSession = FindObjectOfType<GameSession>();
        gameSession.ResetScore();
        gameSession.ResetEnemyCounts();
        SceneManager.LoadScene(0);
    }


    public void LoadGame()
    {
        SceneManager.LoadScene("Level_01");
    }

    public void LoadGameOver(int delay = 0)
    {
        StartCoroutine(WaitAndLoad(delay));
    }

    IEnumerator WaitAndLoad(int delay)
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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
