using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    [SerializeField] GameObject projectileType;
    [SerializeField] int numberOfProjectiles = 4;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float explodeTimer = 3f;

    List<GameObject> laserPool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(countDownAndExplode());
    }

    private void Start()
    {
        laserPool = new List<GameObject>();
        StartCoroutine(countDownAndExplode());
    }

    IEnumerator countDownAndExplode()
    {
        yield return new WaitForSeconds(explodeTimer);
        Explode();
    }

    private void Explode()
    {
        float degreesBetweenProjectiles = 360f / numberOfProjectiles;
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            var rotation = Quaternion.Euler(0, 0, i * degreesBetweenProjectiles);
            var projectile = FindFirstInactiveLaser();
            if (!projectile) {
                projectile = Instantiate(projectileType, transform.position, rotation);
                projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * projectileSpeed;
                laserPool.Add(projectile);
            }
            else
            {
                projectile.SetActive(true);
                projectile.transform.position = transform.position;
                projectile.transform.rotation = rotation;
                projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * projectileSpeed;
            }
        }
        gameObject.SetActive(false);
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
