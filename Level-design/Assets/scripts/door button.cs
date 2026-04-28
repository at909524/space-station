using UnityEngine;
using System.Collections;

public class DoorButton : MonoBehaviour
{
    public DoorUpDown[] doors;
    public float interactDistance = 6f;
    public ElevatorScript elevator;


    public bool controlsElevator = false;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        Ray ray = new Ray(
            Camera.main.transform.position + Camera.main.transform.forward * 0.5f,
            Camera.main.transform.forward
        );

        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, interactDistance, layerMask))
        {
            if (hit.collider.GetComponentInParent<DoorButton>() == this)
            {
                HandleInteraction();
            }
        }
    }

    void HandleInteraction()
    {
        bool anyDoorOpen = false;

        foreach (DoorUpDown d in doors)
        {
            if (d != null && d.IsOpen())
            {
                anyDoorOpen = true;
                break;
            }
        }

        // 🚪 Toggle doors
        foreach (DoorUpDown d in doors)
        {
            if (d == null) continue;

            if (anyDoorOpen)
                d.CloseDoor();
            else
                d.OpenDoor();
        }

        if (controlsElevator && elevator != null)
        {
            // Only trigger elevator if THIS press was meant as inside button action
            if (anyDoorOpen)
            {

                StartCoroutine(TriggerElevatorAfterDoorsClose());
            }
        }
    }

    IEnumerator TriggerElevatorAfterDoorsClose()
    {
        // wait 1 frame so door state updates from Lerp
        yield return null;

        // wait until ALL doors are fully closed
        yield return new WaitUntil(() =>
        {
            foreach (DoorUpDown d in doors)
            {
                if (d != null && d.IsOpen())
                    return false;
            }
            return true;
        });

        // now safely move elevator
        elevator.CloseDoorsAndMove();
    }
}