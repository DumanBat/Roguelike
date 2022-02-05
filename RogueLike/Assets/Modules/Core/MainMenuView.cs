using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Modules.Core;

public class MainMenuView : MonoBehaviour
{
    public Image background;
    public GameObject root;

    public GameObject centralBox;
    public Button newGame;
    public Button continueGame;
    public Button chooseGame;
    public Button options;
    public Button about;
    public Button exit;


    private int paletteIndex;
    private Color color;
    private float step = 0.005f;
    private float PaletteAmount
    {
        get
        {
            switch (paletteIndex)
            {
                case 0:
                    return color.r;
                case 1:
                    return color.g;
                case 2:
                    return color.b;
                default:
                    return color.r;
            }
        }
        set
        {
            switch (paletteIndex)
            {
                case 0:
                    color.r = value;
                    break;
                case 1:
                    color.g = value;
                    break;
                case 2:
                    color.b = value;
                    break;
                default:
                    color.r = value;
                    break;
            }
        }
    }

    private void Awake()
    {
        GameManager.Instance.catalogueManager.onGameSelected += SetActiveCurrentGameButtons;
    }

    public void Start()
    {
        StartCoroutine(AnimateBackground());    
    }

    public void SetActivePanel(bool val)
    {
        root.SetActive(val);
    }

    public void SetActiveModule(bool val)
    {
        centralBox.SetActive(val);
    }

    public void SetActiveCurrentGameButtons()
    {
        bool val = false;

        if (GameManager.Instance.currentGame != CatalogueManager.GameEnum.example)
            val = true;

        newGame.interactable = val;
        continueGame.interactable = val;

    }

    public IEnumerator AnimateBackground()
    {
        var waitForSeconds = new WaitForSeconds(0.05f);

        while (true)
        {
            color = background.color;

            paletteIndex = Random.Range(0, 2);
            var colorAmount = Random.Range(0.1f, 1f);

            if (PaletteAmount > colorAmount)
                while (PaletteAmount > colorAmount)
                {
                    PaletteAmount -= step;
                    background.color = color;
                    yield return waitForSeconds;
                }
            else
                while (PaletteAmount < colorAmount)
                {
                    PaletteAmount += step;
                    background.color = color;
                    yield return waitForSeconds;
                }
        }
    }
}
