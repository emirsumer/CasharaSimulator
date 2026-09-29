using System;
using UnityEngine;

public class Cash : BaseQueueBuilding
{
    [SerializeField] private CashScreen cashScreen;

    public Action OnPurchaseComplete;

    protected override void Start()
    {
        base.Start();
        cashScreen.TargetCash = this;
    }
    private void OnEnable()
    {
        OnPurchaseComplete += ListenOnPurchaseComplete;
    }
    private void OnDisable()
    {
        OnPurchaseComplete -= ListenOnPurchaseComplete;
    }
    public void SetAvailability(bool isAvailable)
    {
        cashScreen.SetCashAvailability(isAvailable);
    }
    private void ListenOnPurchaseComplete()
    {
        BaseCustomer target = GetFirstElementInList();
        target.CompletePurchase();
    }
}
