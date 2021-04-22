using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class StorageController : IInitializable, IDisposable
{
    //[Inject] SignalBus signalBus;
    [Inject] PlayerData playerData;
    [Inject] PlayerItems playerItems;
    //Player

    FileStream file;
    StorageClass storage;
    BinaryFormatter bf;

    public void Initialize()
    {
        //File.Delete(Application.persistentDataPath + "/Storage.dat");

        if (File.Exists(Application.persistentDataPath + "/Storage.dat"))
        {
            //    bf = new BinaryFormatter();
            //    file = File.Create(Application.persistentDataPath + "/Storage.dat");
            //    storage = new StorageClass();
            //    storage.score = 0;
            //    storage.money = 0;

            //    bf.Serialize(file, storage);
            //    Debug.Log("File created!");
            //}
            //else
            //{
            bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/Storage.dat", FileMode.Open);
            storage = new StorageClass();
            storage = (StorageClass)bf.Deserialize(file);

            playerData.score = storage.score;
            playerData.money = storage.money;
            playerItems.itemData = storage.itemsData.ToArray();

            file.Close();

            Debug.Log("File data loaded!");
        }

    }

    public void Dispose()
    {
        if (File.Exists(Application.persistentDataPath + "/Storage.dat"))
        {
            file = File.Open(Application.persistentDataPath + "/Storage.dat", FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/Storage.dat");
            Debug.Log("File created!");
        }

        bf = new BinaryFormatter();
        storage = new StorageClass();

        //List<ItemData> itemdata = new List<ItemData>(playerItems.itemData);
        //itemdata.AddRange(playerItems.itemData);

        storage.score = playerData.score;
        storage.money = playerData.money;
        storage.itemsData = new List<ItemData>(playerItems.itemData);

        bf.Serialize(file, storage);
        file.Close();

        Debug.Log("Data Saved!");
    }


    private void saveData()
    {
    }
}
