using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, settingsPanel, customizationPanel;
    private bool isCustomizing;

    [SerializeField] Animator cameraAnim, playerAnim;

    public void StartGame(Button button)
    {
        button.gameObject.SetActive(false);
        mainMenu.SetActive(true);
        Audio.PlaySound("InterfaceInteraction1", null);
    }

    public void EnterTower()
    {
        FloorManager.floor = 0;
        SceneLoader.instance.LoadScene("GameScene");
        Audio.PlaySound("InterfaceInteraction1", null);

    }

    public void OpenSettings(bool open)
    {
        mainMenu.SetActive(!open);
        settingsPanel.SetActive(open);
        Audio.PlaySound("InterfaceInteraction1", null);

    }

    public void OpenCustomization(bool open)
    {
        isCustomizing = open;
        if (isCustomizing) mainMenu.SetActive(false);
        else customizationPanel.SetActive(false);
        Invoke("ActivatePanelCustomizing", 1.2f);
        playerAnim.SetBool("isCustomizing", isCustomizing);
        cameraAnim.SetBool("isCustomizing", isCustomizing);
        Audio.PlaySound("InterfaceInteraction1", null);
    }

    public void ActivatePanelCustomizing()
    {
        if (isCustomizing) customizationPanel.SetActive(true);
        else mainMenu.SetActive(true);
    }
}
