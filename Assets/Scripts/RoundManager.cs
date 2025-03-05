using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private UIManager uiManager;
    private Board board;
    public float roundTime = 60f;
    private bool endingRound = false;
    public int currentScore;
    public int scoreTarget1, scoreTarget2, scoreTarget3;
    public float displayScore;
    public float scoreSpeed;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
    }
    private void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;
            if (roundTime <= 0)
            {
                roundTime = 0;
                endingRound = true;
            }
        }
        if (endingRound && board.currentBoardState == Board.BoardState.MOVE)
        {
            WinCheck();
            endingRound = false;
        }
        uiManager.timeText.text = roundTime.ToString("0.0") + " s";
        displayScore = Mathf.Lerp(displayScore, currentScore, scoreSpeed * Time.deltaTime);
        uiManager.scoreText.text = displayScore.ToString("0");
    }
    private void WinCheck()
    {
        uiManager.roundOverScreen.SetActive(true);
        uiManager.winScoreText.text = currentScore.ToString();
        if (currentScore >= scoreTarget3)
        {
            uiManager.winScoreText.text = "Congratulations ! You earned 3 stars!";
            uiManager.winStars3.SetActive(true);
        }
        else if (currentScore >= scoreTarget2)
        {
            uiManager.winScoreText.text = "Congratulations ! You earned 2 stars!";
            uiManager.winStars2.SetActive(true);
        }
        else if (currentScore >= scoreTarget1)
        {
            uiManager.winScoreText.text = "Congratulations ! You earned 1 star!";
            uiManager.winStars1.SetActive(true);
        }
        else
        {
            uiManager.winScoreText.text = "Oh no! No stars for you! Try again?";
        }
    }
}
