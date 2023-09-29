using UnityEngine;

public class Player : MonoBehaviour
{
    public TileManager tileManager;
    private Vector3 direction = Vector3.zero;
    private Vector3 nextDirection = Vector3.zero;

    public float speed = 5;

    private int currentX
    {
        get
        {
            return direction == Vector3.left ? Mathf.CeilToInt(transform.position.x) : Mathf.FloorToInt(transform.position.x);
        }
    }

    private int currentZ
    {
        get
        {
            return direction == Vector3.back ? Mathf.CeilToInt(transform.position.z) : Mathf.FloorToInt(transform.position.z);
        }
    }

    private void HandleInput()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        if (vertical < 0)
        {
            if (transform.position.z > 0)
            {
                nextDirection = Vector3.back;
            }
        }
        else if (vertical > 0)
        {
            if (transform.position.z < tileManager.height - 1)
            {
                nextDirection = Vector3.forward;
            }
        }
        else if (horizontal < 0)
        {
            if (transform.position.x > 0)
            {
                nextDirection = Vector3.left;
            }
        }
        else if (horizontal > 0)
        {
            if (transform.position.x < tileManager.width - 1)
            {
                nextDirection = Vector3.right;
            }
        }
    }

    private void HandleMovement()
    {
        var posX = currentX;
        var posZ = currentZ;
        transform.Translate(direction * Time.deltaTime * speed);
        var pos = transform.position;
        if(posX != currentX || posZ != currentZ || direction == Vector3.zero)
        {
            pos.x = currentX;
            pos.z = currentZ;
            if (direction != nextDirection)
            {
                direction = nextDirection;
            }
        }
        transform.position = pos;
    }

    private void HandleStrictPosition()
    {
        tileManager.ChangeTileState(currentX, currentZ, TileState.Trajectory);
        var pos = transform.position;

        if (transform.position.x < 0)
        {
            pos.x = 0;
        }

        if (transform.position.x > tileManager.width - 1)
        {
            pos.x = tileManager.width - 1;
        }
        if (transform.position.z < 0)
        {
            pos.z = 0;
        }
        if (transform.position.z > tileManager.height - 1)
        {
            pos.z =  tileManager.height - 1;
        }
        transform.position = pos;
    }
    void FixedUpdate()
    {
        HandleInput();
        HandleMovement();
        HandleStrictPosition();
    }
}
