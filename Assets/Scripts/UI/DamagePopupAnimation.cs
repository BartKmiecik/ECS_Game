using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupAnimation : MonoBehaviour
{
    public float effectSpeed;
    private Vector3 startScale;
    private Vector3 endScale;
    private Vector3 startAnimPos;
    public Vector3 animEndPos;
    private float currentTimer;

    private void Start()
    {
        currentTimer = 0;
        startScale = new Vector3 (.5f, .5f, .5f);
        endScale = new Vector3(1, 1, 1);
        transform.localScale = startScale;
        startAnimPos = transform.localPosition;
        animEndPos += startAnimPos;
    }

    void Update()
    {
        if (currentTimer < effectSpeed)
        {
            currentTimer += Time.deltaTime;
            float timer = 1 - (effectSpeed - currentTimer) / effectSpeed;
            transform.localScale = Vector3.Lerp(startScale, endScale, timer);
            //transform.position = Vector3.Lerp(startAnimPos, animEndPos, timer);

        } else
        {
            Destroy(gameObject);
        }
    }
}
