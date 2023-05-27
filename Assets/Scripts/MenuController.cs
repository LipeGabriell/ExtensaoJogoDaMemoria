using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    //cena 0 é a tela do menu
    //cena 1 é o fácil
    //cena 2 médio
    //cena 3 dificil

    [SerializeField] GameObject jogarLevels;

    public void playEasy()
    {
        AsyncOperation loadEasy = SceneManager.LoadSceneAsync(1);
        StartCoroutine(loadLevel(loadEasy, 1));
    }
    public void playMedium()
    {
        AsyncOperation loadMedium = SceneManager.LoadSceneAsync(2);
        StartCoroutine(loadLevel(loadMedium, 2));

    }
    public void playHard()
    {
        AsyncOperation loadHard = SceneManager.LoadSceneAsync(3);
        StartCoroutine(loadLevel(loadHard, 3));
    }

    IEnumerator loadLevel(AsyncOperation levelProgress, int level)
    {
        yield return new WaitUntil(() => levelProgress.isDone);

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(level));
        SceneManager.UnloadSceneAsync(0);
    }

    public void menuLevels()
    {
        jogarLevels.SetActive(!jogarLevels.activeSelf);
    }

}
