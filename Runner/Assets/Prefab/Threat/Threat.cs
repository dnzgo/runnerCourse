using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threat : MonoBehaviour
{
    [SerializeField] float _spawnInterval = 2f;
    [SerializeField] MovementComponent movementComponent;

    public float spawnInterval
    {
        get { return _spawnInterval; }
    }

    public MovementComponent GetMovementComponent() { return movementComponent; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
