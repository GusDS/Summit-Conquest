using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmall : MonoBehaviour
{
    // Enemy Small: fast, weak, pushable, heavy spawn. IA: follow player, doesn't avoid world edges
    public float speed = 4.5f; // last tuning speed 4 / mass 20
    private Control control;
    private Rigidbody enemyRb;
    private GameObject player;
    private Vector3 lookDirection;

    void Start()
    {
        control = GameObject.Find("Control").GetComponent<Control>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        if (control.isActionOn) {
            // normalized reduce the vector to a magnitude of 1, for better control
            lookDirection = (player.transform.position - transform.position).normalized;
            lookDirection.y = 0f; // prevents "moving up"
            enemyRb.AddForce(lookDirection * speed, ForceMode.Impulse);
        }
        else {
            // test return to origin
            lookDirection = (Vector3.zero - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed, ForceMode.Impulse);
        }

        if (transform.position.y < -5)
        {
            control.ScoreUpdate(Control.Scores.scoreEnemySmall);
            Destroy(gameObject);
        }
    }
}
