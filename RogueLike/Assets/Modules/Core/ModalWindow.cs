using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalWindow : MonoBehaviour
{
    private ModalWindowView _currentView;

    private void Awake()
    {
        _currentView = GetComponent<ModalWindowView>();

        _currentView.exitAccept.onClick.AddListener(() => Application.Quit());
        _currentView.exitCancel.onClick.AddListener(() => ShowExitWarning(false));

    }

    public void ShowExitWarning(bool val)
    {
        _currentView.ShowExitWarning(val);
    }
}
