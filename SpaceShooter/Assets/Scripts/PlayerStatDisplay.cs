using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatDisplay : MonoBehaviour
{
    [SerializeField] GameObject Sprite;
    [SerializeField] Vector3 SpriteSpacing;
    List<GameObject> sprites = new List<GameObject>();

    public void ChangeDisplay(int newNumber)
    {
        foreach (var sprite in sprites)
        {
            Destroy(sprite);
        }
        sprites.Clear();
        for (int i = 0; i < newNumber; i++)
        {
            var NewSprite = Instantiate(Sprite, transform.position + SpriteSpacing * i, Quaternion.identity, transform);
            sprites.Add(NewSprite);
        }
    }
}
