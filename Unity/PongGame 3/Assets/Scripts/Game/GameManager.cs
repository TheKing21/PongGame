using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum enmConfigNbJoueurs
{
    _1Joueur = 0,
    _2Joueurs = 1
}

public class GameManager : MonoBehaviour {

    public static enmConfigNbJoueurs ConfigNbJoueurs = enmConfigNbJoueurs._1Joueur;

    public static int ScorePlayer1;
    public static int ScorePlayer2;

    public GameObject ObjTxtScorePlayer1;
    public GameObject ObjTxtScorePlayer2;
    public GameObject ObjTxtWinner;
    public GameObject ObjTxtDebug;

    public static float VitesseBalle = 20.0f;
    public static float VitessePaletJoueur1 = 40.0f;
    public static float VitessePaletJoueur2 = 40.0f;

    public static float PosYMin = -15.5f;
    public static float PosYMAX = 11.6f;

    private static Transform _objBall;
    private static float _delaiAvantMouvementBalle = 2.0f;      // Délai avant lequel qu'on amorçe le mouvement de la balle après un but ou après le reset.
    private static int _dernierJoueurAyantScore = 0;            // On garde en mémoire le dernier joueur ayant compter un but. 0: Aucun, 1: Joueur1, 2: Joueur2.
    private static int _difficulte = -1;                        // Difficulté: 0 - Aucune (1 vs 1), 1 = facile, 2 = moyen, 3 = difficile

    private static int _nbCollisionBalleCourant = 0;            // Nombre de fois que la balle est entré en collision avec un palais dans la manche courante.
    private static float _vitesseAjoutParColision = 1;

    private Text _objTxtScorePlayer1;
    private Text _objTxtScorePlayer2;
    private Text _objTxtWinner;
    private Text _objTxtDebug;

    private int _nbPtsPourGagner = 10;

    public static bool IsDebug = false;

    // Il y a eu un but ou reset de la game. On reset le nombre de collision.
    public static void ResetCollisionBalle()
    {
        _nbCollisionBalleCourant = 0;
    }

    // La balle a touché à une des palettes. On augmente le nombre de collision.
    public static void AugmenterCollisionBalle()
    {
        _nbCollisionBalleCourant++;
    }

    public static int GetNbCollisionBalle()
    {
        return _nbCollisionBalleCourant;
    }

    public static float DelaiAvantMouvementBalle
    {
        get { return _delaiAvantMouvementBalle; }
    }

    public static int DernierJoueurAyantScore
    {
        get { return _dernierJoueurAyantScore; }
    }

    public static float VitesseAjoutParColision
    {
        get { return _vitesseAjoutParColision; }
    }

    public static int Difficulte
    {
        get { return _difficulte; }
        set
        {
            _difficulte = value;

            switch (value)
            {
                default:
                case 0:
                    {
                        // Pas de niveau de difficulté (2 joueurs)
                        _difficulte = 0;

                        VitesseBalle = 22.0f;

                        VitessePaletJoueur1 = 40.0f;
                        VitessePaletJoueur2 = 40.0f;

                        _vitesseAjoutParColision = 3.0f;
                    } break;
                case 1:
                    {
                        // Facile
                        VitesseBalle = 16.0f;

                        VitessePaletJoueur1 = 40.0f;
                        VitessePaletJoueur2 = 3.0f;

                        _vitesseAjoutParColision = 0.2f;
                    } break;
                case 2:
                    {
                        // Moyen
                        VitesseBalle = 22.0f;

                        VitessePaletJoueur1 = 40.0f;
                        VitessePaletJoueur2 = 4.0f;

                        _vitesseAjoutParColision = 0.5f;
                    } break;
                case 3:
                    {
                        // Moyen
                        VitesseBalle = 23.0f;

                        VitessePaletJoueur1 = 40.0f;
                        VitessePaletJoueur2 = 7.0f;

                        _vitesseAjoutParColision = 1.0f;
                    } break;
            }
        }
    }

