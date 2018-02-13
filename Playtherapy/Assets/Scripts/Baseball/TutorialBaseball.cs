using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBaseball : MonoBehaviour {

    // Use this for initialization
    private int currentPage = 0;

    public Button buttonPreviousPage;
    public Button buttonNextPage;
    public GameObject[] pages;
    
    void Start()
    {
        currentPage = 0;
        buttonPreviousPage.interactable = false;

        if (pages.Length < 2)
            buttonNextPage.interactable = false;

        if (pages.Length > 0)
            pages[0].SetActive(true);
    }

    public void NextPage()
    {
        buttonPreviousPage.interactable = true;

        int nextPage = currentPage + 1;

        if (nextPage + 1 > pages.Length - 1)
            buttonNextPage.interactable = false;
        else
            buttonNextPage.interactable = true;

        pages[currentPage].SetActive(false);
        pages[nextPage].SetActive(true);
        currentPage = nextPage;
    }

    public void PreviousPage()
    {
        buttonNextPage.interactable = true;

        int previousPage = currentPage - 1;

        if (previousPage - 1 < 0)
            buttonPreviousPage.interactable = false;
        else
            buttonPreviousPage.interactable = true;

        pages[currentPage].SetActive(false);
        pages[previousPage].SetActive(true);
        currentPage = previousPage;
    }

    // Update is called once per frame
    
}



