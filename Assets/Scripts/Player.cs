using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TileManager tileManager;
    private Vector3 direction = Vector3.zero;
    private Vector3 nextDirection = Vector3.zero;

    public float speed = 5;

    private int x = 0;
    private int z = 0;

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

    private void HandleStrictPosition()
    {
        var changeDirection = false;
        if (direction == Vector3.forward && transform.position.z > z)
        {
            z++;
            changeDirection = true;
        }
        else if (direction == Vector3.back && transform.position.z < z)
        {
            z--;
            changeDirection = true;
        }
        else if (direction == Vector3.left && transform.position.x < x)
        {
            x--;
            changeDirection = true;
        }
        else if (direction == Vector3.right && transform.position.x > x)
        {
            x++;
            changeDirection = true;
        }
        if (direction == Vector3.zero || changeDirection)
        {
            if (transform.position.z > 0 && nextDirection == Vector3.back ||
                transform.position.x > 0 && nextDirection == Vector3.left ||
                transform.position.z < tileManager.height - 1 && nextDirection == Vector3.forward ||
                transform.position.x < tileManager.width - 1 && nextDirection == Vector3.right)
            {
                direction = nextDirection;
            }
            else
            {
                direction = Vector3.zero;
            }
        }
    }
    void FixedUpdate()
    {
        HandleInput();
        transform.Translate(direction * Time.deltaTime * speed);
        HandleStrictPosition();
    }
}
