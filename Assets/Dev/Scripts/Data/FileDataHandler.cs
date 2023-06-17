using UnityEngine;
using System;
using System.IO;
public class FileDataHandler
{
        #region Private Fields

        private string dataDirPath = "";
        private string dataFilePath = "";
        private bool useEncryption = false;
        private readonly string encryptionCodeWord = "word";

        #endregion

        #region Public Methods

        // Constructor for FileDataHandler.
        public FileDataHandler(string dataDirPath, string dataFilePath, bool useEncryption)
        {
            this.dataDirPath = dataDirPath;
            this.dataFilePath = dataFilePath;
            this.useEncryption = useEncryption;
        }

        // Try loading from OS related path, if encryption being used then call for EncryptDecrypt method.
        public PlayerData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFilePath);

            PlayerData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<PlayerData>(dataToLoad);

                }
                
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file" + fullPath + "\n" +e);
                }
            }

            return loadedData;

        }

        // Save the data OS related path, if encryption being used then call for EncryptDecrypt method.
        public void Save(PlayerData data)
        {
            string fullPath = Path.Combine(dataDirPath, dataFilePath);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file" + fullPath + "\n" +e);
            }
        }

        #endregion

        #region Private Methods

        // Encrypt the data or vice versa.
        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";

            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }

            return modifiedData;
        }

        #endregion
}
