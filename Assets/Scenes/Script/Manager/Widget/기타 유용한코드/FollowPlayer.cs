using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public float[] speedLevels = new float[5] { 2f, 4f, 6f, 8f, 10f }; 
    public int speedLevelIndex = 0;
    private float currentSpeed;
    private Animator animator;
    public bool stopMove = false; 
    private Transform playerTransform; 

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Start()
    {
        currentSpeed = speedLevels[speedLevelIndex];
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        currentSpeed = speedLevels[speedLevelIndex];

        if (playerTransform != null && !stopMove)
        {
            Vector3 direction = playerTransform.position - transform.position;
            float distance = direction.magnitude;
            direction.Normalize();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Wall"));

            if (hit.collider != null)
            {
                Vector3 avoidDirection = Vector3.Cross(direction, Vector3.forward);
                transform.position += avoidDirection * currentSpeed * Time.deltaTime;
            }
            else
            {
                if (distance > 0.1f) 
                {
                    animator.SetBool("Walking", true);

                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        if (direction.x > 0)
                        {
                            animator.SetFloat("DirX", 1); 
                            animator.SetFloat("DirY", 0);
                        }
                        else
                        {
                            animator.SetFloat("DirX", -1);
                            animator.SetFloat("DirY", 0);
                        }
                    }
                    else
                    {
                        if (direction.y > 0)
                        {
                            animator.SetFloat("DirX", 0);
                            animator.SetFloat("DirY", 1); 
                        }
                        else
                        {
                            animator.SetFloat("DirX", 0);
                            animator.SetFloat("DirY", -1);
                        }
                    }

                    transform.position += direction * currentSpeed * Time.deltaTime;
                }
                else
                {
                    animator.SetBool("Walking", false);
                }
            }
        }
        else
        {
            animator.SetBool("Walking", false); 
        }
    }

    public void StopMoving(bool stop)
    {
        stopMove = stop;
    }
}
