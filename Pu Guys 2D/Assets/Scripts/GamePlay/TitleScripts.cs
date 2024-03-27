using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScripts : MonoBehaviour
{
    public void startButton()
    {
        SceneManager.LoadScene("Game");
    }
}
