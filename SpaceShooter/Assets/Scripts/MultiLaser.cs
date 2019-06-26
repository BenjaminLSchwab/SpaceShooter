using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLaser : MonoBehaviour
{
    private List<GameObject> ChildLasers;
    // Start is called before the first frame update
    void Start()
    {
        ChildLasers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForLaserReset()//makes sure the parent laser object gets set to unactive for item pooling reasons
    {
        var shouldReset = true;
        foreach (var Laser in ChildLasers)
        {
            if (Laser.activeInHierarchy)
            {
                shouldReset = false;
            }
        }
        if (shouldReset)
        {
            gameObject.SetActive(false);
        }
    }

    public void AddLaserToList(GameObject Laser)
    {
        ChildLasers.Add(Laser);
    }
}
