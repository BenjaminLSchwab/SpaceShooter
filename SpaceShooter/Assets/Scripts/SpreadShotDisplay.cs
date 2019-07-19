using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotDisplay : MonoBehaviour
{
    Player Player;
    [SerializeField] GameObject Sprite;
    [SerializeField] Vector3 SpriteSpacing;
    List<GameObject> sprites = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();
        ChangeDisplay();

    }

    public void ChangeDisplay()
    {
        foreach (var sprite in sprites)
        {
            Destroy(sprite);
        }
        sprites.Clear();
        for (int i = 0; i < Player.GetSpreadShotCount(); i++)
        {
            var NewSprite = Instantiate(Sprite, transform.position + SpriteSpacing * i, Quaternion.identity, transform);
            sprites.Add(NewSprite);
        }
    }
}
