using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IDataPersistence
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Animator anim;
    public Camera gameCam;
    public GameObject stackPoint;

    public List<GameObject> stackedGems;
    
    public int collectedCommon;
    public int collectedRare;
    public int collectedLegendary;
    public float collectedCoin = 0;
    
    public static Player Instance { get; private set; }

    private static readonly int Move = Animator.StringToHash("Move");
    private bool offScreen;

    private void Awake()
    {
        stackedGems = new List<GameObject>();

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
        Application.targetFrameRate = 60;
    }

    private void OnTriggerEnter(Collider other)
    {
        Gem gem = other.gameObject.GetComponent<Gem>();

        if (gem)
        {
            if (gem.isCollectible)
            {
                float randX = Random.Range(-0.5f, 0.5f);
                float randY = Random.Range(0f, 2f);
                float randZ = Random.Range(-1.5f, -0.5f);

                foreach (var cell in GemManager.Instance.cells)
                {
                    if (Mathf.Abs(other.gameObject.transform.position.x - cell.CellPosition.x) < 0.05f && 
                        Mathf.Abs(other.gameObject.transform.position.z - cell.CellPosition.z) < 0.05f)
                    {
                        GemManager.Instance.CreateRandomGem(cell.CellTransform);
                        stackedGems.Add(other.gameObject);

                        if (other.gameObject.CompareTag(GemData.GemRarity.Common.ToString()))
                        {
                            collectedCommon++;
                        }
                        
                        else if (other.gameObject.CompareTag(GemData.GemRarity.Rare.ToString()))
                        {
                            collectedRare++;
                        }
                        
                        else if (other.gameObject.CompareTag(GemData.GemRarity.Legendary.ToString()))
                        {
                            collectedLegendary++;
                        }
                        
                        break;
                    }
                }

                other.gameObject.transform.position = stackPoint.transform.position + new Vector3(randX, randY, randZ);
                other.gameObject.transform.SetParent(stackPoint.transform);
            }
        }
        
    }

    public void Update()
    {
        Vector3 pos = gameCam.WorldToViewportPoint(transform.position);
        Vector3 offScreenVector = Vector3.zero;
        
        if (pos.x < 0.0)
        {
            offScreen = true;
            offScreenVector = Vector3.left;
        }

        if (1.0 < pos.x)
        {
            offScreen = true;
            offScreenVector = Vector3.right;
        }

        if (pos.y < 0.0)
        {
            offScreen = true;
            offScreenVector = Vector3.back;
        }

        if (1.0 < pos.y)
        {
            offScreen = true;
            offScreenVector = Vector3.forward;
        }
        
        if (variableJoystick.Horizontal != 0 || variableJoystick.Vertical != 0)
        {
            anim.SetBool(Move, true);

            if (offScreen)
            {
                var joystick = new Vector3(variableJoystick.Horizontal,
                    0f,
                    variableJoystick.Vertical);
                
                var angle = Vector3.Angle(offScreenVector, joystick);

                if (angle > 90)
                {
                    offScreen = false;
                }
            }
            
            if(Mathf.Abs(variableJoystick.Horizontal) > Mathf.Abs(variableJoystick.Vertical) && !offScreen)
            {
                transform.Translate(Vector3.forward * (Time.deltaTime * speed * Mathf.Abs(variableJoystick.Horizontal)));
            }
            
            else if(Mathf.Abs(variableJoystick.Horizontal) < Mathf.Abs(variableJoystick.Vertical) && !offScreen) 
            {
                
                transform.Translate(Vector3.forward * (Time.deltaTime * speed * Mathf.Abs(variableJoystick.Vertical)));
            }

            transform.LookAt(transform.localPosition + new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical));
            
        }

        else
        {
            anim.SetBool(Move, false);
        }

    }

    // IDataPersistence implementation of Load Data. Loads the data from file.
    public void LoadData(PlayerData data)
    {
        collectedCommon = data.collectedCommon;
        collectedRare = data.collectedRare;
        collectedLegendary = data.collectedLegendary;

        collectedCoin = data.collectedCoin;
        
        UIManager.Instance.SetCoinText(collectedCoin);

    }

    // IDataPersistence implementation of Save Data. Saves current reference to the PlayerData to a file.
    public void SaveData(ref PlayerData data)
    {
        data.collectedCommon = collectedCommon;
        data.collectedRare = collectedRare;
        data.collectedLegendary = collectedLegendary;
        data.collectedCoin = collectedCoin;
    }
}
