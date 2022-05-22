using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private List<TileBase> tiles;

    [SerializeField] 
    private List<string> levels;
    

    private string actualLevel;
    private TileBase actualTile;

    private Dictionary<string, TileBase> tilesDict;
    
    // Start is called before the first frame update
    void Start()
    {
        tilesDict = new Dictionary<string, TileBase>();
        foreach (var tile in tiles)
        {
            Debug.Log(tile.name);
            tilesDict.Add(tile.name, tile);
        }
    }

    public void ChooseLevel()
    {
        // var random = new System.Random();
        var index = 0;
        actualLevel = levels[index];
        // For Debug
        // actualLevel = "Ground";
        levels.Remove(actualLevel);
        actualTile = tilesDict[actualLevel];
    }

    public TileBase GetTileBase()
    {
        return actualTile;
    }

    public string getActualBoss()
    {
        return "Boss" + actualLevel;
    }
}
