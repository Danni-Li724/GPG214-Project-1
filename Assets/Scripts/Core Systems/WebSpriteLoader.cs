using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WebBackgroundSpriteLoader : MonoBehaviour
    {
        [Header("Image URL")]
        [SerializeField] private string imageUrl =
            "https://images.unsplash.com/vector-1740123385901-0d69693fce5e?q=80&w=880&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";

        [Header("Settings")]
        [SerializeField] private float pixelsPerUnit = 100f;
        [SerializeField] private Vector2 pivot = new Vector2(0.5f, 0.5f);
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // error handling:
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                Debug.LogWarning("imageUrl is empty, please set imageUrl");
                return;
            }

            StartCoroutine(DownloadAndApply());
        }

        private IEnumerator DownloadAndApply() 
        {
            using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    // error handling (this error actually happens quite frequently...):                                                                              
                    Debug.LogWarning($"download failed {req.error}\nURL: {imageUrl}"); 
                    yield break;
                }

                Texture2D tex = DownloadHandlerTexture.GetContent(req);
                if (tex == null)
                {
                    Debug.LogWarning("texture was null");
                    yield break;
                }

                Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot, pixelsPerUnit);

                spriteRenderer.sprite = spr;
            }
        }
    }
}