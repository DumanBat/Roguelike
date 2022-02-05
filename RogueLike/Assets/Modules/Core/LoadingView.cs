using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    public GameObject root;
    public ProgressBar progressBar;

    public void Load()
    {
        root.SetActive(true);
        progressBar.SetValue(0f);
    }

    public void IncrementProgressBar(float val)
    {
        progressBar.IncrementValue(val);
    }

    public void SetActivePanel(bool val)
    {
        root.SetActive(val);
    }
}
