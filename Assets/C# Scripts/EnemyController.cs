using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;

    public float speed;
    public static int enemiesDestroyed = 0; // shared across all instances

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player_Sphere");
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 lookDirection = (targetPosition - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        if (transform.position.y <= -8)
        {
            Destroy(gameObject);
            enemiesDestroyed++;
            Debug.Log("Enemies Destroyed: " + enemiesDestroyed);
        }
    }
}
