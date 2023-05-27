using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Cartas")]
    public Sprite cardBack;
    public Sprite[] cardImages;             // Imagens das cartas
    [Space]
    [Header("Tamanho")]
    public int gridSizeX;                   // Quantidade de colunas do grid
    public int gridSizeY;                   // Quantidade de linhas do grid

    public GameObject cardPrefab;           // Prefab da carta

    private List<int> availableIDs;         // Lista de IDs dispon�veis para as cartas
    private List<CardController> flippedCards;        // Lista das cartas viradas
    private List<CardController> matchedCards;        // Lista das cartas combinadas

    private void Start()
    {
        flippedCards = new List<CardController>();
        matchedCards = new List<CardController>();
        InitializeCards();
    }

    // M�todo para inicializar as cartas
    private void InitializeCards()
    {
        int totalPairs = (gridSizeX * gridSizeY) / 2;
        availableIDs = new List<int>();

        for (int i = 0; i < totalPairs; i++)
        {
            availableIDs.Add(i);
            availableIDs.Add(i);
        }

        Shuffle(availableIDs); // Embaralhar a lista de IDs

        List<Sprite> availableSprites = new List<Sprite>(cardImages); // Lista de imagens dispon�veis
        Shuffle(availableSprites); // Embaralhar a lista de imagens

        int cardCount = gridSizeX * gridSizeY;

        for (int i = 0; i < cardCount; i++)
        {
            // Instanciar uma nova carta
            GameObject cardObj = Instantiate(cardPrefab, new Vector3(i % gridSizeX, i / gridSizeX, 0), Quaternion.identity);
            CardController card = cardObj.GetComponent<CardController>();

            // Definir o ID da carta
            card.id = availableIDs[i];

            // Escolher uma imagem aleat�ria
            int randomIndex = Random.Range(0, availableSprites.Count);
            card.GetComponent<SpriteRenderer>().sprite = availableSprites[randomIndex];
            availableSprites.RemoveAt(randomIndex); // Remover a imagem escolhida da lista

            card.GetComponent<SpriteRenderer>().sprite = cardBack;
        }
    }


    // M�todo para embaralhar uma lista
    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

    }

    // M�todo para virar uma carta
    public void FlipCard(CardController card)
    {
        if (!card.isFlipped && flippedCards.Count < 2)
        {
            card.Flip();
            flippedCards.Add(card);

            if (flippedCards.Count == 2)
            {
                StartCoroutine(CheckForMatch());
            }
        }
    }
    private IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(1f);

        if (flippedCards[0].id == flippedCards[1].id)
        {
            // As cartas s�o um par
            flippedCards[0].Match();
            flippedCards[1].Match();
            matchedCards.Add(flippedCards[0]);
            matchedCards.Add(flippedCards[1]);

            if (matchedCards.Count == gridSizeX * gridSizeY)
            {
                // Todas as cartas foram combinadas, voc� pode adicionar a l�gica de fim do jogo aqui
                Debug.Log("Voc� ganhou!");
            }
        }
        else
        {
            // As cartas n�o s�o um par, ent�o vire-as novamente
            flippedCards[0].Flip();
            flippedCards[1].Flip();
        }

        flippedCards.Clear();
    }
}
