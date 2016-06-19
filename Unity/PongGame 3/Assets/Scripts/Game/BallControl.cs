using UnityEngine;
using System.Collections;

public class BallControl : MonoBehaviour {

    // TODO
    //   1) Faire bouger la balle du côté de la personne qui a perdu le points (ou random si commencement).
    //   2) Faire bouger la balle de plus en plus vite

    #region Données membres

    private bool _isRestartBall = false;
    private Rigidbody2D _rigidBody2D;
    private float _vitesse;

    #endregion // Données membres

    #region Accesseurs

    public float Vitesse
    {
        get { return _vitesse; }
    }

    #endregion // Accesseurs

    #region Events

    void Start()
    {
        _rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        _isRestartBall = true;

        _vitesse = 0;
    }

    void Update()
    {
        if (_isRestartBall)
        {
            _isRestartBall = false;

            AmorcerMouvement();
        }
    }

    #endregion // Events

    #region Méthodes publiques

    public void Restart()
    {
        _isRestartBall = true;
    }

    /// <summary>
    /// On reset la position de la ball
    /// </summary>
    /// //ObjPlayer -> Objet représentant la raquette du joueur venant de perdre la manche. On va donc restart la balle près de lui. null renvoit la balle à la position 0,0
    public void RestartPositionBall(int IndJoueur)
    {
        // On reset la vélocité à 0 pour que la balle ne bouge plus.
        Vector2 vel = _rigidBody2D.velocity;
        vel.x = 0;
        vel.y = 0;
        _rigidBody2D.velocity = vel;

        if (IndJoueur == -1)
        {
            // Position au centre (0, 0)
            gameObject.transform.position = new Vector2(0, -2);
        }        
        else
        {
            if (IndJoueur == 1)
            {
                // Joueur à gauche
                gameObject.transform.position = new Vector2(-28, -2);
            }
            else
            {
                // Joueur à droite
                gameObject.transform.position = new Vector2(28, -2);
            }
        }
    }

    #endregion // Méthodes publiques

    #region Méthodes privées

    /// <summary>
    /// On va initialiser le mouvement de la balle, mais après quelques secondes d'attentes.
    /// </summary>
    void AmorcerMouvement()
    {
        StartCoroutine(AmorcerMouvementEnDiffere());
    }

    IEnumerator AmorcerMouvementEnDiffere()
    {
        // On attends un nombre de secondes.
        yield return new WaitForSeconds(GameManager.DelaiAvantMouvementBalle);
        AmorcerMouvementBalle();
    }

    /// <summary>
    /// 
    /// </summary>
    void AmorcerMouvementBalle()
    {
        // On détermine le facteur de multiplication (1 ou -1), pour déterminer uniquement la direction de la balle.
        int facteurPositionX = 0;
        int facteurPositionY = 0;
        switch (GameManager.DernierJoueurAyantScore)
        {
            // On va le définir au hasard.
            case 0:
                facteurPositionX = (Random.Range(0.0f, 100.0f) > 50.0f ? -1 : 1); break;
            case 1:  // Le joueur 1 a fait le dernier point. On envoi la balle au joueur 2.
                facteurPositionX = -1; break;
            case 2:  // Le joueur 2 a fait le dernier point. On envoi la balle au joueur 1.
                facteurPositionX = 1; break;
        }
        facteurPositionY = Random.Range(0.0f, 100.0f) > 50.0f ? -1 : 1;


        // On détermine la force à appliquer à la balle.
        //_vitesseInitiale = new Vector2((facteurPositionX * GameManager.VitesseBalleX) + (Random.Range(-3.0f, 3.0f)),
        //    (facteurPositionY * GameManager.VitesseBalleY) + (Random.Range(-1.0f, 1.0f)));
        _vitesse = GameManager.VitesseBalle;

        // On  applique le côté obscur! ;)
        //_rigidBody2D.velocity = _vitesseInitiale;
        _rigidBody2D.velocity = new Vector2(facteurPositionX, facteurPositionY) * _vitesse;
    }

    /// <summary>
    /// On ajuste la vélocité.
    /// </summary>
    void OnCollisionEnter2D(Collision2D Coll)
    {
        if (Coll.collider.CompareTag("Player"))
        {
            // On augmente la vitesse!
            _vitesse += GameManager.VitesseAjoutParColision;

            bool isRaquetteGauche = Coll.gameObject.transform.position.x < 0;

            // On détermine la facteur de collision (où la balle à frapper la raquette) -> entre -1 et 1.
            float y = hitFactor(transform.position, Coll.transform.position, Coll.collider.bounds.size.y);

            // On calcul la direction de la balle.
            Vector2 dir = new Vector2((isRaquetteGauche ? 1 : -1), y);


            // On applique la vélocité.
            Vector2 nouvVitesse;
            nouvVitesse = dir * _vitesse;
            GetComponent<Rigidbody2D>().velocity = nouvVitesse;
        }
    }

    /// <summary>
    /// Permet de savoir où se situe la balle par rapport au palet.
    /// </summary>
    float hitFactor(Vector2 PosBalle, Vector2 PosRaquette, float HauteurRaquette)
    {
        //  1 <- La balle se trouve au haut du palet
        //
        //  0 <- La balle se trouve au centre
        //
        // -1 <- La balle se trouve au bas du palet
        return (PosBalle.y - PosRaquette.y) / HauteurRaquette;
    }

    /// <summary>
    /// La partie est terminé. On va placer la balle le plus loin possible.
    /// </summary>
    void CacherBalle()
    {
        RestartPositionBall(-1);

        // On met la balle le plus loin possible.
        gameObject.transform.position = new Vector2(500, 500);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    #endregion
}
