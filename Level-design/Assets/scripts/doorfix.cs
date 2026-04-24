using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Settings")]
    public string playerTag = "Player";


    private int playersInRange = 0;

    void Start()
    {

        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }


        playersInRange = 0;
        animator.SetBool("character_nearby", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playersInRange++;

            Debug.Log("Player ENTERED. Count: " + playersInRange);

            UpdateDoorState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playersInRange--;

            if (playersInRange < 0)
                playersInRange = 0;

            Debug.Log("Player EXITED. Count: " + playersInRange);

            UpdateDoorState();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {

            if (!animator.GetBool("character_nearby"))
            {
                Debug.Log("Player STILL inside - reopening door");
                animator.SetBool("character_nearby", true);
            }
        }
    }

    void UpdateDoorState()
    {
        bool isNearby = playersInRange > 0;

        Debug.Log("Door state (character_nearby): " + isNearby);

        animator.SetBool("character_nearby", isNearby);
    }
}