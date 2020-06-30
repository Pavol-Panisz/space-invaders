using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Config", menuName = "New Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Header("Enemy Info")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int enemyCount;
    [SerializeField] float moveSpeed;
    [SerializeField] float spawnInterval;

    [Header("Path Info")]
    [SerializeField] GameObject pathPrefab;
    
    [Header("Looping Options")]
    public bool doLoop = true;
    public int loopStartWaypointIndex = 0;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public List<Transform> GetWaypoints() {
        int chCount = pathPrefab.transform.childCount;
        List<Transform> waypoints = new List<Transform>();
        
        for (int j = 0; j != chCount; j++)
        {
            waypoints.Add(pathPrefab.transform.GetChild(j));
        }
        return waypoints;
    }
    public float GetSpawnInterval() { return spawnInterval; }
    public float GetMoveSpeed() { return moveSpeed; }
    public int GetEnemyCount() { return enemyCount; }
}
