/*using UnityEngine;

public class MovementButtons : MonoBehaviour
{
    public PlayerManager playerManager;

    private bool moveUp;
    private bool moveDown;
    private bool moveLeft;
    private bool moveRight;

    private void Update()
    {
        if (moveUp) playerManager.Move(Vector2.up);
        if (moveDown) playerManager.Move(Vector2.down);
        if (moveLeft) playerManager.Move(Vector2.left);
        if (moveRight) playerManager.Move(Vector2.right);
    }

    public void OnMoveUpPressed() => moveUp = true;
    public void OnMoveUpReleased() => moveUp = false;

    public void OnMoveDownPressed() => moveDown = true;
    public void OnMoveDownReleased() => moveDown = false;

    public void OnMoveLeftPressed() => moveLeft = true;
    public void OnMoveLeftReleased() => moveLeft = false;

    public void OnMoveRightPressed() => moveRight = true;
    public void OnMoveRightReleased() => moveRight = false;
}

*/