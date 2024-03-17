using UnityEngine;

public class Cell 
{
    public Vector3 wordPos;
    public Vector2Int gridIdx;

    public Cell(Vector3 pos, Vector2Int idx)
    {
        this.wordPos = pos;
        this.gridIdx = idx;
    }
}
