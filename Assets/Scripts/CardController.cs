using UnityEngine;

public class CardController : MonoBehaviour
{
    public int id;                  // ID da carta
    public bool isFlipped = false;  // Indica se a carta está virada
    public bool isMatched = false;  // Indica se a carta foi combinada com outra
    private SpriteRenderer spriteRenderer;
    GameplayController controller;

    private void Start()
    {
        controller = FindFirstObjectByType<GameplayController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        controller.FlipCard(this);
    }

    // Método para virar a carta
    public void Flip()
    {
        isFlipped = !isFlipped;

        if (isFlipped == false) spriteRenderer.sprite = controller.cardBack;
        else spriteRenderer.sprite = controller.cardImages[id];
    }

    // Método para marcar a carta como combinada
    public void Match()
    {
        isMatched = true;
        Debug.Log("Formou par!");
        Destroy(this, 2f);
    }
}
