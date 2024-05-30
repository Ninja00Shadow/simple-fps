using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Animator animator;
    
    public string destinationScene;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetTrigger("Open");
        
        SaveLoadManager.Instance.SaveMapData();
        
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene(destinationScene);
    }
}
