using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<BaseBuilding> _placedBuildings = new();
   
    public static GameManager Instance;
    public Transform exitTransform;

    [SerializeField] private float totalCoin;
    public Action<float> OnCoinUpdated;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        BuildingManager.Instance.OnBuildingPlaced += ListenOnBuildingPlace; 
        OnCoinUpdated?.Invoke(totalCoin); // Oyun baþladýðýnda mevcut parayý UI'a bildir
        AudioManager.Instance.PlayGameMusic();
    }
    private void OnDisable()
    {
        BuildingManager.Instance.OnBuildingPlaced -= ListenOnBuildingPlace;
    }
    private void ListenOnBuildingPlace(BaseBuilding placeBuilding)
    {
        _placedBuildings.Add(placeBuilding);
    }

    public List<Shelf> GetAllShelves()
    {
        List<Shelf> results = new();
        foreach (var b in _placedBuildings)
        {
            if (b is Shelf) // nesnenin türü Shelf mi diye kontrol et
            {
                results.Add(b as Shelf); // shelf sýnýfýna dönüþtürerek listeye ekle
            }
        }
        return results;
    }

    public Shelf GetRandomShelf()
    {
        List<Shelf> shelves = GetAllShelves();
        if (shelves.Count == 0)
        { 
            return null; 
        }
        return shelves[UnityEngine.Random.Range(0, shelves.Count)];
    }

    public List<Cash> GetAllCashes()
    {
        List<Cash> results = new();
        foreach (var b in _placedBuildings)
        {
            if (b is Cash) // nesnenin türü Cash mi diye kontrol et
            {
                results.Add(b as Cash); // cash sýnýfýna dönüþtürerek listeye ekle
            }
        }
        return results;
    }

    public Cash GetRandomCash()
    {
        List<Cash> cashes = GetAllCashes();
        return cashes[UnityEngine.Random.Range(0, cashes.Count)];
    }
    public void UpdateCoin(float amount)
    {
        totalCoin += amount;
        OnCoinUpdated?.Invoke(totalCoin);
    }

    public bool IsCoinEnough(float amount)
    {
        return totalCoin >= amount;
    }
}
