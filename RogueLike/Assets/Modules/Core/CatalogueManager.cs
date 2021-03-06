using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class CatalogueManager : MonoBehaviour
{
    public List<Game> games = new List<Game>();
    public RogueLike rogueLike;

    public enum GameEnum
    {
        RiseUp,
        example
    }

    public Action onGameSelected;

    private void Awake()
    {
        games.Add(rogueLike);
    }
}
