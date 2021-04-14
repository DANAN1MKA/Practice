using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class StorageController : IInitializable, ILateDisposable
{
    [Inject] SignalBus signalBus;
    [Inject] PlayerData playerData;

    FileStream file;
    StorageClass storage;
    BinaryFormatter bf;

    public void Initialize()
    {
        if (!File.Exists(Application.persistentDataPath + "/Storage.dat"))
        {
            bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "/Storage.dat");
            storage = new StorageClass();
            storage.score = 0;
            storage.money = 0;

            bf.Serialize(file, storage);
            Debug.Log("File created!");
        }
        else
        {
            bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/Storage.dat", FileMode.Open);
            storage = (StorageClass)bf.Deserialize(file);

            playerData.score = storage.score;
            playerData.money = storage.money;

            Debug.Log("File data loaded!");
        }
    }

    public void LateDispose()
    {
        storage.score = playerData.score;
        storage.money = playerData.money;

        bf.Serialize(file, storage);
        file.Close();

        Debug.Log("Data Saved!");
    }

    private void saveData()
    {
    }
}
