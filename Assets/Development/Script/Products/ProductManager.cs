using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct OrderSpecs
{
    public GameObject productPrefab;
    public int amount;
}
public class ProductManager : MonoBehaviour
{
    [SerializeField] private Transform orderTransform;
    [SerializeField] private GameObject productBoxPrefab;
    public static ProductManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public void OrderProducts(List<OrderSpecs> orderSpecsList)
    {
        GameObject productBox = Instantiate(productBoxPrefab, orderTransform.position, orderTransform.rotation);  
        Product_Box pdb = productBox.GetComponent<Product_Box>();
        pdb.FillTheBox(orderSpecsList);
    }
    public GameObject SpawnProduct(GameObject prefab)
    {
        return Instantiate(prefab);
    }
    public int GetBoxCapacity()
    {
        return productBoxPrefab.GetComponent<Product_Box>().SlotCount;
    }
}
