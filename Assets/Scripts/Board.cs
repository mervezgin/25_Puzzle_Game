using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public enum BoardState
    {
        WAIT,
        MOVE
    }
    public BoardState currentBoardState = BoardState.MOVE;
    [HideInInspector] public MatchingFinder matchingFinder;
    [HideInInspector] public int height = 7;
    [HideInInspector] public int width = 7;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Gem[] gems;
    public Gem[,] allGems;
    public Gem bomb;
    public float bombChance = 2f;
    public float gemSpeed;
    private void Awake()
    {
        matchingFinder = FindObjectOfType<MatchingFinder>();

    }
    private void Start()
    {
        allGems = new Gem[width, height];
        SetUpBackground();
    }
    private void Update()
    {
        // matchingFinder.FindAllMatches();
    }
    private void SetUpBackground()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject tileBG = Instantiate(tilePrefab, position, Quaternion.identity);
                tileBG.transform.parent = transform;
                tileBG.name = "BG_Tile - " + x + ", " + y;

                int gemToUse = Random.Range(0, gems.Length);

                int iterations = 0;
                while (MatchesAt(new Vector2Int(x, y), gems[gemToUse]) && iterations < 100)
                {
                    gemToUse = Random.Range(0, gems.Length);
                    iterations++;
                }
                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
            }
        }
    }
    private void SpawnGem(Vector2Int position, Gem gemToSpawn)
    {
        if (Random.Range(0f, 100f) < bombChance)
        {
            gemToSpawn = bomb;
        }
        Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y + height, 0f), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = "Gem - " + position.x + ", " + position.y;
        allGems[position.x, position.y] = gem;
        gem.SetUpGem(position, this);
    }
    private bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
    {
        if (posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type && allGems[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }
        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type && allGems[posToCheck.x, posToCheck.y - 2].type == gemToCheck.type)
            {
                return true;
            }
        }
        return false;
    }
    private void DestroyMatchedGemAt(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y] != null)
        {
            if (allGems[pos.x, pos.y].isMatched)
            {
                Instantiate(allGems[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
                Destroy(allGems[pos.x, pos.y].gameObject);
                allGems[pos.x, pos.y] = null;
            }
        }
    }
    public void DestroyMatches()
    {
        for (int i = 0; i < matchingFinder.currentMatches.Count; i++)
        {
            if (matchingFinder.currentMatches[i])
            {
                DestroyMatchedGemAt(matchingFinder.currentMatches[i].posIndex);
            }
        }
        StartCoroutine(DecreaseRowCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(0.2f);
        int nullCounter = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    allGems[x, y].posIndex.y -= nullCounter;
                    allGems[x, y - nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }
            nullCounter = 0;
        }
        StartCoroutine(FillBoardCo());
    }
    private IEnumerator FillBoardCo()
    {
        yield return new WaitForSeconds(0.5f);
        RefillBoard();

        yield return new WaitForSeconds(0.5f);
        matchingFinder.FindAllMatches();
        if (matchingFinder.currentMatches.Count > 0)
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            currentBoardState = BoardState.MOVE;
        }
    }
    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    int gemToUse = Random.Range(0, gems.Length);
                    SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }
            }
        }
        CheckMisplacedGems();
    }
    private void CheckMisplacedGems()
    {
        List<Gem> foundGems = new List<Gem>();
        foundGems.AddRange(FindObjectsOfType<Gem>());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (foundGems.Contains(allGems[x, y]))
                {
                    foundGems.Remove(allGems[x, y]);
                }
            }
        }

        foreach (Gem g in foundGems)
        {
            Destroy(g.gameObject);
        }
    }
}



