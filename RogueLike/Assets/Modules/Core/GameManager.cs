using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Core
{
    public partial class GameManager : Singleton<GameManager>
    {
        public CatalogueManager.GameEnum currentGame;

        public AddressablesController addressablesController;
        public MainMenu mainMenu;
        public GameSelector gameSelector;
        public CatalogueManager catalogueManager;
        public LoadingController loadingController;
        public ModalWindow modalWindow;
        public CameraController cameraController;
        public InventoryController inventoryController;
        public LevelManager levelManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
