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
    private bool bLoadInProcess;
    
    void Start()
    {
        bLoadInProcess = false;
        _cards = new List<Card>();
        foreach (var item in GetComponentsInChildren<Card>())
        {
            _cards.Add(item);
        }
    }

    private bool AllUp()
    {
        bool allUp = true;
        foreach (var card in _cards)
        {
            if (!card.bFaceUp)
            {
                allUp = false;
                break;
            }
        }

        return allUp;
    }

    /// <summary>
    /// Starts the FlipCardsCorutine function
    /// </summary>
    public void FlipCards()
    {
        StartCoroutine(FlipCardsCorutine());
    }
    
    /// <summary>
    /// Loads and opens cards according to loading mode
    /// </summary>
    private IEnumerator FlipCardsCorutine()
    {
        if (!bLoadInProcess)
        {
            bLoadInProcess = true;
            switch (_loadMode)
            {
                case LoadMode.OneByOne:
                    //one by one loading
                    yield return OneByOneLoading();
                    break;
                case LoadMode.AllAtOnce:
                    //all at once loading
                    yield return AllAtOnceLoading();
                    break;
                case LoadMode.WhenReady:
                    //when ready loading
                    yield return WhenReadyLoading();
                    break;
            }
            bLoadInProcess = false;
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

    private IEnumerator LoadWhenReady(string uri, Card card)
    {
        
        yield return LoadImage(uri, card);
        StartCoroutine(card.Flip(true));
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
            yield return card.Flip(true);
        }
        StartCoroutine(FlipBack(_waitingTime));
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
            StartCoroutine(card.Flip(true));
        }
        StartCoroutine(FlipBack(_waitingTime));
    }

    /// <summary>
    /// Opens images when they are loaded
    /// </summary>
    /// <returns></returns>
    private IEnumerator WhenReadyLoading()
    {
        foreach (var card in _cards)
        {
            StartCoroutine(LoadWhenReady(_uri, card));
        }
        while (!AllUp())
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(FlipBack(_waitingTime));
    }

    /// <summary>
    /// Flips all cards to backside in some period of time
    /// </summary>
    /// <param name="timeToWait">time period</param>
    /// <returns></returns>
    private IEnumerator FlipBack(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        foreach (var card in _cards)
        {
            if (card.bFaceUp)
                StartCoroutine(card.Flip(false));
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

    /// <summary>
    /// Cancels images loading
    /// </summary>
    public void CancelLoading()
    {
        StopAllCoroutines();
        StartCoroutine(FlipBack(0));
    }
}
