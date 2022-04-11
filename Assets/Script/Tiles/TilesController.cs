using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
    [Header("Tiles Object")]
    [SerializeField] GameObject tilesPrefabs;
    [SerializeField] List<GameObject> tilesObject = new List<GameObject>();
    [SerializeField] List<Vector3> tilesSpawnPositon = new List<Vector3>();
    [SerializeField] List<Tiles> tilesList = new List<Tiles>();
    
    [Space]
    [Header("Tiles Grapgic")]
    [SerializeField] Sprite active_tiles_sprite;
    [SerializeField] Sprite unactive_tiles_sprite;

    [Header("Poisiton and Transfrom")]
    [SerializeField] private Vector3 maxPos = Vector3.zero;
    [SerializeField] private Vector3 minPos = Vector3.zero;
    [SerializeField] private Vector3 centerPos = Vector3.zero;
    [SerializeField] private Vector3 distance;
    int tilesRowLength = 3;
    int tilesColumLenght = 3;
    int tilesLength = 0;

    private void Awake() {
        GetTilesPosition(); //Get tiles position
    }

    #region  Communicator
    
    public Vector3 getMaxPos(){ return maxPos; } //Getting Max Position of Tiles
    public Vector3 getMinPos(){ return minPos; } //Getting Min Poisition of Tukes
    #endregion

    #region Get and Register Object
    void GetTilesPosition(){ //Get all tiles position

        List<Vector3> pos = new List<Vector3>();
        Vector3 initialSize = tilesPrefabs.GetComponent<Renderer>().bounds.size;
        Vector3 initialPos = Vector3.zero;
        Vector3 posHolder = Vector3.zero;
    
        Vector3 currentStarterPos = initialPos; //Starter column pos
        int idCounter = 1;
        for(int i = 0; i < tilesColumLenght; i++){ //Loop the column

            currentStarterPos = calculateInitialPos(initialPos, initialSize, i); //Resset and get current starter column pos

            for(int j = 0; j < tilesRowLength; j++) //Loow the row
            {
                var distance = (initialSize * j) /2;
                posHolder =   new Vector3(distance.x - (-currentStarterPos.x), distance.y - (-currentStarterPos.y), 0);
                pos.Add(posHolder);
                
                //Spawn Object
                idCounter = idCounter + 1;
                setTilesObject(idCounter, posHolder ); //Set tiles object
                spawnTiles(posHolder); //Spawn tiles object
            }
        }
        tilesSpawnPositon = pos;
        setTilesDefaultInit(); //Set default tiles active
        setTilesObjectActivation(); //Set tiles activation gameobject 

        
        initializeMaxMinPosition(); //Initialize Max and Min Position
    }

    #endregion

    Vector3 calculateInitialPos(Vector3 initialPos, Vector3 initialSize, int columnIndex){ //Calculate initial position for every column tiles
        Vector3 distance = (initialSize * columnIndex) /2;
        Vector3 posToGo = new Vector3(distance.x - initialPos.x, -(distance.y - initialPos.y), 0);
        return posToGo;
    }

    void spawnTiles(Vector3 posToSpawn){
        GameObject obj = Instantiate(tilesPrefabs, posToSpawn, Quaternion.identity);
        obj.transform.parent = this.gameObject.transform;

        tilesObject.Add(obj); //Add GameObject to object list
    }

    void setTilesObject(int _id, Vector3 _pos){ //Set tiles object 
        int id = _id;
        Vector3 pos = _pos;

        Tiles tiles = new Tiles(id, pos, false);
        tilesList.Add(tiles);
    }

    void setTilesDefaultInit(){ //Set tiles default at init
        tilesLength = tilesList.Count;
        float mean = (tilesList.Count /2);
        int midVal = Mathf.RoundToInt(mean) + 1;

        for (int i = 0; i < tilesLength; i++){
            if(i == (midVal - 1)){
                tilesList[i].setTilesActive();
                centerPos = tilesList[i].getTilesPosition();
            }
        }
    }

    void setTilesObjectActivation(){ //Check tiles object is status active or not
        if(tilesList.Count < 1 && tilesObject.Count < 1)
            return;
        int lenght = tilesList.Count;
        
        for (int i = 0; i < lenght ; i++){
            if(tilesList[i].getIsActiveStatus())   //Active object when is Active
                setTilesSprite(i, true);
            else if(!tilesList[i].getIsActiveStatus()) //Deactivate object when is InActive
                setTilesSprite(i, false);
        }
    }

    void setTilesSprite(int _index, bool _isActive){ //Set the tiles sprite (Active/Unactive Sprite)
        tilesObject[_index].GetComponent<SpriteRenderer>().sprite = _isActive? active_tiles_sprite : unactive_tiles_sprite;
    }

    void initializeMaxMinPosition(){
        Debug.Log("Initialize Max and Min pos ");
        Vector3 initialSize =  ((tilesPrefabs.GetComponent<Renderer>().bounds.size) *  (tilesLength / tilesColumLenght)) / 2;
        Vector3 initialPos = centerPos;
        distance = initialSize;
        Vector3 maxPosCalculation  = (initialPos + distance);
        Vector3 minxPosCalculation = (initialPos - distance);
        
        //Set max and min position value
        maxPos = new Vector3(maxPosCalculation.x, maxPosCalculation.y, 0);
        minPos = new Vector3(minxPosCalculation.x, minxPosCalculation.y, 0);

        Debug.Log("Max Position " + maxPos);
        Debug.Log("Min Position " + minPos);
    } 
}
