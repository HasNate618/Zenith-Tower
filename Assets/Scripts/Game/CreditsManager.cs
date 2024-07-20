using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    public static int attemptCredits;
    public static int balance { get{return PlayerPrefs.GetInt("Credits", 0);} }

    public int[] itemSellValue;

    public static CreditsManager instance;

    [SerializeField] Text[] creditsTexts;

    void Start()
    {
        instance = this;
        if (FloorManager.floor == 0) attemptCredits = 0;
        UpdateCreditUI();
    }

    public void UpdateCreditUI()
    {
        foreach (Text creditText in creditsTexts)
        {
            creditText.text = attemptCredits.ToString();
        }
    }

    public static void AddCreditsToBalance()
    {
        PlayerPrefs.SetInt("Credits", balance + attemptCredits);
    }

    private void OnApplicationQuit()
    {
        AddCreditsToBalance();
    }
}
