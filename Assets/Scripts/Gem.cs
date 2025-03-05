using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public enum GemType
    {
        BLUE,
        GREEN,
        RED,
        YELLOW,
        PURPLE,
        BOMB
    }
    [HideInInspector] public Vector2Int posIndex;
    [HideInInspector] public Board board;
    public GemType type;
    public GameObject destroyEffect;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2Int previousPosition;
    private Gem otherGem;
    private bool mousePressed;
    public bool isMatched;
    private float swipeAngle = 0;
    public int blastSize = 2;
    public int scoreValue = 10;

    private void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allGems[posIndex.x, posIndex.y] = this;
        }

        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
            if (board.currentBoardState == Board.BoardState.MOVE && board.roundManager.roundTime > 0)
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            }
        }
    }
    public void SetUpGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }
    private void OnMouseDown()
    {
        if (board.currentBoardState == Board.BoardState.MOVE && board.roundManager.roundTime > 0)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePressed = true;
        }
    }
    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;
        Debug.Log(swipeAngle);
        MovePieces();
    }
    private void MovePieces()
    {
        previousPosition = posIndex;
        if (swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            otherGem.posIndex.x--;
            posIndex.x++;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            otherGem.posIndex.y--;
            posIndex.y++;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            otherGem.posIndex.y++;
            posIndex.y--;
        }
        else if (swipeAngle > 135 || swipeAngle < -135 && posIndex.x > 0)
        {
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            otherGem.posIndex.x++;
            posIndex.x--;
        }
        board.allGems[posIndex.x, posIndex.y] = this;
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;
        StartCoroutine(CheckMoveCo());
    }
    public IEnumerator CheckMoveCo()
    {
        board.currentBoardState = Board.BoardState.WAIT;
        yield return new WaitForSeconds(0.5f);
        board.matchingFinder.FindAllMatches();
        if (otherGem != null)
        {
            if (!isMatched && !otherGem.isMatched)
            {
                otherGem.posIndex = posIndex;
                posIndex = previousPosition;

                board.allGems[posIndex.x, posIndex.y] = this;
                board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;

                yield return new WaitForSeconds(0.5f);

                board.currentBoardState = Board.BoardState.MOVE;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
