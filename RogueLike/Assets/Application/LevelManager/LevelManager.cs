using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class LevelManager : MonoBehaviour
{
    private LevelConfigurator _levelConfigurator;

    private EnemyTypeHandler _enemyTypeHandler;
    private ItemTypeHandler _itemTypeHandler;
    private WeaponTypeHandler _weaponTypeHandler;

    public PlayerController playerPrefab;
    private PlayerWalkthroughData playerData;

    public LevelConfigurator GetLevelConfigurator() => _levelConfigurator;
    public EnemyTypeHandler GetEnemyTypeHandler() => _enemyTypeHandler;
    public ItemTypeHandler GetItemTypeHandler() => _itemTypeHandler;
    public WeaponTypeHandler GetWeaponTypeHandler() => _weaponTypeHandler;

    private void Awake()
    {
        _levelConfigurator = GetComponent<LevelConfigurator>();
        _enemyTypeHandler = GetComponent<EnemyTypeHandler>();
        _itemTypeHandler = GetComponent<ItemTypeHandler>();
        _weaponTypeHandler = GetComponent<WeaponTypeHandler>();
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
