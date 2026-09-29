using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Task_Cash : BaseTask
{
    private Cash _targetCash;
    private Coroutine _moveCoroutine; //hareket coroutineni takip etmek için

    public void GenerateEvents()
    {
        if (_targetCash != null)
        {
            _targetCash.OnCustomerLeft += ListenOnCustomerLeft;
        }
    }

    private void OnDisable()
    {
        if (_targetCash != null)
        {
            _targetCash.OnCustomerLeft -= ListenOnCustomerLeft;
        }
    }

    private void ListenOnCustomerLeft(BaseCustomer customer)
    {
        if (_targetCustomer == customer)
        {
            StopTask(0);
        }
        else // Ayrýlan baþka bir müþteriyse sýrasýný güncelle
        {
            int index = _targetCash.FindIndex(_targetCustomer);
            if (index != -1)
            {
                Vector3 newPos = _targetCash.GetQueuePosition(index);
                
                if (_moveCoroutine != null)
                {
                    StopCoroutine(_moveCoroutine);
                }
                _moveCoroutine = StartCoroutine(MovePosition(newPos));
            }
        }
    }

    public override void StartTask(BaseCustomer targetCustomer)
    {
        base.StartTask(targetCustomer);
    }

    public override IEnumerator TaskAction()
    {
        _targetCash = GameManager.Instance.GetRandomCash();
        Vector3 targetPos = _targetCash.GetQueuePosition();

        GenerateEvents();

        _targetCash.AddCustomerToQueue(_targetCustomer);
        yield return new WaitForEndOfFrame();
        _moveCoroutine = StartCoroutine(MovePosition(targetPos));
    }

    private IEnumerator MovePosition(Vector3 targetPos)
    {
        float distance = Vector3.Distance(_targetCustomer.transform.position, targetPos);
        while (distance > 0.3f)
        {
            _targetCustomer.agent.SetDestination(targetPos);
            distance = Vector3.Distance(_targetCustomer.transform.position, targetPos);
            yield return new WaitForEndOfFrame();
        }
        _targetCustomer.transform.position = targetPos;

        Quaternion lookRot = Quaternion.LookRotation(-_targetCash.CustomerTransform.right);
        while (_targetCustomer.transform.rotation != lookRot)
        {
            _targetCustomer.transform.rotation = Quaternion.RotateTowards(_targetCustomer.transform.rotation, lookRot, 300 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(CheckQueueAction());
    }
    private IEnumerator CheckQueueAction()
    {
        int customerIndex = _targetCash.FindIndex(_targetCustomer);
        if (customerIndex == 0)
        {
            _targetCash.SetAvailability(true);
        }
        yield return new WaitForEndOfFrame();
    }
    public void CompletePurchase()
    {
        _targetCash.RemoveCustomerFromQueue(_targetCustomer);
    }
    public override void StopTask(float delay)
    {
        _targetCustomer.StartCoroutine(_targetCustomer.TaskComplete<Task_Leave>(delay));
        Destroy(this, delay);
    }
}
