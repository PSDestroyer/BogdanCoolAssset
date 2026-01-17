using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Slider slider;
    public TMP_Text ProgressText, LevelName;

    public void LoadScene(string name, string levelName)
    {
        LevelName.text = levelName;
        StartCoroutine(AsyncSceneLoad(name));
    }
    
    
    IEnumerator  AsyncSceneLoad(string name)
    {
        yield return new WaitForSeconds(.9f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            ProgressText.text = progress + "%";
            yield return null;
        }
    }
}
