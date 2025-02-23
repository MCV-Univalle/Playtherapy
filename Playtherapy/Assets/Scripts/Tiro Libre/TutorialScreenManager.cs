using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreenManager : MonoBehaviour
{
    private int currentPage = 0;
    
    public Button buttonPreviousPage;
    public Button buttonNextPage;
    public Button closeButton;
    public GameObject[] pages;

    void Start()
    {
        UpdatePage();
        buttonNextPage.onClick.AddListener(NextPage);
        buttonPreviousPage.onClick.AddListener(PreviousPage);
        closeButton.onClick.AddListener(CloseTutorial);
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    public void UpdatePage()
    {
      for (int i = 0; i<pages.Length; i++)
      {
            pages[i].SetActive(i == currentPage);
      }

        buttonPreviousPage.interactable = currentPage > 0;
        buttonNextPage.interactable = currentPage < pages.Length - 1;
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}
