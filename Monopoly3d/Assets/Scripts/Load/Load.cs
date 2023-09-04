using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _loadText;
    [SerializeField] private TMP_Text _downButtonText;

    private void Start()
    {
        _downButtonText.gameObject.SetActive(false);
        StartCoroutine(LoadGameScene("OfflineGameScene"));
    }

    private IEnumerator LoadGameScene(string sceneName)
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneName);
        loadAsync.allowSceneActivation = false;

        yield return new WaitForSeconds(1);

        while (!loadAsync.isDone)
        {
            if(loadAsync.progress < 0.9f)
            {
                _slider.value = loadAsync.progress / 0.9f;
                _loadText.text = (loadAsync.progress / 0.9f * 100f).ToString() + "%";
            }
            else
            {
                _slider.value = 1f;
                _loadText.text = "100%";
                _downButtonText.gameObject.SetActive(true);
                if(Input.anyKeyDown)
                {
                    loadAsync.allowSceneActivation = true;
                }
            }
        
            yield return null;
        }
    }
}
