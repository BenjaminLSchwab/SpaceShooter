using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    MultiLaser ParentLaser;
	// Use this for initialization
	void Start () {
        ParentLaser = GetComponentInParent<MultiLaser>();
        if (ParentLaser)
        {
            ParentLaser.AddLaserToList(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);

        if(ParentLaser)ParentLaser.CheckForLaserReset();
    }

}
