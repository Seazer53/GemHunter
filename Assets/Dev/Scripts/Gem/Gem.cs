using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float price;
    public GemData.GemRarity type;
    public bool isGrow;
    public bool isCollectible;
    public bool isCollected;
    

    private void OnEnable()
    {
        SetData();
        StartCoroutine(GrowGem(5.0f));
    }

    void SetData()
    {
        if (CompareTag("Common"))
        {
            price = 50;
            type = GemData.GemRarity.Common;
        }
        
        else if (CompareTag("Rare"))
        {
            price = 100;
            type = GemData.GemRarity.Rare;
        }
        
        else if (CompareTag("Legendary"))
        {
            price = 200;
            type = GemData.GemRarity.Legendary;
        }
    }

    IEnumerator GrowGem(float time)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(1.0f, 1.0f, 1.0f);
		
        float currentTime = 0.0f;
		
        do
        {
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);

            if (transform.localScale.x > 0.25f)
            {
                isCollectible = true;
            }
            
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time & !isCollected);

        isGrow = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollectible)
        {
            isCollected = true;
        }
    }
}
