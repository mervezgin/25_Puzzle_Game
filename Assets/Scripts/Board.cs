using UnityEngine;

public class Board : MonoBehaviour
{
    [HideInInspector] public MatchingFinder matchingFinder;
    [HideInInspector] public int height = 7;
    [HideInInspector] public int width = 7;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Gem[] gems;
    public Gem[,] allGems;
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
        matchingFinder.FindAllMatches();
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
        Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y, 0f), Quaternion.identity);
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
    }
}
