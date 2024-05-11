using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float maxTimer;
    private float currentTimer;
    private bool playerInside = false;
    private Vector3 vectorOne = new Vector3(1, 1, 1);

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }

    private void LateUpdate()
    {
        if (playerInside)
        {
            if (currentTimer <= maxTimer) {
                currentTimer += Time.deltaTime;
                spriteRenderer.transform.localScale = vectorOne * (maxTimer - currentTimer) / maxTimer;
            }
        }
    }
}
