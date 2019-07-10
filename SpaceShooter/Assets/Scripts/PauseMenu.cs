using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseScreen;
    public bool isPaused = false;
    //Player Player;


    private void Start()
    {
        //Player = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        //Player.gameObject.SetActive(false);
        isPaused = true;
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        //Player.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        PauseScreen.SetActive(false);
    }

    public void Quit()
    {
        FindObjectOfType<Level>().QuitGame();   
    }
}
