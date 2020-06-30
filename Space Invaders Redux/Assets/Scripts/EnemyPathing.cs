using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] WaveConfig waveConfig;
    private List<Transform> waypoints = new List<Transform>();
    private int chCount;
    [SerializeField] float speed;

    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        speed = waveConfig.GetMoveSpeed();
        chCount = waypoints.Count;

        transform.position = waypoints[0].transform.position;
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    int i = 1;
    Vector3 newPos;
    private void Move()
    {
        newPos = Vector3.MoveTowards(transform.position, waypoints[i].position, speed * Time.fixedDeltaTime);
        this.gameObject.transform.position = newPos;
        if (newPos == waypoints[i].position)
        {
            i++;
        }
        if (i == chCount)
        {
            if (!waveConfig.doLoop) { Destroy(this.gameObject); }
            else { i = waveConfig.loopStartWaypointIndex; }
        }

    }

    public void SetWaveConfig(WaveConfig w)
    {
        this.waveConfig = w;
    }
}
