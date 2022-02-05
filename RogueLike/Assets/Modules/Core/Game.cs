using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Game : MonoBehaviour
{
    public string gameName;
    public Image preview;
    public string description;

    public abstract void Init();
    public abstract void StartGame();
    public abstract void DownloadGameAssets();
}
