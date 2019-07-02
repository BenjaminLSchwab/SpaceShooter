using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> rocks;
    [SerializeField] int minRocksPerBlast = 1;
    [SerializeField] int maxRocksPerBlast = 3;
    [SerializeField] float rockSpeed = 1f;
    [SerializeField] float rockSpreadWidth = 1f;
    [SerializeField] float rockSpreadDepth = 1f;
    [SerializeField] GameObject warningSign;
    [SerializeField] float minTimeBetweenBlasts = 10f;
    [SerializeField] float maxTimeBetweenBlasts = 15f;
    [SerializeField] float warningTime = 1.5f;
    [SerializeField] bool repeat;
    bool warningActive = false;
    List<GameObject> rockPool;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownAndBlast());
        rockPool = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CountDownAndBlast()
    {
        while (repeat)
        {

        

        var time = UnityEngine.Random.Range(minTimeBetweenBlasts, maxTimeBetweenBlasts);
        yield return new WaitForSeconds(time);

        StartCoroutine(ShowWarningSign());
        yield return new WaitForSeconds(warningTime);


        SpawnRocks();
        }
    }

    private void SpawnRocks()
    {
        var rocksThisBlast = UnityEngine.Random.Range(minRocksPerBlast, maxRocksPerBlast);
        for (int i = 0; i < rocksThisBlast; i++)
        {
            var randomXNudge = UnityEngine.Random.Range(-rockSpreadWidth, rockSpreadWidth);
            Vector3 pos = new Vector3(randomXNudge, i * rockSpreadDepth);

            var rock = FindFirstInactiveRock();
            if (rock == null)
            {
                var rockType = UnityEngine.Random.Range(0, rocks.Count);
                rock = Instantiate(rocks[rockType], transform.position + pos, transform.rotation);
                rockPool.Add(rock);
            }
            else
            {
                rock.SetActive(true);
                rock.transform.position = transform.position + pos;
                rock.transform.rotation = transform.rotation;
            }
            
            var rb = rock.GetComponent<Rigidbody2D>();
            rb.velocity = transform.up * -1 * rockSpeed;
        }
    }

    IEnumerator ShowWarningSign()
    {
        warningActive = true;
        StartCoroutine(FlashWarningSign());
        yield return new WaitForSeconds(warningTime);
        warningActive = false;

    }

    IEnumerator FlashWarningSign()
    {
        while (warningActive)
        {
            warningSign.SetActive(!warningSign.activeInHierarchy);
            yield return new WaitForSeconds(.25f);
        }
    }

    GameObject FindFirstInactiveRock()
    {
        if (rockPool.Count == 0)
        {
            return null;
        }
        foreach (GameObject rock in rockPool)
        {
            if (!rock.activeInHierarchy) return rock;
        }
        return null;
    }
}
