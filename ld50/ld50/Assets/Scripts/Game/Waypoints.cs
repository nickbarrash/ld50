using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : Singleton<Waypoints>
{
    public GameObject waypointPrefab;

    private int activeWaypoint = 0;
    private List<Waypoint> waypoints = new List<Waypoint>();

    private void Start() {
        foreach (var waypoint in waypoints) {
            waypoint.Deactivate();
        }

        Waypoint?.Activate();
    }

    public Waypoint Waypoint {
        get {
            if (activeWaypoint < waypoints.Count)
                return waypoints[activeWaypoint];

            return null;
        }
    }

    public float Thrust => 2.5f;

    public void NextWaypoint()
    {
        var currentWaypoint = Waypoint;
        if (currentWaypoint == null) {
            Debug.LogWarning($"Tried to move to next waypoint but current one is null, shouldn't be possible. Index: {activeWaypoint}");
            return;
        }

        Scorekeeper.Instance.ReachWaypoint();

        currentWaypoint.Deactivate();

        activeWaypoint++;
        
        Waypoint?.Activate();
    }

    public void AddWaypoint(Vector2 pos) {
        GameManager.Instance.startPanel.SetActive(false);

        GameObject newWaypointGO = Instantiate(waypointPrefab, transform);
        newWaypointGO.transform.position = pos;
        var newWaypoint = newWaypointGO.GetComponent<Waypoint>();

        newWaypoint.Deactivate();
        waypoints.Add(newWaypoint);

        if (Waypoint?.gameObject?.activeSelf == false)
            Waypoint.Activate();
    }
}
