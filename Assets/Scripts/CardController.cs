using UnityEngine;
using System.Collections;


public class CardController : MonoBehaviour
{
    public int id; // ID da carta
    public bool isFlipped = false; // Indica se a carta está virada
    public bool isMatched = false; // Indica se a carta foi combinada com outra
    private SpriteRenderer spriteRenderer;
    GameplayController controller;
    
    private void Start()
    {
        controller = FindFirstObjectByType<GameplayController>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (!FindFirstObjectByType<ToastScript>()) controller.FlipCard(this);
    }

    // Método para virar a carta
    public IEnumerator Flip()
    {
        isFlipped = !isFlipped;

        if (isFlipped == false)
        {
            for (var i = 0f; i <= 180; i += 10f)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 90) spriteRenderer.sprite = null;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            for (var i = 180f; i >= 0f; i -= 10f)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 90) spriteRenderer.sprite = controller.cardData[id].cardSprite;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    // Método para marcar a carta como combinada
    public void Match()
    {
        isMatched = true;
        Debug.Log("Formou par!");
        GameObject toast = Instantiate(controller.Toast);
        toast.GetComponent<ToastScript>().setText(controller.cardData[id].cardDescription);

        Destroy(this.gameObject, 1f);
    }
}