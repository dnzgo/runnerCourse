using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform EndPoint;

    [SerializeField] GameObject[] roadBlocks;

    private void Start()
    {
        Vector3 nextBlockPosition = StartPoint.position;
        float endPointDistance = Vector3.Distance(StartPoint.position, EndPoint.position);
        while (Vector3.Distance(StartPoint.position, nextBlockPosition) < endPointDistance)
        {
            int pick = Random.Range(0, roadBlocks.Length);
            GameObject pickedBlock = roadBlocks[pick];
            GameObject newBlock = Instantiate(pickedBlock);
            newBlock.transform.position = nextBlockPosition;
            float blockLenght = newBlock.GetComponentInChildren<Renderer>().bounds.size.z;
            nextBlockPosition -= new Vector3(0, 0, blockLenght);
        }
    }
}
