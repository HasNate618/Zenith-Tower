using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration, waitTime;
    private static bool firstLoad = true;

    private void Awake()
    {
        instance = this;
        if (firstLoad)
        {
            if (!Application.isEditor) Application.targetFrameRate = 30;
            else Application.targetFrameRate = 60;
            gameObject.SetActive(false);
            firstLoad = false;
        }
        else StartCoroutine(OnOpenScene());
    }

    public void LoadScene(string newScene)
    {
        if (newScene == "MainMenu") InventorySystem.Reset();
        gameObject.SetActive(true);
        Time.timeScale = 1;
        StartCoroutine(OnLoadScene(newScene));
    }

    private IEnumerator OnLoadScene(string newScene)
    {
        float elapsed = 0;
        Color color = Color.black;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        elapsed = 0;
        while (elapsed < waitTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(newScene);
    }

    private IEnumerator OnOpenScene()
    {
        float elapsed = fadeDuration;
        Color color = Color.black;

        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            color.a = elapsed / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
        gameObject.SetActive(false);
    }
}
