using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Road Blocks")]
    [SerializeField] float environmentMoveSpeed;
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform EndPoint;
    [SerializeField] GameObject[] roadBlocks;

    [Header("Buildings")]
    [SerializeField] GameObject[] buildings;
    [SerializeField] Transform[] buildingSpawnPoints;
    [SerializeField] Vector2 buildingSpawnScaleRange = new Vector2(0.65f, 0.90f);

    [Header("Street Light")]
    [SerializeField] GameObject streetLight;
    [SerializeField] Transform[] lightSpawnPoints;

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

        SpawnBuildings(newBlock);
        SpawnStreetLights(newBlock);

        return newBlock;
    }

    private void SpawnStreetLights(GameObject parentBlock)
    {
        foreach (Transform spawnPoint in lightSpawnPoints)
        {
            Vector3 spawnLocation = parentBlock.transform.position + (spawnPoint.position - StartPoint.position);
            // Quaternion spawnRotation = Quaternion.LookRotation((StartPoint.position - spawnPoint.position).normalized, Vector3.up);
            Quaternion spawnRotationOffset = Quaternion.Euler(0, 0, 0);
            if ((StartPoint.position.x - spawnPoint.position.x) < 0f)
            {
                spawnRotationOffset = Quaternion.Euler(0, 180, 0);
            }
            GameObject newStreetLight = Instantiate(streetLight, spawnLocation, transform.rotation * spawnRotationOffset, parentBlock.transform);
        }
    }

    private void SpawnBuildings(GameObject parentBlock)
    {
        foreach (Transform buildingSpawnPoint in buildingSpawnPoints)
        {
            Vector3 buildingSpawnLocation = parentBlock.transform.position + (buildingSpawnPoint.position - StartPoint.position);
            int locationOffsetBy90 = Random.Range(0, 3);
            Quaternion buildingSpawnRotation = Quaternion.Euler(0.0f, locationOffsetBy90 * 90, 0.0f);
            Vector3 buildingSpawnSize = Vector3.one * Random.Range(buildingSpawnScaleRange.x, buildingSpawnScaleRange.y);

            int buildingPick = Random.Range(0, buildings.Length);

            GameObject newBuilding = Instantiate(buildings[buildingPick], buildingSpawnLocation, buildingSpawnRotation, parentBlock.transform);
            newBuilding.transform.localScale = buildingSpawnSize;
        }
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
