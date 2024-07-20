using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRobot : MonoBehaviour
{
    [SerializeField] EnemyAI robot;
    [SerializeField] float walkSpeed, destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        robot.animator.SetBool("isWalking", true);
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }
}
