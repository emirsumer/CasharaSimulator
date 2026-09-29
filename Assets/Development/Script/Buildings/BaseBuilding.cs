using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseBuilding : MonoBehaviour
{
    [SerializeField] private List<Renderer> meshRenderer;
    private List<GameObject> _triggeredObject = new ();
    private List<Color> _originalColors = new List<Color>();
    protected bool isPlaced = false; 
    protected virtual void Awake()
    {
        foreach (var m in meshRenderer)
        {
            _originalColors.Add(m.material.color);
        }
    }
    protected virtual void Start()
    {
        if (!isPlaced)
        {
            SetColliders(false);
            SetObstacle(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _triggeredObject.Add(other.gameObject); 
    }
    private void OnTriggerExit(Collider other)
    {
        _triggeredObject.Remove(other.gameObject);
    }
    public bool CalculatePlacement()
    {
        if (_triggeredObject.Count > 0)
        {
            foreach (var m in meshRenderer)
            {
                m.material.color = Color.red;
            }
            return false; 
        }
        else
        {
            foreach (var m in meshRenderer)
            {
                m.material.color = Color.green;
            }
            return true;
        }
    }
    public void OnBuildingPlaced()
    {
        isPlaced = true;
        for (int i = 0; i < meshRenderer.Count; i++)
        {
            meshRenderer[i].material.color = _originalColors[i];
        }
        SetColliders(true);
        SetObstacle(true);
    }
    private void SetColliders(bool enable)
    {
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if (!col.isTrigger)
            {
                col.enabled = enable;
            }
        }
    }
    private void SetObstacle(bool enable)
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();

        if (obstacle != null)
        {
            obstacle.enabled = enable;
        }
    }
}
