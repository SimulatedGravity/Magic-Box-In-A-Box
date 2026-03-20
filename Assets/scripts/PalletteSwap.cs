using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PalletteSwap : MonoBehaviour
{
    [SerializeField] Color color1;
    [SerializeField] Color targetColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (SpriteRenderer s in GameObject.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
        {
            if (s.color == color1) s.color = targetColor;
        }
        foreach (Tilemap s in GameObject.FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
        {
            if (s.color == color1) s.color = targetColor;
        }
        foreach (Image s in GameObject.FindObjectsByType<Image>(FindObjectsSortMode.None))
        {
            if (s.color == color1) s.color = targetColor;
        }
    }
}
