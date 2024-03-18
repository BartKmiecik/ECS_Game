using UnityEngine;

public class Cell 
{
    public Vector3 wordPos;
    public Vector2Int gridIdx;
    public byte cost;
    public ushort bestCost;

    public Cell(Vector3 pos, Vector2Int idx)
    {
        this.wordPos = pos;
        this.gridIdx = idx;
        this.cost = 1;
        this.bestCost = ushort.MaxValue;
    }

    public void IncreaseCost(int cost)
    {
        if (this.cost == byte.MaxValue) { return; }
        if (cost + this.cost >= 255) { this.cost = 255; }
        else this.cost += (byte)cost;
    }
}
