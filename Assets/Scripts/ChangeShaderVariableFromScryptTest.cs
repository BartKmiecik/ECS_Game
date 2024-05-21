using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChangeShaderVariableFromScryptTest : MonoBehaviour
{
    Transform player;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        //SetCost(Random.Range(0.0f, 100.0f));
        player = FindObjectOfType<FollowEntity>().transform;
    }

    private void Update()
    {
        float tmp = math.round(Vector3.Distance(transform.position, player.position));
        SetCost(tmp);
    }


    public void SetCost(float cost)
    {
        rend.material.SetFloat("_Cost", cost);
    }
}
