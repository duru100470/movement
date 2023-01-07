using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRightTile : Tile
{
    public override void RunCommand(Ground ground)
    {
        ground.OperateLaser(new Coordinate(1, 0));
    }
}
