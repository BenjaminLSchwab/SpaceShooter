using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayChildren : MonoBehaviour
{
    [SerializeField] float time = 1f;
    public List<GameObject> children;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndAwake());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitAndAwake()
    {
        yield return new WaitForSeconds(time);
        print("Awaking!");
        foreach (var child in children)
        {
            child.SetActive(true);
        }
    }
}
