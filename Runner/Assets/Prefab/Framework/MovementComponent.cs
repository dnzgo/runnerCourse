using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    float moveSpeed = 20.0f;

    Vector3 moveDirection = Vector3.forward;
    Vector3 destination;

    public void SetMoveDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public void SetMoveSpeed (float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        if (Vector3.Dot(destination - transform.position, moveDirection) < 0)
        {
            Destroy(gameObject);
        }
    }

}
