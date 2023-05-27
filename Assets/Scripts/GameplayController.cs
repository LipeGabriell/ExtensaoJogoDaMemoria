using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Cartas")]
    public Sprite cardBack;
    public CardData[] cardData;             // Imagens das cartas
    [Space]
    [Header("Tamanho")]
    public int gridSizeX;                   // Quantidade de colunas do grid
    public int gridSizeY;                   // Quantidade de linhas do grid

    public GameObject cardPrefab;           // Prefab da carta
    public GameObject Toast;
    public GameObject Win;

    private List<int> availableIDs;         // Lista de IDs disponíveis para as cartas
    private List<CardController> flippedCards;        // Lista das cartas viradas
    private List<CardController> matchedCards;        // Lista das cartas combinadas

    private void Start()
    {
        flippedCards = new List<CardController>();
        matchedCards = new List<CardController>();
        InitializeCards();
    }

    // Método para inicializar as cartas
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

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                int index = y * gridSizeX + x; // Índice baseado na posição do grid

                // Instanciar uma nova carta
                GameObject cardObj = Instantiate(cardPrefab, new Vector3(x, y, 0), Quaternion.identity);
                CardController card = cardObj.GetComponent<CardController>();

                // Definir a imagem e o ID da carta
                card.id = availableIDs[index];
                card.GetComponent<SpriteRenderer>().sprite = cardBack;
            }
        }
    }

    // Método para embaralhar uma lista
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

    // Método para virar uma carta
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
            // As cartas são um par
            flippedCards[0].Match();
            flippedCards[1].Match();
            matchedCards.Add(flippedCards[0]);
            matchedCards.Add(flippedCards[1]);

            if (matchedCards.Count == gridSizeX * gridSizeY)
            {
                // Todas as cartas foram combinadas, você pode adicionar a lógica de fim do jogo aqui
                Instantiate(Win);
                Debug.Log("Jogo trava, precisa adicionar vitória");
            }
        }
        else
        {
            // As cartas não são um par, então vire-as novamente
            flippedCards[0].Flip();
            flippedCards[1].Flip();
        }

        flippedCards.Clear();
    }
}