using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Core
{
    public class MainMenu : MonoBehaviour
    {
        private MainMenuView _currentView;

        private void Awake()
        {
            _currentView = GetComponent<MainMenuView>();

            _currentView.newGame.onClick.AddListener(() =>
            {
                StartCoroutine(GameManager.Instance.loadingController.Load("QuickGame"));
                _currentView.SetActivePanel(false);
            });

            _currentView.chooseGame.onClick.AddListener(() => OpenGameSelector());
            _currentView.exit.onClick.AddListener(() => ExitApp());
        }

        private void OpenGameSelector()
        {
            SetActivePanel(false);
            GameManager.Instance.gameSelector.Init();
        }

        private void ExitApp()
        {
            GameManager.Instance.modalWindow.ShowExitWarning(true);
        }

        public void SetActivePanel(bool val)
        {
            _currentView.SetActiveModule(val);
        }
    }
}

