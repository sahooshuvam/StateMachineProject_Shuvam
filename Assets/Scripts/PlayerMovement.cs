using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float playerSpeed = 12f;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("isIdle");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetTrigger("isIdle");
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetTrigger("isRunning");
            PlayerRun(xInput, zInput);
        }
    }

    private void PlayerRun(float xInput, float zInput)
    {
        Vector3 move = transform.right * xInput + transform.forward * zInput;
        controller.Move(move * playerSpeed * Time.deltaTime);

    }
}
