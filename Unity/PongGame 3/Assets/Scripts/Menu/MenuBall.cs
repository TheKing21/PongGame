using UnityEngine;
using System.Collections;

public class MenuBall : MonoBehaviour {

    public static int BallVelocity = 40;

    void Awake()
    {
        float posX = Random.Range(BallVelocity / 2, BallVelocity);
        float posY = Random.Range(BallVelocity / 2, BallVelocity);
        if (Random.Range(1, 50) > 30)
            posX *= -1;

        Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(posX, posY);
    }
}
