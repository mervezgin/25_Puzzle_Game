using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    public const string MAINMENU = "MainMenu";
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MAINMENU);
    }
}
