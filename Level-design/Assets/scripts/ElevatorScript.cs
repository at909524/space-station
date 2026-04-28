using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour
{
    [Header("Movement")]
    public Transform topPoint;
    public Transform bottomPoint;
    public float speed = 2f;

    [Header("Doors")]
    public DoorUpDown[] doors;

    private bool isMoving = false;
    private bool isInUse = false;
    private bool goingUp = false;

    void Update()
    {
        if (!isMoving) return;

        Transform target = goingUp ? topPoint : bottomPoint;

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            target.position.y,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
            isInUse = false;
        }
    }

    // Called by DoorButton AFTER doors are confirmed closed
    public void CloseDoorsAndMove()
    {
        if (isMoving || isInUse)
        {
            Debug.Log("Elevator blocked by state");
            return;
        }

        StartCoroutine(ElevatorSequence());
    }

    IEnumerator ElevatorSequence()
    {
        Debug.Log("Elevator sequence started");

        isInUse = true;
        isMoving = false;

        // 🚪 Force doors closed (safety layer)
        foreach (DoorUpDown d in doors)
        {
            if (d != null)
                d.CloseDoor();
        }

        // ⏳ Wait until all doors are fully closed
        yield return new WaitUntil(() =>
        {
            foreach (DoorUpDown d in doors)
            {
                if (d != null && !d.IsFullyClosed())
                    return false;
            }
            return true;
        });

        // 🧭 Decide direction (safe + reliable)
        Transform target;

        float distToTop = Vector3.Distance(transform.position, topPoint.position);
        float distToBottom = Vector3.Distance(transform.position, bottomPoint.position);

        if (distToTop < distToBottom)
        {
            target = bottomPoint;
            goingUp = false;
        }
        else
        {
            target = topPoint;
            goingUp = true;
        }

        isMoving = true;

        // 🚀 Move elevator
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, target.position.y, transform.position.z),
                speed * Time.deltaTime
            );

            yield return null;
        }

        isMoving = false;
        isInUse = false;

        Debug.Log("Elevator sequence finished");
    }

    // Player sticks to elevator
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}