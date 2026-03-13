using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    [SerializeField] string nextScene;
    public void Finish()
    {
        SceneManager.LoadScene(nextScene);
    }
}
