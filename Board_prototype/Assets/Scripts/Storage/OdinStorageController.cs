using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using OdinSerializer;
using System.IO;
using System;

public class OdinStorageController : IInitializable, IDisposable
{
    [Inject] PlayerData playerData;
    [Inject] PlayerItems playerItems;



    public static void save(StorageClass data, string filePath)
    {
        byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }

    public static StorageClass load(string filePath)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        return SerializationUtility.DeserializeValue<StorageClass>(bytes, DataFormat.Binary);
    }

    public void setupPlayerData(StorageClass storage)
    {
        playerData.score = storage.score;
        playerData.money = storage.money;
        playerItems.itemData = storage.itemsData.ToArray();
    }

    public StorageClass getPlayerData()
    {
        StorageClass storage = new StorageClass();
        storage.score = playerData.score;
        storage.money = playerData.money;
        storage.itemsData = new List<ItemData>(playerItems.itemData);
        return storage;
    }

    public void setupDefault()
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


    public void Initialize()
    {
        //File.Delete(Application.persistentDataPath + "/Storage.dat");

        if (File.Exists(Application.persistentDataPath + "/Storage.dat"))
        {
            setupPlayerData(load(Application.persistentDataPath + "/Storage.dat"));

            //TODO: отладка
            Debug.Log("Data Loaded!!");
        }
        else
        {
            setupDefault();
            FileStream file = File.Create(Application.persistentDataPath + "/Storage.dat");
            file.Close();

            save(getPlayerData() , Application.persistentDataPath + "/Storage.dat");

            //TODO: отладка
            Debug.Log("File Created!!");
        }
    }

    public void Dispose()
    {
        save(getPlayerData(), Application.persistentDataPath + "/Storage.dat");

        //TODO: отладка
        Debug.Log("Data Saved!!");
    }
}
