using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Task_Shelf : BaseTask
{
    private Shelf _targetShelf;
    private Coroutine _moveCoroutine;

    public override void StartTask(BaseCustomer targetCustomer)
    {
        base.StartTask(targetCustomer);
    }

    public override void StopTask(float delay)
    {
        base.StopTask(delay);
    }

    public void GenerateEvents()
    {
        _targetShelf.OnCustomerLeft += ListenOnCustomerLeft;
    }
    private void OnDisable()
    {
        if (_targetShelf != null)
        {
            _targetShelf.OnCustomerLeft -= ListenOnCustomerLeft;
        }
    }

    private void ListenOnCustomerLeft(BaseCustomer customer)
    {
        if (_targetCustomer == customer)
        {
            StopTask(0);
        }
        else
        {
            int index = _targetShelf.FindIndex(_targetCustomer);
            if (index != -1) 
            {
                Vector3 newPos = _targetShelf.GetQueuePosition(index);
                if (_moveCoroutine != null)
                {
                    StopCoroutine(_moveCoroutine);
                }
                _moveCoroutine = StartCoroutine(MovePosition(newPos));
            }
        }
    }

    public override IEnumerator TaskAction()
    {
        _targetShelf = GameManager.Instance.GetRandomShelf();
        Vector3 targetPos = _targetShelf.GetQueuePosition();
        
        GenerateEvents();
        
        _targetShelf.AddCustomerToQueue(_targetCustomer);

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

        Quaternion lookRot = Quaternion.LookRotation(-_targetShelf.CustomerTransform.forward);
        while (_targetCustomer.transform.rotation != lookRot)
        {
            _targetCustomer.transform.rotation = Quaternion.RotateTowards(_targetCustomer.transform.rotation, lookRot, 300 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(CheckQueueAction());
    }
    private IEnumerator CheckQueueAction()
    {
        int customerIndex = _targetShelf.FindIndex(_targetCustomer);

        if (customerIndex == 0)
        {
            BaseProduct targetProduct = CheckProductInShelf();
            if (targetProduct != null)
            {
                int randomCount = UnityEngine.Random.Range(1,4);
                StartCoroutine(CollectProducts(targetProduct, randomCount));
            }
            else 
            {
                StartCoroutine(WaitForProduct((waitedProduct) =>
                {
                    if (waitedProduct != null)
                    {
                        waitedProduct.KillTweens(); 
                        int randomCount = UnityEngine.Random.Range(1,4);
                        StartCoroutine(CollectProducts(waitedProduct, randomCount));
                    }
                    else //ürün gelmedi sýradan çýktý
                    {
                        _targetShelf.RemoveCustomerFromQueue(_targetCustomer);
                    }
                }, 5f));
            }
        }
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator CollectProducts(BaseProduct firstProduct, int desiredCount)
    {
        int collected = 0;
        BaseProduct currentProduct = firstProduct;
        while (currentProduct != null && collected < desiredCount) //mevcut ürün varsa ve toplanan miktar istenen sayýdan küçük olduðu sürece devam et
        {
            BaseProduct productToTake = currentProduct;
            bool isDone = false;

            _targetCustomer.AddProductToList(() =>
            {
                _targetShelf.RemoveItemFromShelf(productToTake);
                isDone = true;
                AudioManager.Instance.PlayAiTake();
            }, productToTake);

            while (!isDone) //ürün alýnana kadar bekle
            {
                yield return null;
            }

            collected++;

            if (collected < desiredCount)
            {
                currentProduct = CheckProductInShelf();
            }
        }
        _targetShelf.RemoveCustomerFromQueue(_targetCustomer);
    }
    private IEnumerator WaitForProduct(Action<BaseProduct> action, float duration) // parametre olarak bir Action alýyoruz. Bu metodu çaðýrdýðýmýz yerde lambda kullanarak bir kod bloðu gönderiyoruz ve ürün bulunduðunda veya süre bittiðinde bu kod bloðunu tetikliyoruz.
    {
        float timer = 0;
        BaseProduct waitedProduct = null;

        while (timer <= duration)
        {
            timer += Time.deltaTime;

            waitedProduct = CheckProductInShelf();
            if (waitedProduct != null)
            {
                action?.Invoke(waitedProduct);//ürün varsa bilgiyi gönderiyoruz 
                break;
            }
            yield return new WaitForEndOfFrame(); 
        }
        if (waitedProduct == null)
        {
            action?.Invoke(null); //ürün yok ise bir cevap gönderiyoruz
        }
    }
    private BaseProduct CheckProductInShelf()
    {
        BaseProduct targetProduct = _targetShelf.GetProductFromList();
        return targetProduct;
    }
}
