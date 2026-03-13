using UnityEngine;

public class Power : MonoBehaviour
{
    [SerializeField] private bool startEnabled = false;
    private bool _powerOn;
    public SpriteRenderer sprite;
    public Sprite onSprite;
    public Sprite offSprite;

    private void Start()
    {
        PowerOn = startEnabled;
    }

    public bool PowerOn
    {
        get { return _powerOn; }
        set
        {
            _powerOn = value;
            sprite.sprite = value ? onSprite : offSprite;
        }
    }
}
