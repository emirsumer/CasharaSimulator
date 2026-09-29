using UnityEngine;

public interface IAttachable
{
    public void OnAttach(GameObject attachObject);
    public void OnDetach(GameObject detachObject);
}
