﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{
    /// <summary>
    /// "Constant" for checking if ghost has reached destination. </summary>
    [SerializeField] private float DestinationDistanceCheck = 0.5f;
   
    /// <summary>
    /// Parent Gameobject to mark intersections of maze.</summary>
    [SerializeField] private GameObject NavigationMarkers;

    [SerializeField] private string WaypointTag = "Waypoint";

    /// <summary>
    /// List of all the waypoints to visit.</summary>
    private List<Transform> Waypoints;

    /// <summary>
    /// Iterator for waypoints list.</summary>
    private IEnumerator<Transform> WaypointsEnum;

    private NavMeshAgent agent;

    private void Start()
    {
        //Get list of waypoints.
        Waypoints = NavigationMarkers.GetComponentsInChildren<Transform>().Where(child => child.CompareTag(WaypointTag)).ToList();
        WaypointsEnum = Waypoints.GetEnumerator();

        //set agent destination to first waypoint.
        agent = GetComponent<NavMeshAgent>();
        WaypointsEnum.MoveNext();
        agent.SetDestination(WaypointsEnum.Current.position);
    }

    private void Update()
    {
        //check if nav agent as arrived at current waypoint.
        if (AtDestination())
        {
            if (!WaypointsEnum.MoveNext())
            {
                WaypointsEnum = Waypoints.GetEnumerator();
                WaypointsEnum.MoveNext();
            }
            agent.SetDestination(WaypointsEnum.Current.position);
        }
    }

    //Returns true if the agent has reached it's destination.
    private bool AtDestination()
    {
        return (Vector3.Distance(transform.position, agent.destination) < DestinationDistanceCheck);

        //copied from the internet.  maybe tinker to get to work?
        //return agent.pathPending && 
        //       agent.remainingDistance <= agent.stoppingDistance &&
        //       (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}
