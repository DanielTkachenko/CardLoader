using UnityEngine;

public class Card : MonoBehaviour
{
    private bool bFaceUp { get; set; }

    [SerializeField] private SpriteRenderer faceImage;
    
    void Start()
    {
        bFaceUp = false;
    }

    private void SetFaceImage(Texture2D tex)
    {
        // UnityWebRequest web = UnityWebRequestTexture.GetTexture("https://picsum.photos/1024/1024");
        // yield return web.SendWebRequest();
        // Texture2D tex = DownloadHandlerTexture.GetContent(web);
        faceImage.sprite = Sprite.Create(tex, 
            new Rect(0.0f, 0.0f, tex.width, tex.height), 
            new Vector2(0.5f, 0.5f), 
            100.0f);
    }

    public void Flip()
    {
        //animation
        bFaceUp = !bFaceUp;
    }
}
