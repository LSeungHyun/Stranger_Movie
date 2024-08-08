using UnityEngine;

public class BlueEyeEnd : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (objectToActivate != null)
            {
                FollowObject followObject = objectToActivate.GetComponent<FollowObject>();
                if (followObject != null)
                {
                    followObject.StopMoving(true);
                }
            }
        }
    }
}
