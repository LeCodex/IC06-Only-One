using UnityEngine;
using UnityEngine.U2D;
public class CharacterAnimator : MonoBehaviour
{
    public SpriteAtlas spriteSheet;

    SpriteRenderer spriteRenderer;
    bool updateSprite = false;

    private void Awake()
    {
        if (spriteSheet != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            updateSprite = true;
        }
    }

    //LateUpdate changes the shown sprite if a spritesheet is used.
    //The spritesheet (sprite Atlas) must have an sprite with the same name.
    private void LateUpdate()
    {
        if (updateSprite)
        {
            string spriteName = spriteRenderer.sprite.name;
            Sprite sprite = spriteSheet.GetSprite(spriteName);
            if (sprite) spriteRenderer.sprite = sprite;
        }
    }
}
