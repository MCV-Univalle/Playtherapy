using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
	public string levelToLoad;

    public void LoadLevelSelected()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
        Time.timeScale = 1;
    }

}
