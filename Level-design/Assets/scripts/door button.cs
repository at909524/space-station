using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public DoorUpDown door;
    public float interactDistance = 6f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(
                Camera.main.transform.position + Camera.main.transform.forward * 0.5f,
                Camera.main.transform.forward
            );

            RaycastHit hit;

            int layerMask = ~LayerMask.GetMask("Player");

            if (Physics.Raycast(ray, out hit, interactDistance, layerMask))
            {
                // Only trigger THIS button
                if (hit.collider.GetComponentInParent<DoorButton>() == this)
                {
                    if (door != null)
                    {
                        door.ToggleDoor();
                    }
                    else
                    {
                        Debug.LogWarning("Door not assigned to button!");
                    }
                }
            }
        }
    }
}