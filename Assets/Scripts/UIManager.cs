using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text winScoreText;
    public TMP_Text winText;
    public GameObject winStars1, winStars2, winStars3;
    public GameObject roundOverScreen;
    private void Start()
    {
        winStars1.SetActive(false);
        winStars2.SetActive(false);
        winStars3.SetActive(false);
    }
}
