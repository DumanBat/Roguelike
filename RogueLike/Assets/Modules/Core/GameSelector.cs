using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Modules.Core;

public class GameSelector : MonoBehaviour
{
    private GameSelectorView _currentView;

    private List<string> availableGames = new List<string>();

    private void Awake()
    {
        _currentView = GetComponent<GameSelectorView>();

        _currentView.cancel.onClick.AddListener(() =>
        {
            SetActivePanel(false);
            GameManager.Instance.mainMenu.SetActivePanel(true);
        });
    }

    public void Init()
    {
        SetActivePanel(true);

        foreach (var game in GameManager.Instance.catalogueManager.games)
        {
            if (availableGames.Contains(game.gameName))
                continue;
            else
                availableGames.Add(game.gameName);

            var gameItem = Instantiate(_currentView.gameItemPrefab, _currentView.viewport.transform);
            gameItem.SetActive(true);
            gameItem.GetComponent<Button>().onClick.AddListener(() => game.StartGame());

            var gameItemInstance = gameItem.GetComponent<GameItem>();
            gameItemInstance.title.text = game.gameName;
            //gameItemInstance.preview = game.preview;
            gameItemInstance.description.text = game.description;

            game.Init();
        }
    }

    public void SetActivePanel(bool val)
    {
        _currentView.SetActivePanel(val);
    }
}
