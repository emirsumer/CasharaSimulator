using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product_Box : MonoBehaviour, IInteractable,IAttachable
{
    [SerializeField] private List<SlotSpecs> slotSpecs;
    [SerializeField] private List<Collider> colliders;
    public int SlotCount => slotSpecs.Count;

    private Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public void FillTheBox(List<OrderSpecs> orders)
    {
        int tempIndex = 0; 
        for (int i = 0; i < orders.Count; i++)//gelen sipariþ listesindeki her bir sipariþi dön
        {
            for (int j = 0; j < orders[i].amount; j++)//O sipariþin adet miktarý kadar döngü çalýþtýr
            {
                if (tempIndex >= slotSpecs.Count)
                {
                    return;//kutu dolarsa taþmayý engelle
                }

                GameObject spawnedObject = ProductManager.Instance.SpawnProduct(orders[i].productPrefab);

                spawnedObject.transform.position = slotSpecs[tempIndex].slotTransform.position;
                spawnedObject.transform.rotation = slotSpecs[tempIndex].slotTransform.rotation;
                spawnedObject.transform.parent = slotSpecs[tempIndex].slotTransform;

                SlotSpecs newSpecs = new();
                newSpecs.slotTransform = slotSpecs[tempIndex].slotTransform;
                newSpecs.slotItem = spawnedObject.GetComponent<BaseProduct>();

                slotSpecs[tempIndex] = newSpecs;
                tempIndex++;
            }
        }
    }
    public void OnInteract(GameObject interactingObject)
    {
        _rb.isKinematic = true;
        LouiseController louiseController = interactingObject.GetComponent<LouiseController>();
        
        if (louiseController != null)
        {
            louiseController.AttachItem(gameObject); 
            AudioManager.Instance.PlayProductPickup();
        }
    
        foreach (var c in colliders)
        {
            c.enabled = false;
        }
    }
    public void OnDetach(GameObject detachActor)
    {
        foreach (var c in colliders)
        {
            c.enabled = true;
        }

        _rb.isKinematic = false;
        _rb.AddForce(detachActor.transform.forward * 15f , ForceMode.Impulse);
        AudioManager.Instance.PlayProductDrop();
        
        if (IsBoxEmpty())
        {
            StartCoroutine(DestroyBoxRoutine());
        }
    }
    public bool IsBoxEmpty()
    {
        foreach (var slot in slotSpecs)
        {
            if (slot.slotItem != null)
            {
                return false; // En az 1 slotta ürün varsa boþ deðil
            }
        }
        return true; // Bütün slotlar boþ
    }
    private IEnumerator DestroyBoxRoutine()
    {
        yield return new WaitForSeconds(3f); 
        Destroy(gameObject);      
    }
    void IAttachable.OnAttach(GameObject attachObject)
    {

    }
   public List<SlotSpecs> GetSlotSpecs()
    {
        return slotSpecs;
    }
}
