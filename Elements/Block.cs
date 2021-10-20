using UnityEngine;

public class Block : MonoBehaviour
{
    public int xIndex, yIndex;
    [SerializeField]
    public Sprite[] sprites;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void DarkenTheBlock(){
        spriteRenderer.color = Color.gray;
    }
    public void LightenTheBlock(){
        spriteRenderer.color = Color.white;
    }
    public void DisableTheBlock(){
        spriteRenderer.sprite = sprites[1];
    }
    public void EnableTheBlock(){
        spriteRenderer.sprite = sprites[0];
    }
}
