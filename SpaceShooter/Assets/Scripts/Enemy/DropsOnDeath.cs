using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsOnDeath : MonoBehaviour
{
    [SerializeField] GameObject Item;
    [SerializeField] float speed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        var drop = Instantiate(Item, transform.position, Quaternion.identity);
        var rb = drop.GetComponent<Rigidbody2D>();
        var playerPos = FindObjectOfType<Player>().transform.position;
        if (rb) { rb.velocity = (playerPos - transform.position).normalized * speed; }
    }
}
