using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnPopupDamage : MonoBehaviour
{
    public GameObject popupPrefab;
    public static SpawnPopupDamage INSTANCE;

    private void Start()
    {
        INSTANCE = this;
    }

    public void CreatePopUp(float damage, Vector3 spawnPosition)
    {
        GameObject popUp = Instantiate(popupPrefab);
        popUp.GetComponent<TextMeshPro>().text = damage.ToString();
        popUp.transform.position = spawnPosition;
    }

    public void CreatePlayerPopUp(float damage, Vector3 spawnPosition)
    {
        GameObject popUp = Instantiate(popupPrefab);
        popUp.GetComponent<TextMeshPro>().text = damage.ToString();
        popUp.GetComponent<TextMeshPro>().color = Color.red;
        popUp.transform.position = spawnPosition;
    }
}
