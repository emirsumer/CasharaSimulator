using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject shelfPrefab;
    public GameObject cashPrefab;

    public static BuildingManager Instance;

    public Action<BaseBuilding>OnBuildingPlaced; //building inþa edildiðinde tetiklenen ve inþa edilen objeyi bildiren event

    [HideInInspector] public bool isBuildingModeActive = false;
    [HideInInspector] public GameObject spawnedObject;

    private bool _canPlaceable;
    private Camera _cam;
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
        _cam = Camera.main;
    }
    private void Update()
    {
        if (isBuildingModeActive)
        {
            if (spawnedObject)
            {
                if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, 150f))
                {
                    spawnedObject.transform.position = hit.point;
                }
                Debug.DrawRay(_cam.transform.position, _cam.transform.forward * 150f, Color.red);
                _canPlaceable = spawnedObject.GetComponent<BaseBuilding>().CalculatePlacement();//spawn edilen objenin yerleþtirilebilir olup olmadýðýný kontrol eder
            }
        }
    }
    public void PlaceBuilding() //uygunsa binayý inþa eder ve süreci tamamlar
    {
        if (_canPlaceable && spawnedObject != null)
        {
            BaseBuilding placeBuilding = spawnedObject.GetComponent<BaseBuilding>(); //spawn edilen objenin BaseBuilding bileþenini alýyoruz
            placeBuilding.OnBuildingPlaced();
            OnBuildingPlaced?.Invoke(placeBuilding);//OnBuildingPlaced eventini tetikleyerek, objenin yerleþtirildikten sonra yapýlacak iþlemleri diðer scriptlere bildireceðiz.
            AudioManager.Instance.PlayBuildingPlace();

            spawnedObject = null;
            isBuildingModeActive = false;
            _canPlaceable = false;
        }
    }
    public void CancelBuilding()
    {
        if (spawnedObject)
        {
            Destroy(spawnedObject);
            isBuildingModeActive = false;
        }
    }
    public void RotateBuilding(bool isRight)
    {
        if (spawnedObject)
        {
            int multiplier = isRight ? 1 : -1;
            spawnedObject.transform.Rotate(Vector3.up * 15 * multiplier);
        }
    }
    public void ActivateBuildingMode()
    {
        isBuildingModeActive = !isBuildingModeActive;
    }
    public void OnBuildingSelected(BaseBuilding selectedBuilding)
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }
        spawnedObject = selectedBuilding.gameObject;
    }
}
