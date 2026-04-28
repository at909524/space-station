using UnityEngine;

public class DoorUpDown : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(3, 0, 0);
    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    public bool IsFullyClosed()
    {
        return Vector3.Distance(transform.localPosition, closedPosition) < 0.01f;
    }

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        Vector3 target = isOpen ? openPosition : closedPosition;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            target,
            Time.deltaTime * speed
        );
    }

    // ✅ REQUIRED BY ELEVATOR SCRIPT
    public void OpenDoor()
    {
        isOpen = true;
    }

    // ✅ REQUIRED BY ELEVATOR SCRIPT
    public void CloseDoor()
    {
        isOpen = false;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}