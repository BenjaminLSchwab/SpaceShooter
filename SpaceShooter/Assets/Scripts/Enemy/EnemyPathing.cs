using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    Enemy Enemy;
    List<Transform> waypoints = new List<Transform>();
    int waypointIndex = 0;
    GameSession GameSession;
    public enum PathType {DeleteAtEnd, Loop, Reverse };
    PathType pathType = PathType.DeleteAtEnd;
    bool isReversed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (waveConfig == null)
        {
            return;            
        }
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[0].position;
        Enemy = GetComponent<Enemy>();
        GameSession = FindObjectOfType<GameSession>();
        pathType = waveConfig.GetPathType();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].position;
            var movementThisFrame = Time.deltaTime * waveConfig.GetMoveSpeed();
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)
            {
                if (isReversed) { waypointIndex--; }
                else { waypointIndex++; }

                if (waypointIndex == 0) { isReversed = false; }             
                
            }

        }
        else
        {
            switch (pathType)
            {
                case PathType.DeleteAtEnd:
                    GameSession.AddToEnemyDeSpawnCount();
                    if (Enemy) Enemy.DestroyLasers();
                    Destroy(gameObject);
                    break;

                case PathType.Loop:
                    waypointIndex = 0;
                    break;

                case PathType.Reverse:
                    isReversed = true;
                    waypointIndex--;
                    break;

            }

            

        }
    }
}
