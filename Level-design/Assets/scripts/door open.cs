using UnityEngine;

public class DoorUpDown : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0);
    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * speed);
        }

        // Check if door reached target
        if (Vector3.Distance(transform.position, isOpen ? openPosition : closedPosition) < 0.01f)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    public void ToggleDoor()
    {
        if (isMoving) return;

        isOpen = !isOpen;
    }

    public void OpenDoor()
    {
        if (isMoving) return;

        isOpen = true;
    }

    public void CloseDoor()
    {
        if (isMoving) return;

        isOpen = false;
    }
}