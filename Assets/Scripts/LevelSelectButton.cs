using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public string LEVELTOLOAD;
    public GameObject star1, star2, star3;
    public void LoadLevel()
    {
        SceneManager.LoadScene(LEVELTOLOAD);
    }
}
