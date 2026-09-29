using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseQueueBuilding : BaseBuilding
{
    [SerializeField] protected Vector3 offset;
    [SerializeField] protected Transform customerTransform;
    public Transform CustomerTransform => customerTransform;
    private List<BaseCustomer> _queueList = new();

    public Action<BaseCustomer> OnCustomerLeft;

    protected override void Start()
    {
        base.Start();
    }

    public void AddCustomerToQueue(BaseCustomer targetCustomer)
    {
        _queueList.Add(targetCustomer);
    }

    public void RemoveCustomerFromQueue(BaseCustomer targetCustomer)
    {
        _queueList.Remove(targetCustomer);
        OnCustomerLeft?.Invoke(targetCustomer);
    }

    public Vector3 GetQueuePosition(int targetIndex = -1)
    {
        Vector3 direction = (customerTransform.forward * offset.z) + (customerTransform.right * offset.x) + (customerTransform.up * offset.y); //kuyruktaki yön ve konum

        if (targetIndex == -1)
        {
            if (_queueList.Count == 0)
            {
                return customerTransform.position;
            }
            else //müþteri varsa
            {
                int listCount = _queueList.Count; //kuyrukta kaç müþteri olduðunu sayýyoruz
                Vector3 desiredPosition = customerTransform.position + direction * listCount;
                return desiredPosition;
            }
        }
        else //dýþarýdan özel bir sýra numarasý verilirse
        {
            Vector3 desiredPosition = customerTransform.position + direction * targetIndex; 
            return desiredPosition;
        }
    }

    public int FindIndex(BaseCustomer targetCustomer)
    {
        return _queueList.IndexOf(targetCustomer); //müþterinin kuyruktaki sýrasýný bulur (bulamazsa -1 döndürür)
    }

    public BaseCustomer GetFirstElementInList()
    {
        if (_queueList.Count == 0)
        {
            return null;
        }
        return _queueList[0];
    }
}