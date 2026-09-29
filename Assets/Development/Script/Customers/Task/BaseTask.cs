using System.Collections;
using UnityEngine;

public abstract class BaseTask : MonoBehaviour
{
    protected BaseCustomer _targetCustomer;
    public virtual void StartTask(BaseCustomer targetCustomer)
    {
        _targetCustomer = targetCustomer;
        StartCoroutine(TaskAction());
    }
    public abstract IEnumerator TaskAction();
    public virtual void StopTask(float delay)
    {
        _targetCustomer.StartCoroutine(_targetCustomer.TaskComplete(delay));
        Destroy(this,delay);
    }
}