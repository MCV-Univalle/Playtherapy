using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaylistManager : MonoBehaviour
{
    public static PlaylistManager pm;

    public GameObject transitionScreen;

    private List<string> playlist;
    private List<Object> playlistParameters;
    private int currentIndex;
    public bool active;
    public int timeBetweenGames;
    public float timeToScreen;

    private NextGameScreenManager ngsm;

    public void Start()
    {
        if (pm == null)
            pm = gameObject.GetComponent<PlaylistManager>();

        currentIndex = 0;
        active = false;
    }

    public void StartPlaylist()
    {
        active = true;
        GameObject go = Instantiate(transitionScreen) as GameObject;
        ngsm = go.GetComponent<NextGameScreenManager>();
        ngsm.nextGameText.text = playlist[currentIndex] + "\ncomenzará en";
        StartCoroutine(NextGameScreen(timeBetweenGames));
        Debug.Log("termina nextgame");
    }

    public void NextGame()
    {
        StartCoroutine(DelayedNextGameScreen());
    }

    private IEnumerator DelayedNextGameScreen()
    {
        Debug.Log("crear pantalla");
        yield return new WaitForSeconds(timeToScreen);
        GameObject go = Instantiate(transitionScreen) as GameObject;
        ngsm = go.GetComponent<NextGameScreenManager>();
        ngsm.nextGameText.text = playlist[currentIndex] + "\ncomenzará en";
        StartCoroutine(NextGameScreen(timeBetweenGames));
        Debug.Log("termina nextgame");
    }

    private IEnumerator NextGameScreen(int seconds)
    {
        ngsm.timeText.text = seconds.ToString();
        for (int i = 1; i <= seconds; i++)
        {
            yield return new WaitForSeconds(1f);
            ngsm.timeText.text = (seconds - i).ToString();
        }

        StartCoroutine(LoadNextMinigame());        
        Debug.Log("termina nextgamescreen");
    }

    public void StartNextMinigameNow()
    {
        StopAllCoroutines();
        StartCoroutine(LoadNextMinigame());
    }

    private IEnumerator LoadNextMinigame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(playlist[currentIndex]);
        yield return new WaitUntil(() => { return operation.isDone; });

        LoadParameters();

        currentIndex++;
        if (currentIndex == playlist.Count)
            active = false;       
    }

    private void LoadParameters()
    {
        //Debug.Log(playlistParameters[currentIndex]); // que le pasa a esto que no autocompleta reinstala esta monda
        ((IParametersManager)playlistParameters[currentIndex]).StartGame();
    }

    public void Test()
    {
        StartCoroutine(TestCoroutine());
    }

    private IEnumerator TestCoroutine()
    {        
        while (currentIndex < playlist.Count)
        {
            Debug.Log("ci: "+currentIndex+" --- count: "+playlist.Count);
            NextGame();
            yield return new WaitForSeconds(15f);
        }
    }

    public List<string> Playlist
    {
        get
        {
            return playlist;
        }

        set
        {
            playlist = value;
        }
    }

    public List<Object> PlaylistParameters
    {
        get
        {
            return playlistParameters;
        }

        set
        {
            playlistParameters = value;
        }
    }

    public int CurrentIndex
    {
        get
        {
            return currentIndex;
        }

        set
        {
            currentIndex = value;
        }
    }
}
