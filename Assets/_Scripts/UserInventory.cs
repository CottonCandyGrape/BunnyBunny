using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Item 
{
    public string Name;
    public string Type;
}

[Serializable]
public class UserInventory
{
    public List<Item> ItemList = new List<Item>();
}