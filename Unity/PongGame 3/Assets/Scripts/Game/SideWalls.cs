using UnityEngine;
using System.Collections;

public class SideWalls : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D HitInfo)
    {
        if (HitInfo.name == "Ball")
        {
            string wallName = transform.name;
            GameManager.Score(wallName);
        }
    }
}
