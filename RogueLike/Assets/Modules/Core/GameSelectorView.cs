using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectorView : MonoBehaviour
{
    public GameObject root;
    public GameObject gameItemPrefab;
    public GameObject viewport;
    public Button cancel;

    public void SetActivePanel(bool val)
    {
        root.SetActive(val);
    }
}
