using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class SpawningGizmos : MonoBehaviour
{
    public float radius;
    public int count;
    public float distance;
    private const float deg2rad = Mathf.Deg2Rad;
    public float offset;
    private float currentOffset;

    //Spawning enemies
    /* void OnDrawGizmos()
     {
         float space = 360 / count;
         for (int i = 0; i < count; i++)
         {
             float x = radius * Mathf.Sin(space * i * deg2rad);
             float y = radius * Mathf.Cos(space * i * deg2rad);
             *//*Debug.Log($"Degree {space * i} and x {x} y {y}");*//*
             Gizmos.color = Color.blue;
             Gizmos.DrawSphere(new Vector3(x, 1, y) + transform.position, 1);
         }
     }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float spacing = 360 / count;
        for (int i = 0; i < count; i++)
        {
            float x = radius * Mathf.Sin((spacing * i + currentOffset) * deg2rad);
            float y = radius * Mathf.Cos((spacing * i + currentOffset) * deg2rad);
            Gizmos.DrawLine(transform.position, new Vector3(x, 1, y));
        }
        currentOffset += offset;
        if (currentOffset >= 360)
        {
            currentOffset = 0;
        }
    }
}
