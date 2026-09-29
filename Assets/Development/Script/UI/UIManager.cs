using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnCoinUpdated += UpdateCoinText;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCoinUpdated -= UpdateCoinText;
    }

    public void UpdateCoinText(float totalCoin) 
    {
        coinText.text = totalCoin.ToString(); 
    }
}
