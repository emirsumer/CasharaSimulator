using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LouiseController louiseController;
    [SerializeField] private Vector2 mouseSensitivity; 

    public static InputManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += (ctx) =>
        {
            louiseController.MoveCharacter(ctx.ReadValue<Vector2>(), true);

        };

        playerInput.actions["Move"].canceled += (ctx) =>
        {
            louiseController.MoveCharacter(ctx.ReadValue<Vector2>(), false);

        };

        //playerInput.actions["Look"].performed += (ctx) =>
        //{
        //    louiseController.LookCharacter(ctx.ReadValue<Vector2>(), mouseSensitivity);

        //};

        playerInput.actions["Build"].performed += (ctx) =>
        {
            BuildingManager.Instance.ActivateBuildingMode();
        }
        ;

        playerInput.actions["Cash"].performed += (ctx) =>
        {
            if (!BuildingManager.Instance.isBuildingModeActive)
            {
                return;
            }  
            
            GameObject obj = Instantiate(BuildingManager.Instance.cashPrefab);
            BaseBuilding bb = obj.GetComponent<BaseBuilding>();               
            BuildingManager.Instance.OnBuildingSelected(bb);
        };

        playerInput.actions["Shelf"].performed += (ctx) =>
        {
            if (!BuildingManager.Instance.isBuildingModeActive)
            {
                return;
            }
            GameObject obj = Instantiate(BuildingManager.Instance.shelfPrefab);
            BaseBuilding bb = obj.GetComponent<BaseBuilding>();
            BuildingManager.Instance.OnBuildingSelected(bb);
        };

        playerInput.actions["RotateBuilding"].performed += (ctx) =>
        {
            if (!BuildingManager.Instance.isBuildingModeActive)
            {
                return;
            }

            bool isRight = ctx.ReadValue<Vector2>().y >= 0 ? true : false;
            BuildingManager.Instance.RotateBuilding(isRight);
        };


        playerInput.actions["PlaceBuilding"].performed += (ctx) =>
        {
            BuildingManager.Instance.PlaceBuilding();
        };

        playerInput.actions["CancelBuilding"].performed += (ctx) =>
        {
            BuildingManager.Instance.CancelBuilding();
        };

        playerInput.actions["Interact"].performed += (ctx) =>
        {
            louiseController.SendRaycast();
        };

        playerInput.actions["Pause"].performed += (ctx) =>
        {
            PauseManager.Instance.TogglePause();
        };
    }
}
