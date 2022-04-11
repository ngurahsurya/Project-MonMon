using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles
{
    int id_tiles;
    Vector3 tiles_location;
    bool tiles_active;
    

    public Tiles(int _id, Vector3 _tiles_location, bool _isActive){
        id_tiles = _id;
        tiles_location = _tiles_location;
        tiles_active = _isActive;
    }

    #region Communcator
    public void setTilesActive() => tiles_active = true;
    public void setTilesUnActive() => tiles_active = false;
    public bool getIsActiveStatus(){ return tiles_active; }
    public Vector3 getTilesPosition(){ return tiles_location; }

    #endregion
}
