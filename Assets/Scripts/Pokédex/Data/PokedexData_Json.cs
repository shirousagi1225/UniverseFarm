using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexData_Json
{
    public string storyName;
    public string story;
    public string background;
}

public class PokedexData_Ob
{
    //public List<PokedexData_Json > pokedexDataList;
    public Dictionary<string, List<PokedexData_Json>> pokedexData;
}
