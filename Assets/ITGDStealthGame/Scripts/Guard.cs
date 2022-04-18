 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] Transform pathHolder;
    [SerializeField] float waitTime, speed, turnSpeed, viewDistance, timeToSpotPlayer = .5f;
    [SerializeField] Light spotligth;
    float viewAngle, playerVisibleTime;
    [SerializeField] LayerMask viewMask;
    [SerializeField] Transform player;
    Color originalSpotlightColor;

    public static event Action OnGuardHasSpottedPlayer;
    private void Awake()
    {
        viewAngle = spotligth.spotAngle;
        originalSpotlightColor = spotligth.color;
    }
    private void Start()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(Patrolling(waypoints));
    }
    private void Update()
    {
        if (CanSeePlayer())
        {
            playerVisibleTime += Time.deltaTime;
        }
        else
        {
            playerVisibleTime -= Time.deltaTime;
        }

        playerVisibleTime = Mathf.Clamp(playerVisibleTime, 0, timeToSpotPlayer);
        spotligth.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTime / timeToSpotPlayer);

        if (playerVisibleTime >= timeToSpotPlayer)
        {
            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }
    }
    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
    IEnumerator Patrolling(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int indexTargetWaypoint = 1;
        Vector3 targetWaypoint = waypoints[indexTargetWaypoint];
        transform.LookAt(targetWaypoint);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[indexTargetWaypoint], speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                indexTargetWaypoint = (indexTargetWaypoint + 1) % waypoints.Length;
                targetWaypoint = waypoints[indexTargetWaypoint];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnFace(targetWaypoint));
            }
            yield return null;
        }
    }
    IEnumerator TurnFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > Mathf.Epsilon)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 currentPosition = pathHolder.GetChild(0).position;

        Vector3 previousPosition = currentPosition;
        foreach (Transform waypoints in pathHolder)
        {
            Gizmos.DrawSphere(waypoints.position, .3f);//sphere ekliyoru oyunda gorunmeyen
            Gizmos.DrawLine(previousPosition, waypoints.position);//cizgi ekliyor oyunda gorunmeyen
            previousPosition = waypoints.position;
        }
        Gizmos.DrawLine(previousPosition, currentPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}