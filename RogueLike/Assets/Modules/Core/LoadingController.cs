using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Modules.Core;

public class LoadingController : MonoBehaviour
{
    private LoadingView _currentView;

    private void Awake()
    {
        _currentView = GetComponent<LoadingView>();    
    }

    public IEnumerator Load(string sceneId, bool autoDisable = true)
    {
        _currentView.Load();

        yield return StartCoroutine(LoadSceneAsync(sceneId));
        
        if (autoDisable)
            SetActivePanel(false);
    }

    public IEnumerator LoadSceneAsync(string sceneId)
    {
        AsyncOperation handle = SceneManager.LoadSceneAsync(sceneId);
        handle.allowSceneActivation = true;

        while (!handle.isDone)
        {
            _currentView.IncrementProgressBar(handle.progress);
            yield return null;
        }
    }

    public void SetActivePanel(bool val) => _currentView.SetActivePanel(val);
}
