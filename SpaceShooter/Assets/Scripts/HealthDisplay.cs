using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Player Player;
    Text Text;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();
        Text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = Player.GetHealth().ToString();
    }
}
