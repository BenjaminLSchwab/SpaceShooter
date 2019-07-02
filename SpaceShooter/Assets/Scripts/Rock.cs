using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] float destroyTime = 10;
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.SetActive(false);
    }
}
