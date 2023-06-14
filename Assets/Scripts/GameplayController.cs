using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//TODO botar os crédios em um array (preguiça de fazer agr)
//TODO melhorar interface
//TODO melhorar os sons, tão ai só pra ter igual a interface (vlw ai design)
//TODO refatoração do código

public class GameplayController : MonoBehaviour
{
    [Header("Cartas")] 
    public Sprite cardBack;
    public CardData[] cardData; // Imagens das cartas
    [Space] 
    [Header("Tamanho")] 
    public int gridSizeX; // Quantidade de colunas do grid
    public int gridSizeY; // Quantidade de linhas do grid

    [Space] 
    [Header("Sons")] 
    private AudioSource _audioSource;
    [SerializeField] private AudioClip somClick;
    [SerializeField] private AudioClip somUnmatch;
    [SerializeField] private AudioClip somMatch;
    [SerializeField] private Slider _volumeSlider; 
    [SerializeField] private AudioMixer _audioMixer;

    [Space] 
    [Header("Components")] 
    [SerializeField] private GameObject cardPrefab; // Prefab da carta
    public GameObject Toast;
    public GameObject Win;

    private List<int> availableIDs; // Lista de IDs disponíveis para as cartas
    private List<CardController> flippedCards; // Lista das cartas viradas
    private List<CardController> matchedCards; // Lista das cartas combinadas
    private Canvas _canvas;
    [Space]
    [Header("Configs")]
    [SerializeField] private GameObject configs;
    [NonSerialized] public bool configOpen = false;
    private void Start()
    {
        Components();
        Shuffle(cardData);
        flippedCards = new List<CardController>();
        matchedCards = new List<CardController>();
        InitializeCards();
    }

    void Components()
    {
        _canvas = gameObject.GetComponentInChildren<Canvas>();
        _canvas.worldCamera = Camera.main;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Método para inicializar as cartas
    private void InitializeCards()
    {
        Transform cardsTransform = GameObject.Find("Cards").transform;
        Vector3 cardsPosition = cardsTransform.position;
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
                GameObject cardObj = Instantiate(cardPrefab, new Vector3(cardsPosition.x + x,cardsPosition.y + y, 0), Quaternion.identity,cardsTransform);
                CardController card = cardObj.GetComponent<CardController>();

                // Definir a imagem e o ID da carta
                card.id = availableIDs[index];
            }
        }
    }

    // Método para embaralhar uma lista
    private static void Shuffle<T>(List<T> list)
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

    private static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int j = i + UnityEngine.Random.Range(0, n - i);
            T temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
    }


    // Método para virar uma carta
    public void FlipCard(CardController card)
    {
        if (!card.isFlipped && flippedCards.Count < 2)
        {
            card.StartCoroutine(card.Flip());
            SoundPlay(somClick);
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
            SoundPlay(somMatch);
            flippedCards[0].Match();
            flippedCards[1].Match();
            matchedCards.Add(flippedCards[0]);
            matchedCards.Add(flippedCards[1]);

            if (matchedCards.Count == gridSizeX * gridSizeY)
            {
                StartCoroutine(WinCoroutine());
            }
        }
        else
        {
            // As cartas não são um par, então vire-as novamente
            SoundPlay(somUnmatch);
            flippedCards[0].StartCoroutine((flippedCards[0].Flip()));
            flippedCards[1].StartCoroutine((flippedCards[1].Flip()));
        }

        flippedCards.Clear();
    }

//Chama a tela de vitória
    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        yield return new WaitUntil(() =>
            !FindFirstObjectByType<ToastScript>() && !FindFirstObjectByType<CardController>());
        Instantiate(Win);
    }

    public void SoundPlay(AudioClip audio)
    {
        _audioSource.clip = audio;
        _audioSource.Play();
    }
    
    public void ChangeVolume()
    {
        _audioMixer.SetFloat("MasterVolume", _volumeSlider.value);
    }
    
    public void MenuConfigs()
    {
        if (FindFirstObjectByType<WinScript>()) return;
        configOpen = !configOpen;
        configs.SetActive(!configs.activeSelf);
    }
}