using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapons : MonoBehaviour
{
    [Header("Lasers")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] protected float minTimebetweenShots = 0.2f;
    [SerializeField] protected float maxTimeBetweenShots = 3f;
    [SerializeField] float laserSpawnDistance = .1f;
    [SerializeField] float laserSpeed = 10f;

    [Header("Sounds")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.5f;

    List<GameObject> laserPool = new List<GameObject>();
    protected float shotCounter;
    // Start is called before the first frame update
    protected void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }
    protected void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter < 0)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
        }
    }

    protected void Fire()
    {
        if (laserSound) AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVolume);
        //var laserSpawnPos = transform.position + new Vector3(0, -laserSpawnDistance, -1);
        var laserSpawnPos = transform.position + transform.up * -1 * laserSpawnDistance;
        var laser = FindFirstInactiveLaser();
        if (laser == null)
        {
            laser = Instantiate(laserPrefab, laserSpawnPos, transform.rotation);
            laserPool.Add(laser);
        }
        else
        {
            laser.SetActive(true);
            laser.transform.position = laserSpawnPos;
            laser.transform.rotation = transform.rotation;
        }
        var rb = laser.GetComponent<Rigidbody2D>();
        var vel = transform.up * -laserSpeed;
        rb.velocity = vel;
    }

    public void DestroyLasers()
    {
        foreach (var laser in laserPool)
        {
            Destroy(laser, 3f);
        }
    }


    GameObject FindFirstInactiveLaser()
    {
        if (laserPool.Count == 0)
        {
            return null;
        }
        foreach (GameObject laser in laserPool)
        {
            if (!laser.activeInHierarchy) return laser;
        }
        return null;
    }

}
