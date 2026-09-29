using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderPanel : MonoBehaviour,IInteractable
{
    [SerializeField] private Camera orderCamera; 

    [Header("Main Panels")]
    [SerializeField] private GameObject categoryPanel;    
    [SerializeField] private GameObject productListPanel; 
    [SerializeField] private GameObject cartConfirmPanel;

    [Header("Category List")]
    [SerializeField] private GameObject foodList;
    [SerializeField] private GameObject drinkList;
    [SerializeField] private GameObject snackList;

    [Header("Total Display")]
    [SerializeField] private TextMeshProUGUI totalText;

    [Header("Cart Summary")]
    [SerializeField] private TextMeshProUGUI cartSummaryText;   //hangi üründen kaç adet text
    [SerializeField] private TextMeshProUGUI orderReceivedText; //sipariþ onay mesajý
    
    [Header("All Product Panels For Reset")]
    [SerializeField] private List<ProductPanel> allProductPanels;

    [SerializeField] private List<OrderSpecs> _orderSpecs; //sepette olan ürünlerin listesi

    [SerializeField] private TextMeshProUGUI insufficientFundsText; //yetersiz bakiye texti
    
    [SerializeField] private TextMeshProUGUI maxOrderExceededText; //kutu kapasitesininden fazla sipariþ alýrsak gösterilecek uyarý yazýsý

    private bool _isInteracted;
    private Camera _mainCamera;

    private void Awake()
    {
        orderCamera.gameObject.SetActive(false);
        _mainCamera = Camera.main; 

        orderReceivedText.gameObject.SetActive(false);
        cartConfirmPanel.SetActive(false);
        productListPanel.SetActive(false);
        categoryPanel.SetActive(true);
        insufficientFundsText.gameObject.SetActive(false);
        maxOrderExceededText.gameObject.SetActive(false);

        UpdateTotalText();
    }

    public void OnInteract(GameObject interactingObject)
    {
        _isInteracted = !_isInteracted;

        interactingObject.GetComponent<LouiseController>().SetController(!_isInteracted); //true ise karakter kontrolü aktif, false ise kilitli

        if (_isInteracted)
        {
            orderCamera.gameObject.SetActive(true);
            _mainCamera.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            orderCamera.gameObject.SetActive(false);
            _mainCamera.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void OpenFoodCategory()
    {
        if (!_isInteracted)
        {
            return;
        }
        categoryPanel.SetActive(false);
        productListPanel.SetActive(true);

        foodList.SetActive(true);
        drinkList.SetActive(false);
        snackList.SetActive(false); 
        AudioManager.Instance.PlayClickSfx();
    }

    public void OpenDrinkCategory()
    {
        if (!_isInteracted)
        {
            return;
        }
        categoryPanel.SetActive(false);
        productListPanel.SetActive(true);

        foodList.SetActive(false);
        drinkList.SetActive(true);
        snackList.SetActive(false);
        AudioManager.Instance.PlayClickSfx();
    }

    public void OpenSnackCategory()
    {
        if (!_isInteracted)
        {
            return;
        }
        categoryPanel.SetActive(false);
        productListPanel.SetActive(true);

        foodList.SetActive(false);
        drinkList.SetActive(false);
        snackList.SetActive(true);
        AudioManager.Instance.PlayClickSfx();
    }

    public void BackToCategoryPanel()
    {
        if (!_isInteracted)
        {
            return;
        }
        productListPanel.SetActive(false);
        categoryPanel.SetActive(true);
        UpdateTotalText();
        AudioManager.Instance.PlayClickSfx();
    }

    public void UpdateOrderSpecsList(GameObject productPrefab, int newAmount)
    {
        for (int i = 0; i < _orderSpecs.Count; i++)  // sepette bu ürün var mý diye tüm listeyi kontrol ediyoruz
        {
            if (_orderSpecs[i].productPrefab == productPrefab) // eþleþen ürünü bulduk
            {
                OrderSpecs spec = _orderSpecs[i]; //kopyayý alýyoruz
                spec.amount = newAmount;
                _orderSpecs[i] = spec; //kopyayý listeye geri yaz
                UpdateTotalText();
                return;
            }
        }

        OrderSpecs newSpec = new(); //ürün sepette yoksa yeni bir spec oluþturuyoruz
        newSpec.productPrefab = productPrefab; 
        newSpec.amount = newAmount;
        _orderSpecs.Add(newSpec);
        UpdateTotalText();
    }

    private float GetTotalCost()
    {
        float total = 0f;
        foreach (var spec in _orderSpecs) // sepetteki her ürünü tek tek geziyoruz
        {
            if (spec.amount > 0 && spec.productPrefab != null) //miktarý 0'dan büyük ve prefab null deðilse
            {
                total += spec.productPrefab.GetComponent<BaseProduct>().BuyPrice * spec.amount;
            }
        }
        return total;
    }
    private int GetTotalAmount()
    {
        int total = 0;
        foreach (var spec in _orderSpecs)
        {
            total += spec.amount;
        }
        return total;
    }
    private void UpdateTotalText()
    {
        totalText.text = GetTotalCost().ToString();
    }

    public void OnClickOrder()
    {
        if (!_isInteracted)
        {
            return;
        }
        float totalCost = GetTotalCost();

        if (totalCost <= 0f)
        {
            return; //hiçbir ürün seçilmemiþ, sepet boþ
        }

        int totalAmount = GetTotalAmount(); 
        int boxCapacity = ProductManager.Instance.GetBoxCapacity(); //sipariþ kutusunun gerçek kapasitesini al
        if (totalAmount > boxCapacity)
        {
            StartCoroutine(ShowMaxOrderExceeded()); //uyarý göster
            AudioManager.Instance.PlayMaxOrderExceededSfx();
            return;
        }
        //Sepet özetini oluþtur
        string summary = "";
        foreach (var spec in _orderSpecs)
        {
            if (spec.amount > 0) //miktarý 0dan büyük olanlarý al
            {
                BaseProduct product = spec.productPrefab.GetComponent<BaseProduct>();
                summary += spec.amount + "x " + product.ProductName + "\n"; // "Adet x ÜrünAdý" þeklinde alt alta yaz
            }
        }
        summary += "\nToplam: " + totalCost; //en alta toplam tutarý yaz
        cartSummaryText.text = summary;

        categoryPanel.SetActive(false);
        cartConfirmPanel.SetActive(true);
    }


    public void OnClickConfirmOrder()
    {
        if (!_isInteracted)
        {
            return;
        }
        float totalCost = GetTotalCost();

        if (!GameManager.Instance.IsCoinEnough(totalCost)) //para yetmiyor mu
        {
            StartCoroutine(ShowInsufficientFunds()); //yetmiyorsa uyarý göster
            AudioManager.Instance.PlayInsufficientFundsSfx();
            return;
        }

        GameManager.Instance.UpdateCoin(-totalCost);
        ProductManager.Instance.OrderProducts(_orderSpecs);
       
        AudioManager.Instance.PlayOrderConfirmSfx(); 
        AudioManager.Instance.PlayMoneySfx(); 

        StartCoroutine(ShowOrderReceivedThenReset());
    }

    private IEnumerator ShowOrderReceivedThenReset()
    {
        cartConfirmPanel.SetActive(false);
        orderReceivedText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        orderReceivedText.gameObject.SetActive(false);

        //sepeti ve tüm ürün panellerinin sayaçlarýný sýfýrla
        _orderSpecs.Clear();
        foreach (var panel in allProductPanels)
        {
            panel.ResetAmount();
        }

        UpdateTotalText();
        categoryPanel.SetActive(true);
    }
    public void BackFromCartConfirm()
    {
        if (!_isInteracted)
        {
            return;
        }
        cartConfirmPanel.SetActive(false);
        categoryPanel.SetActive(true);
        AudioManager.Instance.PlayClickSfx();
    }
    private IEnumerator ShowInsufficientFunds() //yetersiz bakiye
    {
        insufficientFundsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        insufficientFundsText.gameObject.SetActive(false);
    }
    private IEnumerator ShowMaxOrderExceeded() //kutu kapasitesinden fazla sipariþ
    {
        maxOrderExceededText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        maxOrderExceededText.gameObject.SetActive(false);
    }
}
