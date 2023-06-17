using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public int height; 
    public int width;
    
    public GameObject cellPrefab;
    public GameObject spawnLocation;

    public static CustomGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    public void CreateGrid()
    {
        var name = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 scale = cellPrefab.transform.localScale;
                Vector3 worldPosition = spawnLocation.transform.position + new Vector3(j * scale.x, 0, -i * scale.z);
                Transform cell = Instantiate(cellPrefab, worldPosition, Quaternion.identity).transform;
                
                cell.name = "Cell" + name;
                cell.tag = "Cell";

                name++;
            }
        }
        
        Debug.Log("The grid created.");
    }

}
