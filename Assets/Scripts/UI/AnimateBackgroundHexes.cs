using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimateBackgroundHexes : MonoBehaviour
{
    public float speed;
    private bool animateBackground = false;
    private List<Transform> hexes = new List<Transform>();
    private List<Vector3> initPos = new List<Vector3>();
    private List<Vector3> endPoses = new List<Vector3>();
    public Vector3 offset = Vector3.zero;


    public void SetAnimateBackground(bool shouldAnimate)
    {
        animateBackground = shouldAnimate;
        if (animateBackground)
        {
            hexes = GetComponentsInChildren<Transform>().ToList();
        }

        for (int i = 0; i < hexes.Count; i++)
        {
            initPos.Add(hexes[i].position);
            int rand = Random.Range(0, 2);
            Debug.Log(rand);
            if (rand % 2 == 0)
            {
                endPoses.Add(hexes[i].position + offset);
            }
            else
            {
                endPoses.Add(hexes[i].position - offset);
            }
        }
    }

    private void Update()
    {
        if (animateBackground)
        {
            for (int i = 0; i < hexes.Count; i++)
            {
                /*                Debug.Log(i);
                                Debug.Log(initPos[i]);*/
                float pingPong = Mathf.PingPong(Time.time * speed, 1);
                hexes[i].position = Vector3.Lerp(initPos[i], endPoses[i], pingPong);
            }
        }
    }
}
