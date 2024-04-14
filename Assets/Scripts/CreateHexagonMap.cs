using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHexagonMap : MonoBehaviour
{
    public GameObject hexagon;
    public float yOffset;
    public float xOffset;
    public float angleOffset;

    public int width;
    public int height;

    public void CreateMap()
    {
        for (int w = -(int)(width / 2); w < (int)(width / 2); w++)
        {
            for (int h = -(int)(height / 2); h < (int)(height / 2); h++)
            {
                float y = h * yOffset;
                if (w % 2 != 0)
                {
                    y += angleOffset;
                }
                Vector3 spawningPos = new Vector3(w * xOffset, 0, y);
                GameObject tmp = Instantiate(hexagon, spawningPos, Quaternion.identity, transform);
                tmp.transform.localScale = new Vector3(1, Random.Range(0.5f, 3f), 1);
            }
        }
    }

    private void Start()
    {
        CreateMap();
    }
}
