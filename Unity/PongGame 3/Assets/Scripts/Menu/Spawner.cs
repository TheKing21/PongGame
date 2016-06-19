using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //CreerBalle();   // Il y en a déjà une au départ...
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CreerBalle();
        }
    }

    void CreerBalle()
    {
        // On a cliquer, on va ajouter une balle :)
        GameObject objBall = GameObject.FindGameObjectWithTag("Ball");
        GameObject nouvBalle = (Instantiate(objBall, new Vector3(0, 0, 0), Quaternion.identity) as GameObject);

        float velocity = MenuBall.BallVelocity;

        float posX = Random.Range(velocity / 2, velocity);
        float posY = Random.Range(velocity / 2, velocity);
        if (Random.Range(1, 50) > 30)
            posX *= -1;

        Rigidbody2D rigidBody = nouvBalle.gameObject.GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(posX, posY);
    }
}
