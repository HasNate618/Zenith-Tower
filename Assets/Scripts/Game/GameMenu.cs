using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject gamePanel, pausePanel, inventoryMenuPanel, inventoryPanel, deathPanel;
    private bool touchingScreen { get { return Input.touchCount > 1; } }

    [SerializeField] Text floorText;

    private void Start()
    {
        floorText.text = "FLOOR " + (FloorManager.floor + 1).ToString();
    }

    public void PauseGame()
    {
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        Audio.musicSource.Pause();
        Audio.PlaySound("InterfaceInteraction1", null);
    }
    
    public void UnpauseGame()
    {
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Audio.musicSource.UnPause();
        Audio.PlaySound("InterfaceInteraction1", null);
    }

    public void OpenInventory()
    {
        pausePanel.SetActive(false);
        inventoryMenuPanel.SetActive(true);
        inventoryPanel.SetActive(true);
        Audio.PlaySound("InterfaceInteraction1", null);
    }
    
    public void CloseInventory()
    {
        if (touchingScreen) return;
        pausePanel.SetActive(true);
        inventoryMenuPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        Audio.PlaySound("InterfaceInteraction1", null);
    }

    public void Restart()
    {
        SceneLoader.instance.LoadScene("GameScene");
        Audio.PlaySound("InterfaceInteraction1", null);
    }

    public void OpenMainMenu()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().enabled = false;
        CreditsManager.AddCreditsToBalance();
        SceneLoader.instance.LoadScene("MainMenu");
        Audio.PlaySound("InterfaceInteraction1", null);
    }

    [SerializeField] string[] deathScreenText;
    [SerializeField] Text[] deathScreenTexts;

    [SerializeField] GameObject[] deathScreenData;
    [SerializeField] Text[] data;
    public void EndGame()
    {
        gamePanel.SetActive(false);
        deathPanel.SetActive(true);
        Time.timeScale = 0;

        data[0].text = CreditsManager.balance.ToString();
        data[1].text = CreditsManager.attemptCredits.ToString();
        CreditsManager.AddCreditsToBalance();
        data[2].text = CreditsManager.balance.ToString();

        for (int i = 0; i < deathScreenTexts.Length; i++)
        {
            StartCoroutine(CoroutineUtilities.TypeOutText(deathScreenText[i], deathScreenTexts[i], 0.08f, i*.5f, deathScreenData[i]));
        }
    }
}
