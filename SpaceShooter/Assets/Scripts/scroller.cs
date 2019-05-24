using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;
    Material material;
    Vector2 offset;


    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        offset = new Vector2(0, scrollSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
