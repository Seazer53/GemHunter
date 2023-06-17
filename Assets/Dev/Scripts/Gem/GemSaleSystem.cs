using System.Collections;
using System.Linq;
using UnityEngine;

public class GemSaleSystem : MonoBehaviour
{
    private bool isDestroyed = false;
    private bool leftArea = false;
    private void OnTriggerStay(Collider other)
    {
        leftArea = false;
        var sortedStackedGems = Player.Instance.stackedGems.OrderBy(go => -go.transform.position.y).ToList();
        foreach(var go in sortedStackedGems)
        {
            if (isDestroyed || go == sortedStackedGems.First())
            {
                StartCoroutine(DestroyGame(0.1f, go));
                isDestroyed = false;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        leftArea = true;
    }

    IEnumerator DestroyGame(float time, GameObject go)
    {
        float currentTime = 0.0f;
		
        do
        {
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time && !leftArea);

        if (!leftArea)
        {
            if (go)
            {
                Gem gem = go.GetComponent<Gem>();   
                
                if (gem)
                {
                    Player.Instance.collectedCoin += gem.price + Mathf.Round(go.transform.localScale.x * 100);
                    UIManager.Instance.SetCoinText(Player.Instance.collectedCoin);
                }
                
            }

            Player.Instance.stackedGems.Remove(go);
            Destroy(go);
            isDestroyed = true;
        }

    }
    
}
