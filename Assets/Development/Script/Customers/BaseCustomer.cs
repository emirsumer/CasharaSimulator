using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using DG.Tweening;
public class BaseCustomer : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private Transform productTransform;
    [SerializeField] private Animator animator;

    private BaseTask _activeTask; 
    private List<BaseProduct> _ownedProducts = new();
    public Action OnCustomerExit;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.avoidancePriority = UnityEngine.Random.Range(10, 90);
        }
        DeclareState();
    }

    private void Update()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed",speed);
    }

    private void DeclareState()
    {
        int shelfCount = GameManager.Instance.GetAllShelves().Count;
        int cashCount = GameManager.Instance.GetAllCashes().Count;

        if (_ownedProducts.Count > 0)
        {
            if (cashCount > 0)
            {
                int random = UnityEngine.Random.Range(0, 2);

                if (random == 0)
                {
                    StartTask<Task_Cash>(); 
                }
                else
                {
                    StartTask<Task_Patrol>();
                }
                return;
            }
        }
        if (shelfCount > 0)
        {
            int randomNumber = UnityEngine.Random.Range(0, 3);

            if (randomNumber == 0)
            {
                StartTask<Task_Shelf>();
            }
            else
            {
                StartTask<Task_Patrol>();
            }
            return;
        }
        StartTask<Task_Patrol>();
    }

    public void AddProductToList(Action isFinished,BaseProduct product) //hangi productu listeye ekliyorsak bilgsini göndermemiz lazým
    {
        product.transform.DOKill();// ürün üzerinde çalýþan önceki tüm tween animasyonlarýný durdur

        product.transform.SetParent(productTransform, true);
        product.transform.localRotation = Quaternion.identity;

        float productHeight = 0.2f;
        Vector3 desiredPosition = new Vector3(0, _ownedProducts.Count * productHeight, 0);

        product.transform.DOLocalJump(desiredPosition, jumpPower: 1f, numJumps: 1, duration: 0.4f)
            .OnComplete(() =>
            {
                product.transform.localPosition = desiredPosition; //anim bittiðinde posizyonu sabitle
                product.transform.localRotation = Quaternion.identity;

                isFinished?.Invoke(); // Ürün müþterinin eline geldiðinde eventi tetikle
            });
        _ownedProducts.Add(product);
    }
    public void StartTask<T>() where T : BaseTask //T yi BaseTask sýnýfýndan türemiþ bir class olarak tanýmladýk, böylece T yerine hangi classý yazarsak o class BaseTask sýnýfýndan türemiþ olmasý gerekir
    {
        _activeTask = gameObject.AddComponent<T>(); //istediðim T sýnýfýný gameobjecte ekler
        _activeTask.StartTask(this); //hangi task çalýþacaksa onu baþlatýr

    }
    public IEnumerator TaskComplete(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeclareState();
    }

    public IEnumerator TaskComplete<T>(float delay) where T : BaseTask
    {
        yield return new WaitForSeconds(delay);
        StartTask<T>();
    }
    public Vector3 GetRandomPointOnNavMesh(float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randPoint = UnityEngine.Random.insideUnitSphere * radius; //rastgele bir yön seç
            randPoint += transform.position;

            NavMeshHit hit;
            int areaIndex = NavMesh.GetAreaFromName("Inside");
            int targetAreaMask = 1 << areaIndex; //maskeleme iþlemi yap

            if (NavMesh.SamplePosition(randPoint, out hit, radius, targetAreaMask)) //alanda geçerli navmesh noktasý var mý
            {
                return hit.position; //geçerli noktayý döndür
            }
        }
        return transform.position;
    }
    public void CompletePurchase()
    {
        float totalEarned = 0f; //müþterinin sahip olduðu ürünlerden kazanýlacak toplam parayý tutacak deðiþken

        for (int i = _ownedProducts.Count - 1; i >= 0; i--)
        {
            totalEarned += _ownedProducts[i].SellPrice; //ürün yok edilmeden önce satýþ fiyatýný topluyoruz
            Destroy(_ownedProducts[i].gameObject);
        }
        _ownedProducts.Clear();

        GameManager.Instance.UpdateCoin(totalEarned);
        AudioManager.Instance.PlayAiPay(); 
        AudioManager.Instance.PlayMoneySfx(); 

        Task_Cash cashTask = _activeTask as Task_Cash; //aktif görevin Kasa görevi olup olmadýðýný kontrol et
        if (cashTask)
        {
            cashTask.CompletePurchase();
        }
    }
}
