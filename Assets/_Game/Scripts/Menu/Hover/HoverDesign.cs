using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HoverDesign : MonoBehaviour
{
    //A simple script to handle hovering :D
    Image image;
    [Header("Sprites:"), SerializeField]
    Sprite[] sprites = new Sprite[2];
    private void Awake() =>    
        image = GetComponent<Image>();

    public void Hover(int id)
    {
        AudioManager.instance.PlayHover();
        image.sprite = sprites[id];
    }
}
