using UnityEngine;

public class Cell
{
    public bool IsPlaceable;
    public Vector3 CellPosition;
    public Transform CellTransform;

    public Cell(bool isPlaceable, Vector3 cellPosition, Transform cellTransform)
    {
        IsPlaceable = isPlaceable;
        CellPosition = cellPosition;
        CellTransform = cellTransform;
    }
}
