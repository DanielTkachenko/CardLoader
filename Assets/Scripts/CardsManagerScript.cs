using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;

public enum LoadMode {OneByOne, AllAtOnce, WhenReady}

public class CardsManagerScript : MonoBehaviour
{
    [SerializeField] private string _uri = "https://picsum.photos/1024";
    [SerializeField] private float _waitingTime = 1.5f;
    private List<Card> _cards;

    private LoadMode _loadMode;
    
    void Start()
    {
        _loadMode = LoadMode.OneByOne;
        _cards = new List<Card>();
        foreach (var item in GetComponentsInChildren<Card>())
        {
            _cards.Add(item);
        }
    }

    /// <summary>
    /// Loads and opens cards according to loading mode
    /// </summary>
    public void FlipCards()
    {
        switch (_loadMode)
        {
            case LoadMode.OneByOne:
                //one by one loading
                StartCoroutine(OneByOneLoading());
                break;
            case LoadMode.AllAtOnce:
                //all at once loading
                StartCoroutine(AllAtOnceLoading());
                break;
            case LoadMode.WhenReady:
                //when ready loading
                break;
        }
    }

    /// <summary>
    /// Loads an image from uri
    /// </summary>
    /// <param name="uri">uri to load</param>
    /// <param name="card">card component that gets the image</param>
    /// <returns></returns>
    private IEnumerator LoadImage(string uri, Card card)
    {
        UnityWebRequest web = UnityWebRequestTexture.GetTexture(uri);
        yield return web.SendWebRequest();
        Texture2D tex = DownloadHandlerTexture.GetContent(web);
        card.SetFaceImage(tex);
    }

    /// <summary>
    /// Loads and opens Cards one by one
    /// </summary>
    /// <returns></returns>
    private IEnumerator OneByOneLoading()
    {
        foreach (var card in _cards)
        {
            yield return LoadImage(_uri, card);
            yield return card.Flip();
        }
        yield return new WaitForSeconds(_waitingTime);
        FlipBack();
    }

    /// <summary>
    /// Loads and opens Cards all together
    /// </summary>
    /// <returns></returns>
    private IEnumerator AllAtOnceLoading()
    {
        foreach (var card in _cards)
        {
            yield return LoadImage(_uri, card);
        }
        foreach (var card in _cards)
        {
            StartCoroutine(card.Flip());
        }
        yield return new WaitForSeconds(_waitingTime);
        FlipBack();
    }

    /// <summary>
    /// Flips all cards to back side
    /// </summary>
    private void FlipBack()
    {
        foreach (var card in _cards)
        {
            if (card.bFaceUp)
            {
                StartCoroutine(card.Flip());
            }
        }
    }

    /// <summary>
    /// Sets the loading mode
    /// </summary>
    /// <param name="v">load mode int value</param>
    public void SetLoadMode(int v)
    {
        _loadMode = (LoadMode)v;
    }
}
