using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] float jumpHeight = 2.5f;
    [SerializeField] [Range(0, 1)] float groundCheckRadius = 0.2f;
    [SerializeField] Transform[] laneTransforms;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundCheckMask;

    PlayerInput playerInput;
    Vector3 destination;

    int currentLaneIndex;

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
        }
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        playerInput.gameplay.Move.performed += MovePerformed;
        playerInput.gameplay.Jump.performed += JumpPerformed;

        for (int i = 0; i < laneTransforms.Length; i++)
        {
            if (laneTransforms[i].position == transform.position)
            {
                currentLaneIndex = i;
                destination = laneTransforms[i].position;
            }
        }
    }

    private void Update()
    {
        float transformX = Mathf.Lerp(transform.position.x, destination.x, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transformX, transform.position.y, transform.position.z);
    }

    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float inputValue = obj.ReadValue<float>();
        Move(inputValue);


    }

    private void Move(float inputValue)
    {
        if (inputValue > 0)
        {
            if (currentLaneIndex == laneTransforms.Length - 1) { return; }

            currentLaneIndex++;
            destination = laneTransforms[currentLaneIndex].position;
        }
        else
        {
            if (currentLaneIndex == 0) { return; }

            currentLaneIndex--;
            destination = laneTransforms[currentLaneIndex].position;
        }
    }

    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!IsOnGround()) { return; }

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
            rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        }
    }

    private bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask);
    }


}
