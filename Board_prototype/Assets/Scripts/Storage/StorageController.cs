using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class StorageController : IInitializable, IDisposable
{
    [Inject] PlayerData playerData;
    [Inject] PlayerItems playerItems;

    FileStream file;
    StorageClass storage;
    BinaryFormatter bf;

    public void Initialize()
    {
        //File.Delete(Application.persistentDataPath + "/Storage.dat");

        if (File.Exists(Application.persistentDataPath + "/Storage.dat"))
        {
            bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/Storage.dat", FileMode.Open);
            storage = new StorageClass();
            storage = (StorageClass)bf.Deserialize(file);

            playerData.score = storage.score;
            playerData.money = storage.money;
            playerItems.itemData = storage.itemsData.ToArray();

            Debug.Log("File data loaded!");
        }
        else
        {
            setup();
            bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "/Storage.dat");
            storage = new StorageClass();
            storage.score = 0;
            storage.money = 0;
            storage.itemsData = new List<ItemData>(playerItems.itemData);

            bf.Serialize(file, storage);
            Debug.Log("File created!");

        }
        file.Close();

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

    public void setup()
    {
        playerItems.itemData = new ItemData[DefaultCoef.itemsData.Length];
        //Array.Copy(DefaultCoef.itemsData, playerItems.itemData, DefaultCoef.itemsData.Length);
        for (int i = 0; i < DefaultCoef.itemsData.Length; i++)
        {
            playerItems.itemData[i] = new ItemData();
            playerItems.itemData[i].itemName = DefaultCoef.itemsData[i].itemName;
            playerItems.itemData[i].baseCoast = 0 + DefaultCoef.itemsData[i].baseCoast;
            playerItems.itemData[i].baseGrowthRate = 0 + DefaultCoef.itemsData[i].baseGrowthRate;
            playerItems.itemData[i].isBought = false;
            playerItems.itemData[i].level = 0;
        }
    }

}
