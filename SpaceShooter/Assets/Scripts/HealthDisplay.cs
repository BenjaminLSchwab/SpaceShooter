using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Player Player;
    [SerializeField] GameObject HealthSprite;
    [SerializeField] Vector3 SpriteSpacing;
    List<GameObject> sprites = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();
        ChangeHealth();
        
    }

    public void ChangeHealth()
    {
        foreach (var sprite in sprites)
        {
            Destroy(sprite);
        }
        sprites.Clear();
        for (int i = 0; i < Player.GetHealth(); i++)
        {
            var HPSprite = Instantiate(HealthSprite, transform.position + SpriteSpacing * i, Quaternion.identity, transform);
            sprites.Add(HPSprite);
        }
    }
}
