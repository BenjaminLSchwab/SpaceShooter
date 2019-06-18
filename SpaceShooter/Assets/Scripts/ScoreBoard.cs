using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{

    Text scoreText;
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameSession)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
        scoreText.text = gameSession.GetScore().ToString();
    }
}
