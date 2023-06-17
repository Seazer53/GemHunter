using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text coinText;
    public Button collectedGemButton;
    public Button closeMenuButton;
    public Canvas collectedGemsCanvas;
    public GameObject content;
    public GameObject infoCardPrefab;

    private List<GameObject> instantiatedInfoCards;

    public static UIManager Instance { get; private set; }

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
        
        SetListeners();

        instantiatedInfoCards = new List<GameObject>();
    }

    private void SetListeners()
    {
        closeMenuButton.onClick.AddListener(CloseGemCollectedCanvas);
        collectedGemButton.onClick.AddListener(ShowGemCollectedCanvas);
    }

    public void SetCoinText(float value)
    {
        coinText.text = value.ToString();
    }

    private void ShowGemCollectedCanvas()
    {
        collectedGemsCanvas.enabled = true;
        CreateAndSetInfoCards();
    }

    private void CloseGemCollectedCanvas()
    {
        collectedGemsCanvas.enabled = false;
    }

    private void CreateAndSetInfoCards()
    {
        GameObject go = infoCardPrefab; 
        
        foreach (var gemType in GemManager.Instance.gemTypes)
        {
            if (instantiatedInfoCards.Count < GemManager.Instance.gemTypes.Count)
            {
                go = Instantiate(infoCardPrefab, content.transform, false);
                instantiatedInfoCards.Add(go);
            }

            var gemImage = go.transform.GetChild(0);
            gemImage.gameObject.GetComponent<Image>().sprite = gemType.icon;
            
            var gemTextGo = go.transform.GetChild(1);
            var gemText = gemTextGo.GetComponent<TMP_Text>();

            if (gemType.type == GemData.GemRarity.Common)
            {
                gemText.text = "Gem Type: " + gemType.type + "\n" + "Collected: " + Player.Instance.collectedCommon;
            }
            
            else if (gemType.type == GemData.GemRarity.Rare)
            {
                gemText.text = "Gem Type: " + gemType.type + "\n" + "Collected: " + Player.Instance.collectedRare;
            }
            
            else if (gemType.type == GemData.GemRarity.Legendary)
            {
                gemText.text = "Gem Type: " + gemType.type + "\n" + "Collected: " + Player.Instance.collectedLegendary;
            }

        }
    }
    
}
