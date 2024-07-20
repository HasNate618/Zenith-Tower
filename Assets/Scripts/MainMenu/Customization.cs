using UnityEngine;
using UnityEngine.UI;

public class Customization : MonoBehaviour
{
    public static int[] unlockedHelmets, unlockedSuitColours, unlockedLightColours;
    public static int helmetID, suitColourID, lightColourID;

    [SerializeField] Text helmetText, suitColourText, lightColourText, creditsText;
    [SerializeField] string[] helmetNames, suitColourNames, lightColourNames;
    [SerializeField] Color[] suitColours, lightColours;
    [SerializeField] float emissionStrength;

    [SerializeField] HelmetManager helmetManager;
    [SerializeField] Texture2D characterTexture;
    [SerializeField] Material characterMat;

    [SerializeField] GameObject[] purchaseButtons;
    public static int credits
    {
        get
        {
            return PlayerPrefs.GetInt("Credits", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Credits", value);
        }
    }

    private void Start()
    {
        characterMat.EnableKeyword("_EMISSION");
        LoadOutfit();
        creditsText.text = credits.ToString();
    }

    public void SelectHelmet(bool increase)
    {
        if (increase)
        {
            if (helmetID == helmetManager.helmets.Length - 1)
            {
                helmetID = 0;
            }
            else
            {                
                helmetID++;
            }
        }
        else
        {
            if (helmetID == 0)
            {
                helmetID = helmetManager.helmets.Length - 1;
            }
            else
            {
                helmetID--;
            }
        }

        helmetText.text = helmetNames[helmetID];
        helmetManager.SetHelmet(helmetID);

        if (unlockedHelmets[helmetID] == 1)
        {
            PlayerPrefs.SetInt("Helmet", helmetID);
            purchaseButtons[0].SetActive(false);
        }
        else
        {
            print("Purchase This To Equip");
            purchaseButtons[0].SetActive(true);
        }

        Audio.PlaySound("InterfaceInteraction3", null);
    }

    public void SelectSuitColour(bool increase)
    {
        if (increase)
        {
            if (suitColourID == suitColours.Length - 1)
            {
                suitColourID = 0;
            }
            else
            {
                suitColourID++;
            }
        }
        else
        {
            if (suitColourID == 0)
            {
                suitColourID = suitColours.Length - 1;
            }
            else
            {
                suitColourID--;
            }
        }

        suitColourText.text = suitColourNames[suitColourID];
        characterTexture.SetPixel(0, 1, suitColours[suitColourID]);
        characterTexture.Apply();

        if (unlockedSuitColours[suitColourID] == 1)
        {
            PlayerPrefs.SetInt("SuitColour", suitColourID);
            purchaseButtons[1].SetActive(false);
        }
        else
        {
            print("Purchase This To Equip");
            purchaseButtons[1].SetActive(true);
        }
        Audio.PlaySound("InterfaceInteraction3", null);
    }

    public void SelectLightColour(bool increase)
    {
        if (increase)
        {
            if (lightColourID == lightColours.Length - 1)
            {
                lightColourID = 0;
            }
            else
            {
                lightColourID++;
            }
        }
        else
        {
            if (lightColourID == 0)
            {
                lightColourID = lightColours.Length - 1;
            }
            else
            {
                lightColourID--;
            }
        }

        lightColourText.text = lightColourNames[lightColourID];
        characterMat.SetColor("_EmissionColor", lightColours[lightColourID] * emissionStrength);

        if (unlockedLightColours[lightColourID] == 1)
        {
            PlayerPrefs.SetInt("LightColour", lightColourID);
            purchaseButtons[2].SetActive(false);
        }
        else
        {
            print("Purchase This To Equip");
            purchaseButtons[2].SetActive(true);
        }
        Audio.PlaySound("InterfaceInteraction3", null);
    }

    public void PurchaseItem(int item)
    {
        bool purchased = false;

        switch (item)
        {
            case 0:
                if (credits >= 2800)
                {
                    credits -= 2800;
                    unlockedHelmets[helmetID] = 1;
                    SaveArray(unlockedHelmets, "UnlockedHelmets");
                    PlayerPrefs.SetInt("Helmet", helmetID);
                    purchased = true;
                }
                break;
            case 1:
                if (credits >= 2400)
                {
                    credits -= 2400;
                    unlockedSuitColours[suitColourID] = 1;
                    SaveArray(unlockedSuitColours, "UnlockedSuitColours");
                    PlayerPrefs.SetInt("SuitColour", suitColourID);
                    purchased = true;
                }
                break;
            case 2:
                if (credits >= 2000)
                {
                    credits -= 2000;
                    unlockedLightColours[lightColourID] = 1;
                    SaveArray(unlockedLightColours, "UnlockedLightColours");
                    PlayerPrefs.SetInt("LightColour", lightColourID);
                    purchased = true;
                }
                break;
        }

        if (purchased)
        {
            creditsText.text = credits.ToString();
            purchaseButtons[item].SetActive(false);
            Audio.PlaySound("ApplyEnhancement", null);
        }
        else
        {
            Audio.PlaySound("InterfaceError", null);
        }
    }

    public void LoadOutfit()
    {
        helmetID = PlayerPrefs.GetInt("Helmet", 1);
        helmetText.text = helmetNames[helmetID];
        helmetManager.SetHelmet(99);
        unlockedHelmets = GetArray("UnlockedHelmets");
        purchaseButtons[0].SetActive(false);

        suitColourID = PlayerPrefs.GetInt("SuitColour", 0);
        suitColourText.text = suitColourNames[suitColourID];
        characterTexture.SetPixel(0, 1, suitColours[suitColourID]);
        characterTexture.Apply();
        unlockedSuitColours = GetArray("UnlockedSuitColours");
        purchaseButtons[1].SetActive(false);

        lightColourID = PlayerPrefs.GetInt("LightColour", 0);
        lightColourText.text = lightColourNames[lightColourID];
        characterMat.SetColor("_EmissionColor", lightColours[lightColourID] * emissionStrength);
        unlockedLightColours = GetArray("UnlockedLightColours");
        purchaseButtons[2].SetActive(false);
    }

    public void SaveArray(int[] array, string saveName)
    {
        for (int i = 0; i < array.Length; i++)
        {
            PlayerPrefs.SetInt(saveName + i, array[i]);
            PlayerPrefs.SetInt(saveName + "TotalLength", array.Length);
        }
    }

    public int[] GetArray(string baseSaveName)
    {
        int[] ReconstructedArrayFromName = new int[PlayerPrefs.GetInt(baseSaveName + "TotalLength")];
        if (PlayerPrefs.HasKey(baseSaveName + "TotalLength"))
        {
            for (int i = 0; i < ReconstructedArrayFromName.Length; i++)
            {
                ReconstructedArrayFromName[i] = PlayerPrefs.GetInt(baseSaveName + i);
            }
            return ReconstructedArrayFromName;
        }
        else
        {
            switch (baseSaveName)
            {
                case "UnlockedHelmets":
                    return new int[] { 1, 1, 0, 0, 0, 0, 0 };
                case "UnlockedSuitColours":
                    int[] lightColourArray = new int[suitColours.Length];
                    lightColourArray[0] = 1;
                    return lightColourArray;
                case "UnlockedLightColours":
                    int[] suitColourArray = new int[lightColours.Length];
                    suitColourArray[0] = 1;
                    return suitColourArray;
            }

            return null;
        }
    }
}
