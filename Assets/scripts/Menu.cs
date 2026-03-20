using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
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

    private void Start()
    {
        Time.timeScale = 1;
        fxSlider.value = fxLevel;
        musicSlider.value = musicLevel;
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
    }
    public void OpenSelect(bool open)
    {
        levelSelect.SetActive(open);
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
        pauseMenu.SetActive(true);
    }
    public void OnResume()
    {
        FindAnyObjectByType<Player>().Resume();
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
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
