using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager uiManager;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    private void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;
            if (roundTime <= 0)
            {
                roundTime = 0;

            }
        }
        uiManager.timeText.text = roundTime.ToString("0.0") + " s";
    }
}
