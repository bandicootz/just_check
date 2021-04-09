using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    private Rigidbody ballRb;
    Vector3 lastVelocity;
    public Vector3 startPos;
    private GameManager gameManager;

    bool bounce = false;

    void Awake()
    {
        ballRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        StartCoroutine(waitForDestroy());

        Debug.Log(gameManager.speedThrow);
        ballRb.AddRelativeForce(Vector3.back * gameManager.speedThrow);
    }

    IEnumerator waitForDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        gameManager.canThrowBall = true;
    }

    void FixedUpdate()
    {
        lastVelocity = ballRb.velocity;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Border"))
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            ballRb.velocity = direction * speed;
            if (!bounce)
            {
                bounce = true;
            }

        }

        if (col.gameObject.CompareTag("EnemyBlock"))
        {
            col.gameObject.SetActive(false);
            gameManager.blocksDestroyed++;
            Destroy(gameObject);
            gameManager.canThrowBall = true;
        }

        if (col.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            gameManager.canThrowBall = true;
        }
    }
}