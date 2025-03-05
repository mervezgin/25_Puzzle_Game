using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private UIManager uiManager;
    private Board board;
    public float roundTime = 60f;
    private bool endingRound = false;
    public int currentScore;
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
    }
}
