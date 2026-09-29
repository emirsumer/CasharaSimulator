using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> customerPrefabs = new();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private int maxCustomerCount = 8;

    private List<BaseCustomer> _activeCustomers = new();
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            bool canSpawn = _activeCustomers.Count < maxCustomerCount;
            bool hasList = customerPrefabs.Count > 0;
           
            if (canSpawn && hasList)
            {
                SpawnCustomer();
            }
        }
    }

    private void SpawnCustomer()
    {
        int randomIndex = Random.Range(0, customerPrefabs.Count);
        GameObject selectedPrefab = customerPrefabs[randomIndex];

        GameObject customerObject = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
        BaseCustomer customer = customerObject.GetComponent<BaseCustomer>();

        _activeCustomers.Add(customer);

        customer.OnCustomerExit += () =>
        {
            _activeCustomers.Remove(customer);
        };
    }
}
