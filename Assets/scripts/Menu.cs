using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;
    [SerializeField] Animator animator;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingMenu;
    [SerializeField] AudioMixer audio;

    [SerializeField] Slider fxSlider;
    [SerializeField] Slider musicSlider;
    static float fxLevel = 1;
    static float musicLevel = 1;

    [SerializeField] bool lockCursor;
    [SerializeField] Button firstSelected;

    [SerializeField] Button level1Button;
    [SerializeField] Button settingsBackButton;
    [SerializeField] ColorBlock colors;
    private void Start()
    {
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
        fxSlider.value = fxLevel;
        musicSlider.value = musicLevel;

        firstSelected.Select();

        foreach (Selectable s in FindObjectsByType<Selectable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            s.colors = colors;
        }
    }
    public void StartGame()
    {
        StartCoroutine(ChangeScene("Tutorial"));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenSettings(bool open)
    {
        settingMenu.SetActive(open);

        if (open)
        {
            settingsBackButton.Select();
        }
        else
        {
            firstSelected.Select();
        }
    }
    public void OpenSelect(bool open)
    {
        levelSelect.SetActive(open);

        if (open)
        {
            level1Button.Select();
        }
        else
        {
            firstSelected.Select();
        }
    }
    public void SelectLevel(string level)
    {
        StartCoroutine(ChangeScene(level));
    }
    public void ReturnToMenu()
    {
        FindAnyObjectByType<Player>().Resume();
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        firstSelected.Select();
    }
    public void OnResume()
    {
        FindAnyObjectByType<Player>().Resume();
    }
    public void Resume()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        pauseMenu.SetActive(false);
    }
    public void Restart()
    {
        FindAnyObjectByType<Player>().Resume();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator ChangeScene(string name)
    {
        Time.timeScale = 1;
        animator.SetBool("Fade", true);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(name);
    }

    public void SetSoundFxVolume(float level)
    {
        audio.SetFloat("SoundFxVolume", Mathf.Log10(level)*20);
        fxLevel = level;
    }
    public void SetMusicFxVolume(float level)
    {
        audio.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
        musicLevel = level;
    }
}
