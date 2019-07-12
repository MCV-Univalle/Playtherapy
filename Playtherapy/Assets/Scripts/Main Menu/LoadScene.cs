
using UnityEngine;
using System.Collections;

public class LoadLevels : MonoBehaviour
{
    public string levelToLoad;
    public StartTherapySession st;

    public void LoadLevelSelected()
    {
        st.StartPlayList();
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
        Time.timeScale = 1;
    }
}
