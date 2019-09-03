using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    [SerializeField] float amplitude = 1f;
    [SerializeField] float frequecy = 1f;
    GameObject child;
    float counter;

    private void Start()
    {
        counter = 0;
        child = GetComponentInChildren<Laser>().gameObject;
    }

    private void OnEnable()
    {
        counter = 0;
        child = GetComponentInChildren<Laser>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime * frequecy;
        child.transform.localPosition = new Vector3(Mathf.Sin(counter) * amplitude, 0, 0);
    }
}
