using UnityEngine;
using System;
using System.Collections.Generic;
//using OdinSerializer;

[Serializable]
public class StorageClass
{
    public System.UInt64 score { get; set; }

    public System.UInt64 money { get; set; }

    public List<ItemData> itemsData { get; set; }
}
