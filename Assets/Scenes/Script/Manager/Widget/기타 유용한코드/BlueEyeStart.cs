using UnityEngine;

public class BlueEyeStart : MonoBehaviour
{
    public GameObject objectToActivate;
    public Vector3 activationPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
            if (objectToActivate != null)
            {
                FollowObject followObject = objectToActivate.GetComponent<FollowObject>();
                if (followObject)
                {
                    followObject.StopMoving(false);
                }
            }

            objectToActivate.transform.position = activationPosition;
        }
    }
}
