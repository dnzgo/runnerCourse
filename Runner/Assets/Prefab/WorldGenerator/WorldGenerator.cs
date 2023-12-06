using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] float environmentMoveSpeed;
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform EndPoint;

    [SerializeField] GameObject[] roadBlocks;
    Vector3 moveDirection;

    private void Start()
    {
        Vector3 nextBlockPosition = StartPoint.position;
        float endPointDistance = Vector3.Distance(StartPoint.position, EndPoint.position);
        moveDirection = (EndPoint.position - StartPoint.position).normalized;

        while (Vector3.Distance(StartPoint.position, nextBlockPosition) < endPointDistance)
        {
            GameObject newBlock = SpawnNewBlock(nextBlockPosition, moveDirection);   
            float blockLenght = newBlock.GetComponentInChildren<Renderer>().bounds.size.z;
            
            nextBlockPosition += moveDirection * blockLenght;
        }
    }

    private GameObject SpawnNewBlock(Vector3 spawnPosition, Vector3 moveDirection)
    {
        int pick = Random.Range(0, roadBlocks.Length);
        GameObject pickedBlock = roadBlocks[pick];
        GameObject newBlock = Instantiate(pickedBlock);
        newBlock.transform.position = spawnPosition;
        MovementComponent movementComponent = newBlock.GetComponent<MovementComponent>();
        if (movementComponent != null)
        {
            movementComponent.SetMoveSpeed(environmentMoveSpeed);
            movementComponent.SetMoveDirection(moveDirection);
            movementComponent.SetDestination(EndPoint.position);

        }

        return newBlock;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null)
        {
            GameObject newBlock = SpawnNewBlock(other.transform.position, moveDirection);
            float newBlockHalfWidth = newBlock.GetComponent<Renderer>().bounds.size.z / 2f;
            float previousHalfBlockWidth = other.GetComponent<Renderer>().bounds.size.z / 2f;
            Vector3 newBlockSpawnOffset = -(newBlockHalfWidth + previousHalfBlockWidth) * moveDirection;
            newBlock.transform.position += newBlockSpawnOffset;
        }
    }
}
