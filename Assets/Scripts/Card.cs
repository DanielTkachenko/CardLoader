using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    public bool bFaceUp { get; set; }

    [SerializeField] private SpriteRenderer _faceImage;
    [SerializeField] private GameObject _backImageGameObject;
    
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
        //animation
        Tween tween = transform.DORotate(Vector3.up * 90, 0.5f);
        yield return tween.WaitForKill();
        _backImageGameObject.SetActive(!_backImageGameObject.activeSelf);
        tween = transform.DORotate(Vector3.zero, 0.5f);
        yield return tween.WaitForKill();
        bFaceUp = !bFaceUp;
    }
}
