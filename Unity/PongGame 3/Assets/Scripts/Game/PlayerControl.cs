using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    private string _strControlAxes;
    private int _nbJoueurs;
    private bool _isPlayerNo1;

    private GameObject _objBall;

	void Start () {
        _nbJoueurs = (GameManager.ConfigNbJoueurs == enmConfigNbJoueurs._1Joueur ? 1 : 2);
        _isPlayerNo1 = gameObject.transform.position.x < 0;

        _strControlAxes = (_isPlayerNo1 ? "Vertical1" : (_nbJoueurs > 1 ? "Vertical2" : ""));
        _objBall = GameObject.FindGameObjectWithTag("Ball");
    }
	
	// Update is called once per frame
	void Update () {
        if (!_isPlayerNo1 && _nbJoueurs == 1)
        {
            // C'est l'ordinateur...
            if (_objBall != null)
            {
                Vector3 posActuel = gameObject.transform.position;
                Vector3 posBalle = _objBall.transform.position;

                Vector3 posCible = Vector3.Lerp(posActuel, posBalle, Time.deltaTime * ObtenirVitessePalet());

                // On attribut la nouvelle position
                float sizeY = gameObject.GetComponent<BoxCollider2D>().size.y;
                float posYMin = GameManager.PosYMin;
                float posYMax = GameManager.PosYMAX;
                gameObject.transform.position = new Vector3(posActuel.x, Mathf.Clamp(posCible.y, posYMin, posYMax), posActuel.z);
            }
        }
        else
        {
            // C'est un humain qui contrôle...
            Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D>();

            // On applique le mouvement à la palette et on s'assure qu'elle ne bouge pas en X.
            Vector2 vel = rigidBody2D.velocity;
            vel.x = 0;
            vel.y = Input.GetAxis(_strControlAxes) * ObtenirVitessePalet();
            rigidBody2D.velocity = vel;
        }        
    }

    /// <summary>
    /// La vitesse des joueurs sont déterminé par la difficulté.
    /// </summary>
    /// <returns></returns>
    private float ObtenirVitessePalet()
    {
        return (_isPlayerNo1 ? GameManager.VitessePaletJoueur1 : GameManager.VitessePaletJoueur2);
    }
}
