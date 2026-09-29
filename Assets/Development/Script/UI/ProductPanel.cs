using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductPanel : MonoBehaviour
{
    [SerializeField] private GameObject productPrefab;
    [SerializeField] private Image iconImage;
    [SerializeField] private OrderPanel orderPanel;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI productNameText;

    private int _currentAmount = 0;

    private void Start()
    {
        BaseProduct product = productPrefab.GetComponent<BaseProduct>();
        productNameText.text = product.ProductName;
        iconImage.sprite = product.Icon;
        amountText.text = _currentAmount.ToString();
    }

    public void OnClickIncrease()
    {
        _currentAmount++;
        amountText.text = _currentAmount.ToString();
        orderPanel.UpdateOrderSpecsList(productPrefab, _currentAmount);
        AudioManager.Instance.PlayClickSfx();
    }

    public void OnClickDecrease() 
    {
        _currentAmount--;
        if (_currentAmount < 0 )
        {
            _currentAmount = 0;
        }
        amountText.text = _currentAmount.ToString();

        orderPanel.UpdateOrderSpecsList(productPrefab, _currentAmount);
        AudioManager.Instance.PlayClickSfx();
    }

    public void ResetAmount()
    {
        _currentAmount = 0;
        amountText.text = _currentAmount.ToString();
    }
}
