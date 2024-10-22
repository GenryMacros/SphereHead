using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    
    public static ScenesController instance;
    [SerializeField] public Animator transitionAnim;
    
    private void Awake()
    {
        instance = this;
    }

    public void ToMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }
    
    public void ToGame()
    {
        StartCoroutine(LoadLevel(1));
    }
    
    public void ToEnd()
    {
        StartCoroutine(LoadLevel(2));
    }
    
    IEnumerator LoadLevel(int lvlIndex)
    {
        //transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(lvlIndex);
        //transitionAnim.SetTrigger("Start");
    }
    
    public void CloseGame()
    {
        Application.Quit();
    }
}
