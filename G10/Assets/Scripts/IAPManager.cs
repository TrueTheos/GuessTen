using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    // Start is called before the first frame update
    private string HintKeys10 = "com.shatteredbitsstudio.guessten.hints10";
    private string HintKeys50 = "com.shatteredbitsstudio.guessten.hints50";
    private string HintKeys100 = "com.shatteredbitsstudio.guessten.hints100";
    private string removeAds = "com.shatteredbitsstudio.guessten.removeads";

    public void OnPuchaseComplete(Product product)
    {
        try
        {
            switch (product.definition.id)
            {

                case string prod when prod == HintKeys10:
                    StoreManager.instance.UpdateKeys(10);
                    break;
                case string prod when prod == HintKeys50:
                    StoreManager.instance.UpdateKeys(50);
                    break;
                case string prod when prod == HintKeys100:
                    StoreManager.instance.UpdateKeys(100);
                    break;
                case string prod when prod == removeAds:
                    StoreManager.instance.LoadAdBool(true);
                    break;
            }
        }
        catch {

        }
    }

    
    
    public void OnPuchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(product.definition.id + " Failed because: " + failureReason);
    }
}
