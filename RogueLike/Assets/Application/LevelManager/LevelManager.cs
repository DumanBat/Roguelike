using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class LevelManager : MonoBehaviour
{
    private LevelConfigurator _levelConfigurator;
    public LevelConfigurator GetLevelConfigurator() => _levelConfigurator;

    public PlayerController playerPrefab;
    private PlayerWalkthroughData playerData;

    private void Awake()
    {
        _levelConfigurator = GetComponent<LevelConfigurator>();
    }

    public void Init(string sceneId)
    {
        StartCoroutine(InitRoutine(sceneId));
    }

    public IEnumerator InitRoutine(string sceneId)
    {
        yield return StartCoroutine(GameManager.Instance.loadingController.Load(sceneId, false));

        var player = PlayerController.Instance == null
            ? Instantiate(playerPrefab, Vector3.zero, Quaternion.identity)
            : PlayerController.Instance;

        // Camera and player Init order important. Camera.Init() then PlayerController.Init().
        GameManager.Instance.cameraController.SetCamera(Camera.main, player.transform);
        GameManager.Instance.inventoryController.Init();

        if (playerData != null)
            PlayerController.Instance.Init(playerData);
        else
            PlayerController.Instance.Init();

        PlayerController.Instance.SetPosition(Vector2.zero);
        _levelConfigurator.SetLevelConfig();
        _levelConfigurator.Init();

        yield return new WaitUntil(() => _levelConfigurator.RoomSpawnCompleted());
        GameManager.Instance.loadingController.SetActivePanel(false);
    }

    public void Unload(bool saveProgress = true)
    {
        if (saveProgress)
            SaveProgress();

        PlayerController.Instance.Unload();
        GameManager.Instance.inventoryController.Unload();
        _levelConfigurator.Unload();
    }

    public void SaveProgress()
    {
        playerData = new PlayerWalkthroughData()
        {
            health = PlayerController.Instance.Health,
            maxHealth = PlayerController.Instance.GetMaxHealthValue(),
            weaponConfigs = PlayerController.Instance.weaponController.GetWeaponConfigs()
        };
    }
}
