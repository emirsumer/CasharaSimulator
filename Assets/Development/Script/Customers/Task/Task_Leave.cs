using System.Collections;
using UnityEngine;

public class Task_Leave : BaseTask
{
    public override void StartTask(BaseCustomer targetCustomer)
    {
        base.StartTask(targetCustomer);
    }
    public override IEnumerator TaskAction()
    {
        Vector3 targetPos = GameManager.Instance.exitTransform.position;
        float distance = Vector3.Distance(transform.position,targetPos);
        while(distance > 1f)
        {
            _targetCustomer.agent.SetDestination(targetPos);
            distance = Vector3.Distance(transform.position, targetPos);
            yield return new WaitForEndOfFrame();
        }
        _targetCustomer.OnCustomerExit?.Invoke();
        Destroy(gameObject);
    }
    public override void StopTask(float delay)
    {
        base.StopTask(delay);
    }
}
