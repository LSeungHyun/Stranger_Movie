using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PositionTracker : MonoBehaviour
{
    public ObjectMover objectMoverA; 
    public ObjectMover objectMoverB; 

    private void Start()
    {
        objectMoverA.lastPosition = new Vector3(1452, -888, transform.position.z);
        objectMoverB.lastPosition = new Vector3(1452, -888, transform.position.z);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectMoverA.lastPosition = transform.position;
            objectMoverB.lastPosition = transform.position;
        }
    }
}
