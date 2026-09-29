using UnityEngine;

public class CashScreen : MonoBehaviour,IInteractable
{
    private Renderer _renderer;
    private bool _isAvaliable;

    private Cash _targetCash;
    public Cash TargetCash
    {
        get => _targetCash;
        set => _targetCash = value;
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        SetCashAvailability(false);
    }


    public void OnInteract(GameObject interactingObject)
    {
        if (!_isAvaliable)
        {
            return;
        }
        _targetCash.OnPurchaseComplete?.Invoke();
        SetCashAvailability(false);

    }

    public void SetCashAvailability(bool isAvailable)
    {
        _isAvaliable = isAvailable;
        if (isAvailable)
        {
            _renderer.material.color = Color.green;
        }
        else
        {
            _renderer.material.color = Color.red;
        }
    }
}
