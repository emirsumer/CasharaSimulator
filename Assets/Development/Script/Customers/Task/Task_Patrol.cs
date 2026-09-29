using System.Collections;
using UnityEngine;

public class Task_Patrol : BaseTask
{
    public override void StartTask(BaseCustomer targetCustomer)
    {
        base.StartTask(targetCustomer);
    }
    public override IEnumerator TaskAction()
    {
        Vector3 randomPoint = _targetCustomer.GetRandomPointOnNavMesh(25);
        _targetCustomer.agent.SetDestination(randomPoint);

        float distance = Vector3.Distance(_targetCustomer.transform.position, randomPoint);
        while (distance > 1f)
        {
            distance = Vector3.Distance(_targetCustomer.transform.position, randomPoint);
            yield return new WaitForEndOfFrame(); 
        }
        int randomDelay = Random.Range(1, 3);
        StopTask(randomDelay);
    }
    public override void StopTask(float delay)
    {
        base.StopTask(delay);
    }
}