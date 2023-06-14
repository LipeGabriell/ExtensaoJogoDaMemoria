using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //cena 0 é a tela do menu
    //cena 1 é o fácil
    //cena 2 médio
    //cena 3 dificil
    
    
  
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] private GameObject jogarLevels;
    [SerializeField] private GameObject creditos;
    [SerializeField] private GameObject creditosNext;
    [SerializeField] private GameObject creditosBack;
    [SerializeField] private GameObject menuConfigs;
    
    [SerializeField] private GameObject[] creditosTextos;
    private int creditosTextoAtual = 0;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private Slider _volumeSlider;
    private void Start()
    {
        FindFirstObjectByType<AudioSource>().clip = null;
    }

    public void ChangeVolume()
    {
        _audioMixer.SetFloat("MasterVolume", _volumeSlider.value);
    }
    public void PlayEasy()
    {
        AsyncOperation loadEasy = SceneManager.LoadSceneAsync(1);
        StartCoroutine(LoadLevel(loadEasy, 1));
    }

    public void PlayMedium()
    {
        AsyncOperation loadMedium = SceneManager.LoadSceneAsync(2);
        StartCoroutine(LoadLevel(loadMedium, 2));
    }

    public void PlayHard()
    {
        AsyncOperation loadHard = SceneManager.LoadSceneAsync(3);
        StartCoroutine(LoadLevel(loadHard, 3));
    }


    public void MenuCredits()
    {
        creditos.SetActive(!creditos.activeSelf);
    }

    private void FixedUpdate()
    {
        for (var i = 0; i < creditosTextos.Length; i++)
        {
            if (creditosTextoAtual == i)
            {
                creditosTextos[i].SetActive(true);
            }
            else
            {
                creditosTextos[i].SetActive(false);
            }
        }
    }

    public void NextPage()
    {
        creditosTextoAtual += 1;
        PageChecker();
    }

    public void PreviousPage()
    {
        creditosTextoAtual -= 1;
        PageChecker();
    }

    void PageChecker()
    {
        if(creditosTextoAtual == creditosTextos.Length - 1) creditosNext.SetActive(false);
        else creditosNext.SetActive(true);
        if(creditosTextoAtual == 0) creditosBack.SetActive(false);
        else creditosBack.SetActive(true);
    }
    
    IEnumerator LoadLevel(AsyncOperation levelProgress, int level)
    {
        yield return new WaitUntil(() => levelProgress.isDone);
        yield return new WaitForSeconds(_audioClip.length);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(level));
        SceneManager.UnloadSceneAsync(0);
    }

    public void MenuLevels()
    {
        jogarLevels.SetActive(!jogarLevels.activeSelf);
    }
    
    
    public void MenuConfigs()
    {
        menuConfigs.SetActive(!menuConfigs.activeSelf);
    }
    
}