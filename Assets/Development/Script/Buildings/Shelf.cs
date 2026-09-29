using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : BaseQueueBuilding, IInteractable
{
    [SerializeField] private List<SlotSpecs> slotSpecs;
    public void OnInteract(GameObject interactingObject)
    {
        if (!isPlaced)
        {
            return;
        }

        LouiseController lc = interactingObject.GetComponent<LouiseController>();

        if (lc != null)
        {
            GameObject attachedItem = lc.AttachedItem;
            if (attachedItem != null)
            {
                Product_Box box = attachedItem.GetComponent<Product_Box>();
                if (box != null)
                {
                    AddItemsToShelf(box.GetSlotSpecs());
                }
            }
        }
    }

    private void AddItemsToShelf(List<SlotSpecs> itemSpecs)
    {
        int counter = 0;

        for (int i = 0; i < slotSpecs.Count; i++)
        { 
            if (slotSpecs[i].slotItem == null)
            {
                for (int j = 0; j < itemSpecs.Count; j++) //elimizde itemlere bakýyoruz
                {
                    BaseProduct item= itemSpecs[j].slotItem;

                    if (item != null)
                    {
                        SlotSpecs newSpecs = new SlotSpecs();
                        newSpecs.slotTransform = slotSpecs[i].slotTransform;
                        newSpecs.slotItem = item;

                        SlotSpecs boxSpecs = new SlotSpecs();
                        boxSpecs.slotTransform = itemSpecs[j].slotTransform;
                        boxSpecs.slotItem = null;
                      
                        item.PlaceToShelf(newSpecs.slotTransform, counter * 0.25f, 1);
                        DOVirtual.DelayedCall(counter * 0.25f, () =>
                        {
                            AudioManager.Instance.PlayShelfPlace();
                        });

                        slotSpecs[i] = newSpecs;
                        itemSpecs[j] = boxSpecs;
                        counter++;
                        break;
                    }
                }
                    
            }
          
        }
    }
    public void RemoveItemFromShelf(BaseProduct targetProduct)
    {
        for (int i = 0; i < slotSpecs.Count; i++)
        {
            if (slotSpecs[i].slotItem == targetProduct)
            {
                SlotSpecs newSpecs = new();
                newSpecs.slotTransform = slotSpecs[i].slotTransform;
                newSpecs.slotItem = null;

                slotSpecs[i] = newSpecs; 
                break;
            }
        }
    }
    public BaseProduct GetProductFromList()
    {
        BaseProduct targetProduct = null;
        for (int i = 0; i < slotSpecs.Count; i++)
        {
            if (slotSpecs[i].slotItem != null)
            {
                targetProduct = slotSpecs[i].slotItem;
                break;
            }
        }
        return targetProduct;
    }
}
