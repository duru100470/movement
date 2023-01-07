using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBehavior<TileManager>
{
    private Dictionary<Coordinate, TileHolder> tileHolderDict;
    public Dictionary<Coordinate, TileHolder> TileHolderDict => tileHolderDict;

    private void Start()
    {
        tileHolderDict = new Dictionary<Coordinate, TileHolder>();

        var objs = GameObject.FindGameObjectsWithTag("TileHolder");

        foreach(var obj in objs)
        {
            var tileHolder = obj.GetComponent<TileHolder>();
            tileHolderDict[tileHolder.Pos] = tileHolder;

            Debug.Log($"{tileHolder.Pos.X} {tileHolder.Pos.Y}");
        }
    }

    public List<TileHolder> GetTileHoldersDFS(Coordinate startPos)
    {
        List<TileHolder> tileHolderList = new List<TileHolder>();

        GetTileHoldersDFSRecursion(startPos, ref tileHolderList);

        return tileHolderList;
    }

    private void GetTileHoldersDFSRecursion(Coordinate pos, ref List<TileHolder> tileList)
    {
        if (!TileHolderDict.ContainsKey(pos)) return;
        tileList.Add(tileHolderDict[pos]);

        foreach (var kv in tileHolderDict)
        {
            if (Coordinate.Distance(kv.Key, pos) == 1)
            {
                if (!tileList.Contains(kv.Value))
                {
                    GetTileHoldersDFSRecursion(kv.Key, ref tileList);
                }
            }
        }
    }

    public TileHolder FindTileHolder(Coordinate pos)
    {
        if (TileHolderDict.ContainsKey(pos)) return TileHolderDict[pos];
        return null;
    }

    public bool DestroyTile(TileHolder tileHolder)
    {
        if (!tileHolderDict.ContainsValue(tileHolder)) return false;
        Coordinate key = new Coordinate();
        foreach (var e in tileHolderDict)
        {
            if(e.Value == tileHolder)
            {
                key = e.Key;
                break;
            }
        }
        if (key == null) return false;
        tileHolderDict.Remove(key);
        Destroy(tileHolder.gameObject);
        return true;
    }

    public bool DestroyEntity(Entity entity)
    {
        Coordinate pos = entity.Pos;
        if (pos == null) return false;
        if (!tileHolderDict.ContainsKey(pos))
        { 
            Destroy(entity.gameObject);
            return true;
        }
        return false;
    }

    public void RefreshDict(Dictionary<Coordinate, TileHolder> newDict)
    {
        foreach (var kv in newDict)
        {
            if (kv.Value == null)
                tileHolderDict.Remove(kv.Key);
            else
                tileHolderDict[kv.Key] = kv.Value;
        }
    }
}
