using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    [SerializeField] float moveSpeed = 2f;
    int waypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].position;
            var movementThisFrame = Time.deltaTime * moveSpeed;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
