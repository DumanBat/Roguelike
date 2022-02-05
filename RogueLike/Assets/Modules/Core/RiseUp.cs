using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.Core;

public class RiseUp : Game
{
    
    public override void Init()
    {

    }

    public override void StartGame()
    {
        GameManager.Instance.currentGame = CatalogueManager.GameEnum.RiseUp;
        GameManager.Instance.catalogueManager.onGameSelected.Invoke();
        Debug.Log("Downloading game");
        DownloadGameAssets();
    }
    public override void DownloadGameAssets()
    {
        GameManager.Instance.addressablesController.DownloadAssets();
    }
}
