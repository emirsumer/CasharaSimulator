using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class BaseProduct : MonoBehaviour
{
    [SerializeField] protected string productName;   
    [SerializeField] protected Sprite icon;          
    [SerializeField] protected float buyPrice;   
    [SerializeField] protected float sellPrice;
    public string ProductName => productName;
    public Sprite Icon => icon;
    public float BuyPrice => buyPrice;
    public float SellPrice => sellPrice;

    private Tween _jumpTween;
    private Tween _rotateTween;

    public void PlaceToShelf(Transform endTransform, float startDelay, float duration)
    {

        _jumpTween = transform.DOJump(endTransform.position, 1, 1, 1).SetDelay(startDelay);
        //Zýplamasýný istediðim yer,zýplama gücü,zýplama sayýsý,zýplama süresi
        _rotateTween = transform.DORotateQuaternion(endTransform.rotation, 1);
        transform.parent = endTransform;
    }
    public void KillTweens()
    {
        _jumpTween?.Kill();
        _rotateTween?.Kill();
    }

}
