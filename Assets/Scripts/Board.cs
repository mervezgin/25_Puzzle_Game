using UnityEngine;

public class Board : MonoBehaviour
{
    [HideInInspector] public int height = 7;
    [HideInInspector] public int width = 7;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Gem[] gems;
    private MatchingFinder matchingFinder;
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
}
