using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

enum OpenAnimation {HorizontalFlip, VerticalFlip}

public class Card : MonoBehaviour
{
    public bool bFaceUp { get; set; }

    [SerializeField] private SpriteRenderer _faceImage;
    [SerializeField] private GameObject _backImageGameObject;
    [SerializeField] private float _animationTime;
    [SerializeField] private OpenAnimation _openAnimation;
    
    void Start()
    {
        bFaceUp = false;
    }

    /// <summary>
    /// Sets texture to sprite of Face Image
    /// </summary>
    /// <param name="tex">Texture to set</param>
    public void SetFaceImage(Texture2D tex)
    {
        _faceImage.sprite = Sprite.Create(tex, 
            new Rect(0.0f, 0.0f, tex.width, tex.height), 
            new Vector2(0.5f, 0.5f), 
            100.0f);
    }

    /// <summary>
    /// Flips card to the opposite side
    /// </summary>
    /// <returns></returns>
    public IEnumerator Flip()
    {
        bFaceUp = !bFaceUp;
        //animation
        Tween tween;
        if (_openAnimation == OpenAnimation.HorizontalFlip)
        {
            tween = transform.DORotate(Vector3.up * 90, 0.5f * _animationTime);
            yield return tween.WaitForKill();
        }
        else if (_openAnimation == OpenAnimation.VerticalFlip)
        {
            tween = transform.DORotate(Vector3.right * 90, 0.5f * _animationTime);
            yield return tween.WaitForKill();
        }
        _backImageGameObject.SetActive(!_backImageGameObject.activeSelf);
        tween = transform.DORotate(Vector3.zero, 0.5f * _animationTime);
        yield return tween.WaitForKill();
    }
}
