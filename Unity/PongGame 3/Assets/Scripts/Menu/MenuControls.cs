using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {

    private TextMesh _textMesh;

    void Start()
    {
        _textMesh = gameObject.GetComponent<TextMesh>();
    }

    void OnMouseOver()
    {
        _textMesh.color = Color.green;
    }

    void OnMouseExit()
    {
        _textMesh.color = Color.white;
    }

    void OnMouseDown()
    {
        transform.localScale *= 0.9F;
    }

    void OnMouseUp()
    {
        switch (gameObject.transform.name)
        {
            case "btn1Joueur":
                {
                    GameManager.ConfigNbJoueurs = enmConfigNbJoueurs._1Joueur;

                    // On load le menu de difficultés
                    SceneManager.LoadScene(2);
                    return;
                }
            case "btn2Joueur":
                {
                    GameManager.Difficulte = 0; // Pas de difficultés puisqu'il y a 2 joueurs.
                    GameManager.ConfigNbJoueurs = enmConfigNbJoueurs._2Joueurs;
                }
                break;
            case "btnClose":
                {
                    Application.Quit();
                    return;
                }
            case "btnFacile":
                {
                    GameManager.Difficulte = 1;
                }
                break;
            case "btnNormal":
                {
                    GameManager.Difficulte = 2;
                }
                break;
            case "btnDifficile":
                {
                    GameManager.Difficulte = 3;
                }
                break;
            case "btnRetour":
                {
                    // Retour - On load le menu principal.
                    SceneManager.LoadScene(0);
                    return;
                }
        }

        // On load la game.
        SceneManager.LoadScene(1);
    }
}
