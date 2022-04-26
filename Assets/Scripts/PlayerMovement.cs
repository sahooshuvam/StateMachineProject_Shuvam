using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float playerSpeed = 12f;
    Animator animator;
    public Transform gunFirePoint;
    [SerializeField] private GameObject bloopPS;

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
            PlayerRun(xInput, zInput);
        }

        if (Input.GetMouseButtonDown(0))
        {
            CheckEnemyGotHit();
        }
    }

    private void CheckEnemyGotHit()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(gunFirePoint.position, gunFirePoint.forward, out hitInfo, 100f))
        {
            GameObject hitEnemy = hitInfo.collider.gameObject;
            if (hitEnemy.tag == "Enemy")
            {
                GetBloodParticle(hitInfo.point);
                hitEnemy.SetActive(false);
            }
        }
    }

    private void GetBloodParticle(Vector3 position)
    {
        Instantiate(bloopPS, position, Quaternion.identity);
        
    }

    private void PlayerRun(float xInput, float zInput)
    {
        Vector3 move = transform.right * xInput + transform.forward * zInput;
        controller.Move(move * playerSpeed * Time.deltaTime);

    }
}
