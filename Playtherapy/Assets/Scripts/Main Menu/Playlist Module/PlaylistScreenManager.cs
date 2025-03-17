using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaylistScreenManager : MonoBehaviour
{
    public Dropdown minigamesDropdown;
    public GameObject playlistUI;
    public GameObject parametersHolder;
    public GameObject listElement;
    public GameObject[] parametersElement;
    public Button startButton;
    public Button startButtonFoot;

    private List<string> playlist;
    private List<string> minigames;
    private GameObject currrentParamentersScreen;

    // Use this for initialization
    void Start()
    {

        startButton.interactable = false;
        startButtonFoot.interactable = false;

        playlist = new List<string>();
        minigames = new List<string>();

        //minigames.Add("Atrapalo");
        minigames.Add("Baseball");
        minigames.Add("Fight");
        //minigames.Add("Cavano");
        //minigames.Add("Dulce Hogar");
        minigames.Add("El Gran Viaje");
        minigames.Add("Figuras Magicas");
		//minigames.Add("Futbol Libre");
        minigames.Add("Guerra Medieval");
        minigames.Add("Piano");
        minigames.Add("Rieles");
        //minigames.Add("Space");
        //minigames.Add("Sushi Samurai");
        minigames.Add("Tiro Libre");
        minigames.Add("Topos");
        minigames.Add("Vecinos Invasores");
        minigames.Add("Frenesi de Compras");

        minigamesDropdown.AddOptions(minigames);
    }

    public void AddMinigame()
    {
        string name = minigamesDropdown.captionText.text;
        int index = playlist.Count;
        playlist.Add(name);

        GameObject go = Instantiate(listElement, playlistUI.transform) as GameObject;
        go.GetComponent<PlaylistElementBehaviour>().Initialize(name, this, index);

        if (currrentParamentersScreen != null)
            currrentParamentersScreen.SetActive(false);

        string parametersName = name + " Parameters Panel";
        for (int i = 0; i < parametersElement.Length; i++)
        {
            if (parametersName.Equals(parametersElement[i].name))
                currrentParamentersScreen = Instantiate(parametersElement[i], parametersHolder.transform) as GameObject;
        }

        if (index > 0)
            playlistUI.transform.GetChild(index - 1).gameObject.GetComponent<PlaylistElementBehaviour>().UpdateElement();

        startButton.interactable = true;
        startButtonFoot.interactable = true;

    }

    public void MoveUp(int currentIndex)
    {
        //Debug.Log("UP: " + currentIndex);
        GameObject go1 = playlistUI.transform.GetChild(currentIndex - 1).gameObject;
        GameObject go2 = playlistUI.transform.GetChild(currentIndex).gameObject;

        go2.transform.SetSiblingIndex(currentIndex - 1);
        parametersHolder.transform.GetChild(currentIndex).SetSiblingIndex(currentIndex - 1);

        go1.GetComponent<PlaylistElementBehaviour>().index = currentIndex;
        go1.GetComponent<PlaylistElementBehaviour>().UpdateElement();
        go2.GetComponent<PlaylistElementBehaviour>().index = currentIndex - 1;
        go2.GetComponent<PlaylistElementBehaviour>().UpdateElement();

        string temp = playlist[currentIndex - 1];
        playlist[currentIndex - 1] = playlist[currentIndex];
        playlist[currentIndex] = temp;
    }

    public void MoveDown(int currentIndex)
    {
        GameObject go1 = playlistUI.transform.GetChild(currentIndex).gameObject;
        GameObject go2 = playlistUI.transform.GetChild(currentIndex + 1).gameObject;

        go1.transform.SetSiblingIndex(currentIndex + 1);
        parametersHolder.transform.GetChild(currentIndex).SetSiblingIndex(currentIndex + 1);

        go1.GetComponent<PlaylistElementBehaviour>().index = currentIndex + 1;
        go1.GetComponent<PlaylistElementBehaviour>().UpdateElement();
        go2.GetComponent<PlaylistElementBehaviour>().index = currentIndex;
        go2.GetComponent<PlaylistElementBehaviour>().UpdateElement();

        string temp = playlist[currentIndex];
        playlist[currentIndex] = playlist[currentIndex + 1];
        playlist[currentIndex + 1] = temp;
    }

    public void Remove(int currentIndex)
    {
        Destroy(playlistUI.transform.GetChild(currentIndex).gameObject);
        Destroy(parametersHolder.transform.GetChild(currentIndex).gameObject);
        playlist.RemoveAt(currentIndex);
        //Debug.Log("REMOVE:"+currentIndex);
        //Debug.Log("TOTAL:"+playlist.Count);        

        for (int i = currentIndex + 1; i < playlist.Count + 1; i++)
        {
            Debug.Log(playlistUI.transform.GetChild(i).gameObject.GetComponent<PlaylistElementBehaviour>().label.text);
            playlistUI.transform.GetChild(i).gameObject.GetComponent<PlaylistElementBehaviour>().index -= 1;
            playlistUI.transform.GetChild(i).gameObject.GetComponent<PlaylistElementBehaviour>().UpdateElement();
        }

        if (IsLastIndex(currentIndex - 1))
            playlistUI.transform.GetChild(currentIndex - 1).gameObject.GetComponent<PlaylistElementBehaviour>().UpdateElement();

        if (playlist.Count == 0)
            startButton.interactable = false;
            startButtonFoot.interactable = false;

    }

    public void Select(int currentIndex)
    {
        if (currrentParamentersScreen != null)
            currrentParamentersScreen.SetActive(false);
        parametersHolder.transform.GetChild(currentIndex).gameObject.SetActive(true);
        currrentParamentersScreen = parametersHolder.transform.GetChild(currentIndex).gameObject;
    }

    public void StartPlaylist()
    {
        PlaylistManager.pm.CurrentIndex = 0;
        PlaylistManager.pm.Playlist = playlist;
        PlaylistManager.pm.PlaylistParameters = LoadParameters();
        PlaylistManager.pm.StartPlaylist();
        //PlaylistManager.pm.Test();
    }

    public List<Object> LoadParameters()
    {
        List<Object> playlistParameters = new List<Object>();

        for (int i = 0; i < playlist.Count; i++)
        {
            playlistParameters.Add(parametersHolder.transform.GetChild(i).gameObject.GetComponent<MonoBehaviour>());
           Debug.Log(playlistParameters[playlistParameters.Count - 1].name);
        }

        return playlistParameters;
    }

    public bool IsLastIndex(int index)
    {
        return index == playlist.Count - 1 && playlist.Count > 0;
    }
}
