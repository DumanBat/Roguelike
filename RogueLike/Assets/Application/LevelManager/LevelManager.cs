using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelConfigurator _levelConfigurator;

    private void Awake()
    {
        _levelConfigurator = GetComponent<LevelConfigurator>();
    }

    private void Start()
    {
        _levelConfigurator.SetLevelConfig();
        _levelConfigurator.Init();
    }
}
