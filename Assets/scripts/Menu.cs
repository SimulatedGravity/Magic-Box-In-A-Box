using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;
    [SerializeField] Animator animator;
    public void StartGame()
    {
        StartCoroutine(ChangeScene("Tutorial"));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenSettings()
    {

    }
    public void OpenSelect(bool open)
    {
        levelSelect.SetActive(open);
    }
    public void SelectLevel(string level)
    {
        StartCoroutine(ChangeScene(level));
    }
    IEnumerator ChangeScene(string name)
    {
        animator.SetBool("Fade", true);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(name);
    }
}