    void Start()
    {
        if (_difficulte == -1)
            Difficulte = 1;

        _objBall = GameObject.FindGameObjectWithTag("Ball").transform;

        _objTxtScorePlayer1 = ObjTxtScorePlayer1.GetComponent<Text>();
        _objTxtScorePlayer2 = ObjTxtScorePlayer2.GetComponent<Text>();
        _objTxtWinner = ObjTxtWinner.GetComponent<Text>();
        _objTxtDebug = ObjTxtDebug.GetComponent<Text>();

        ObjTxtWinner.gameObject.transform.position = new Vector3(0, -200, -3);
    }

    void Update()
    {       
        // On écris le score à l'écran.
        _objTxtScorePlayer1.text = ScorePlayer1.ToString();
        _objTxtScorePlayer2.text = ScorePlayer2.ToString();

        // On vérifie s'il faut terminé la partie.
        if (ScorePlayer1 == _nbPtsPourGagner || ScorePlayer2 == _nbPtsPourGagner)
        {
            TerminerPartie();
        }

        if (Input.GetKey(KeyCode.F3))
        {
            IsDebug = !IsDebug;
        }

        AfficherInformationsDebug();
    }

    public static void Score(string WallID)
    {
        if (WallID == "WallRight")
        {
            ScorePlayer1++;
            _dernierJoueurAyantScore = 1;
        }
        else
        {
            ScorePlayer2++;
            _dernierJoueurAyantScore = 2;
        }

        // On reset la position de la balle.
        Transform objBall = GameObject.FindGameObjectWithTag("Ball").transform;
        objBall.gameObject.SendMessage("RestartPositionBall", (_dernierJoueurAyantScore == 1 ? 2 : 1), SendMessageOptions.RequireReceiver);
        objBall.gameObject.SendMessage("Restart", null, SendMessageOptions.RequireReceiver);

        ResetCollisionBalle();
    }

    public void ResetGame()
    {
        ObjTxtWinner.gameObject.transform.position = new Vector3(0, -200, -3);
        _dernierJoueurAyantScore = 0;
        ScorePlayer1 = 0;
        ScorePlayer2 = 0;

        if (_objBall != null)
        {
            _objBall.gameObject.SendMessage("RestartPositionBall", -1, SendMessageOptions.RequireReceiver);
            _objBall.gameObject.SendMessage("Restart", null, SendMessageOptions.RequireReceiver);
        }        

        ResetCollisionBalle();
    }

    public void RetourMenu()
    {
        ObjTxtWinner.gameObject.transform.position = new Vector3(0, -200, -3);
        _dernierJoueurAyantScore = 0;
        ScorePlayer1 = 0;
        ScorePlayer2 = 0;

        SceneManager.LoadScene(0);
    }

    void TerminerPartie()
    {
        ObjTxtWinner.gameObject.transform.position = new Vector3(0, 0, -3);
        _objTxtWinner.text = string.Format("PLAYER {0} WINS!", ScorePlayer1 == _nbPtsPourGagner ? "ONE" : "TWO");

        _objBall.gameObject.SendMessage("CacherBalle", null, SendMessageOptions.RequireReceiver);
    }

    void AfficherInformationsDebug()
    {
        if (IsDebug)
        {
            string strInfosDebug = "";

            GameObject objBall = GameObject.FindGameObjectWithTag("Ball");
            BallControl ballControl = objBall.GetComponent<BallControl>();

            strInfosDebug += "Vitesse balle: " + ballControl.Vitesse.ToString();
            strInfosDebug += "\r\nDifficulté: " + (_difficulte == 1 ? "Facile" : (_difficulte == 2 ? "Moyen" : (_difficulte == 3 ? "Difficile" : "-")));
            strInfosDebug += "\r\nVitesse Joueur1: " + VitessePaletJoueur1.ToString();
            strInfosDebug += "\r\nVitesse Joueur2: " + VitessePaletJoueur2.ToString();
            strInfosDebug += "\r\nVitesse Balle initiale: " + VitesseBalle.ToString();

            _objTxtDebug.text = strInfosDebug;
        }
        else
        {
            _objTxtDebug.text = "";
        }
    }
}
