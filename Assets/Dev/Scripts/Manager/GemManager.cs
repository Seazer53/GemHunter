using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GemManager : MonoBehaviour
{
    public List<GemData> gemTypes;
    public List<Cell> cells;
    
    public static GemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        FindCells();
        ChooseRandomGems();
    }

    void FindCells()
    {
        cells = new List<Cell>();
        var cellGameObjects = GameObject.FindGameObjectsWithTag("Cell");

        foreach (var cellGo in cellGameObjects)
        {
            Cell newCell = new Cell(true, cellGo.transform.position, cellGo.transform);
            cells.Add(newCell);
        }
        
    }

    void ChooseRandomGems()
    {
        foreach (var cell in cells)
        {
            var randIndex = Random.Range(0, gemTypes.Capacity);
            var gem = Instantiate(gemTypes[randIndex].prefab, cell.CellPosition + new Vector3(0,1,0), Quaternion.identity);

            cell.IsPlaceable = false;

        }
    }

    public void CreateRandomGem(Transform cell)
    {
        var randIndex = Random.Range(0, gemTypes.Capacity);
        var gem = Instantiate(gemTypes[randIndex].prefab, cell.position + new Vector3(0,1,0), Quaternion.identity);
    }

}
