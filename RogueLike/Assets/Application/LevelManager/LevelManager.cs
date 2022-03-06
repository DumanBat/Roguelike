using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelConfigurator _levelConfigurator;
    public LevelConfigurator GetLevelConfigurator() => _levelConfigurator;

    private void Awake()
    {
        _levelConfigurator = GetComponent<LevelConfigurator>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {

        _levelConfigurator.SetLevelConfig();
        _levelConfigurator.Init();
    }

    public void Unload()
    {
        _levelConfigurator.Unload();
    }
}
