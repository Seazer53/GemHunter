using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
        #region Private Fields

        [Header("File Storage Config")] 
        [SerializeField] private string fileName;
        
        [SerializeField] private bool useEncryption;
        
        private PlayerData playerData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileDataHandler dataHandler;

        //private StartLevelUI startLevelUI;
        //private InLevelUI inLevelUI;

        #endregion

        #region Public Properties

        // Instance of DataPersistenceManager for other scripts.
        public static DataPersistenceManager instance { get; private set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one DataPersistenceManager instance in the scene.");
            }
            
            instance = this;
        }

        private void Start()
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        #endregion

        #region Private Methods

        // Create new PlayerData.
        private void NewGame()
        {
            playerData = new PlayerData();
        }

        // Load data, if it's empty create new PlayerData.
        private void LoadGame()
        {
            playerData = dataHandler.Load();
            
            if (playerData == null)
            {
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(playerData);
            }
        }

        // Save data by reference so that we'll be sure we have the latest changes.
        private void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref playerData);
            }

            dataHandler.Save(playerData);
        }

        // If game is not paused or started, when the app closed, save the data before exiting.
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        // Find all IDataPersistence implementors and return the list of them.
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        #endregion
}
