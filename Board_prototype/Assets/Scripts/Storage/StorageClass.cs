using UnityEngine;
using System;
using System.Collections.Generic;
//using OdinSerializer;

[Serializable]
public class StorageClass
{
    //[OdinSerialize]
    public System.UInt64 score { get; set; }

    //[OdinSerialize]
    public System.UInt64 money { get; set; }

    //[OdinSerialize]
    public List<ItemData> itemsData { get; set; }
}
