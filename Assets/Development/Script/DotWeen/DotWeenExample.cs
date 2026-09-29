using DG.Tweening;
using UnityEngine;

public class DotWeenExample : MonoBehaviour
{
    public Transform sphereTransform;

    private void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOJump(sphereTransform.position, 2, 3, 4));
        mySequence.Append(transform.DORotateQuaternion(Quaternion.Euler(0,250,0), 2));
        mySequence.Append(transform.DOShakeScale(5, new Vector3(3,1,3), 10, 180));
        mySequence.AppendInterval(1);//bekleme süresi ekler
        mySequence.Append(transform.DOScale(Vector3.zero, 2));

        //transform.DOShakeScale(5, 1.5f, 25, 180).SetEase(Ease.InOutCubic);

        //transform.DOMove(sphereTransform.position, 10).SetEase(Ease.InOutCubic).OnUpdate(() =>
        //{
        //    // Her güncellemede yapýlacak iþlemler buraya yazýlýr
        //    Debug.Log("Animasyon güncelleniyor! Mevcut pozisyon: " + transform.position);
        //}).OnComplete(() =>
        //{
        //    // Animasyon tamamlandýðýnda yapýlacak iþlemler buraya yazýlýr
        //    Debug.Log("Animasyon tamamlandý! Son pozisyon: " + transform.position);
        //}).OnStart(() =>
        //{
        //    // Animasyon baþladýðýnda yapýlacak iþlemler buraya yazýlýr 1 kere çalýþýr
        //    Debug.Log("Animasyon baþladý! Baþlangýç pozisyonu: " + transform.position);
        //});

        //transform.DOMove(sphereTransform.position, 5).SetEase(Ease.InOutCubic).SetLoops(-1,LoopType.Yoyo);

        //transform.DOMove(sphereTransform.position, 10).SetEase(Ease.InOutCubic).SetLoops(-1);
        //-1 yazdýgýmýzda sonsuz döngü yapar yani sürekli hareket eder normal sayý yazarsak yazdýgýmýz sayý kadar tekrar eder


        //Easing kullanmak istersek mesela hýzlý bþalayýp yavaþ biter veya yavaþtan hýzlýya geçmek istersek bu þekilde kullanabiliriz
        




        //transform.DOJump(sphereTransform.position, 2, 3, 4);
        //gitmek istediðimiz konumu, zýplama gücü, zýplama sayýsý, hareket süresi



        //transform.DOMove(sphereTransform.position, 10);
        //Fonksiyon nereye gitmek istediðimi ve ne kadar sürede gitmek istediðini soruyor
        //Kendi içinde coroutine yapýyor biz yapmýyoruz yani update kullanmadan hareket saðlýyoruz
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, sphereTransform.position,15 * Time.deltaTime);
        //bu kodda küp küreye doðru hareket edecek bu normal kod
        //bunu dotween ile bunu çok daha basit bir þekilde ve update kullanmadan yapabiliriz
    }
}


