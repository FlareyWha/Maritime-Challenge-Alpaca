using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPuck : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Vector2 velocity = Vector2.zero;
    private Vector2 accel = Vector2.zero;
    private const float hitForce = 0.7f;
    private const float frictionConst = 0.05f;
    private const float  inelasticWallConst = 0.1f;
    private const float  inelasticPaddleConst = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnPaddleHitApplyForce(TestPaddle paddle)
    {
        Vector2 u1 = paddle.GetVelocity();
        Vector2 u2 = velocity; // or my own see how i try rb forces firs

        Vector2 N = (paddle.transform.position - transform.position).normalized;
        accel = -2 * hitForce * Vector2.Dot(u2 - u1, N) * N;
        velocity += accel;
        velocity *= (1.0f - inelasticPaddleConst);
        //Debug.Log("==Paddle Hit Puck!== \nAccel: " + accel + "\nNew Velocity is" + velocity);
    }

    private void FixedUpdate()
    {

        // Stop Puck if coming to a stop  (curr dir diff from last accel dir) - so it doesn't go backwards from deccel
        if (velocity.magnitude != 0 &&
            Vector2.Dot(accel, velocity) < 0)
        {
            velocity = Vector2.zero;
        }

        // Apply Decceleration if ship is moving
        if (velocity.magnitude != 0)
            velocity -= velocity.normalized * frictionConst * Time.deltaTime;

        // Clamp values wihtin Max Limit
        //velocity.x = Mathf.Clamp(velocity.x, -MAX_VEL, MAX_VEL);
        //velocity.y = Mathf.Clamp(velocity.y, -MAX_VEL, MAX_VEL);

       
        rb.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            TestWall wall = collision.gameObject.GetComponent<TestWall>();
            accel = -2 * Vector2.Dot(velocity, wall.Normal) * (wall.Normal);
            velocity += accel;
            velocity *= (1.0f - inelasticWallConst);
        }
        else if (collision.isTrigger && collision.gameObject.CompareTag("AirHockeyPaddle"))
        {
            OnPaddleHitApplyForce(collision.gameObject.GetComponent<TestPaddle>());
        }
    }
}
