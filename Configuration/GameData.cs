using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData 
{
    public const int gridSize = 10;
    public static float blockSize = 1.45f; //0.568 //1.43f
    public const float backgroundWidth = 1300, backgroundHeight = 2048;
    public static float yPositionOfGrid = 0.87f; //0.35f

    public static Vector3 GetOffset( int xSize, int ySize){
        return new Vector3( blockSize / 2 - xSize * blockSize / 2, blockSize / 2 - ySize * blockSize / 2, 0);
    }
    public static readonly string[] chunkShapes = {
        "3x2,111010",   "2x3,110011",   "2x3,010111",   "2x3,111010",
        "4x1,1111",     "1x4,1111",     "1x1,1",
        "2x2,1111",     "3x3,010111010"
    };
    public static readonly string[] chunkNames = {
        "L",          ".:'",          "':'",          ".:.",
        "|",          "....",         ".",
        "::",         "+"
    };
    public static Vector2 GetIndicesOfBlock( Vector3 position ){
        Vector3 relativeBlockPosition = position - new Vector3(0, yPositionOfGrid, 0) - new Vector3( - gridSize * blockSize / 2, - gridSize * blockSize / 2, 0);
        int xIndex = (int) ( relativeBlockPosition.x / blockSize );
        int yIndex = (int) ( relativeBlockPosition.y / blockSize );
        return new Vector2(xIndex, yIndex);
    }

    public static Vector3 GetMousePosition(){
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
