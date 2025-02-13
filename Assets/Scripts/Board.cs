using UnityEngine;

public class Board : MonoBehaviour
{
    private int height = 7;
    private int width = 7;
    [SerializeField] private GameObject tilePrefab;
    private void Start()
    {
        SetUpBackground();
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
            }
        }
    }
}
