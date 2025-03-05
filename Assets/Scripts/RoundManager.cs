using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager uiManager;
    private Board board;
    private bool endingRound = false;
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
    }
    private void WinCheck()
    {
        uiManager.roundOverScreen.SetActive(true);
    }
}
