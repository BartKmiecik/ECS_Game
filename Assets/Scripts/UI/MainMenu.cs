using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CreateHexagonMap createHexagonMap;
    public AnimateBackgroundHexes animatedBackgroundHexes;
    public List<GameObject> uiPanels = new List<GameObject>();

    private void Start()
    {
        CreateBackground();
        animatedBackgroundHexes.SetAnimateBackground(true);
    }

    public void StartGame()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentScene + 1);
    }

    private void CreateBackground()
    {
        createHexagonMap.CreateMap();
        createHexagonMap.transform.rotation = Quaternion.Euler(-90, 0, 0);
        createHexagonMap.transform.localScale = new Vector3(.5f, 10f, .5f);
    }

    public void BackButton()
    {
        for (int i = 1; i < uiPanels.Count; i++)
        {
            uiPanels[i].gameObject.SetActive(false);
        }
        uiPanels[0].gameObject.SetActive(true);
    }

    public void StartGamePanel()
    {
        uiPanels[0].gameObject.SetActive(false);
        uiPanels[1].gameObject.SetActive(true);
    }

    public void Options()
    {
        uiPanels[0].gameObject.SetActive(false);
        uiPanels[2].gameObject.SetActive(true);
    }

    public void Achivements()
    {
        uiPanels[0].gameObject.SetActive(false);
        uiPanels[3].gameObject.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
