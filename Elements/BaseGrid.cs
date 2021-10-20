using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{
    protected int size;
    protected Vector3 offset;
    protected float blockSize;
    public GameObject blockPrefab;
    public Block[,] blocks;
    public GameObject[, ] woodBlocks;
    void Start()
    {
        CreateGrid();
    }
    private void Update() {
        
    }
    protected virtual void CreateGrid(){
        size = GameData.gridSize;
        blockSize = GameData.blockSize;
        woodBlocks = new GameObject[size, size];
        blocks = new Block[size, size];
        offset = GameData.GetOffset(size, size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                PlaceANewBlock(j, i);
            }
        }
    }
    protected virtual void PlaceANewBlock( int x, int y){
        Vector3 currentPosition = transform.position + offset + new Vector3( x * blockSize, y * blockSize, transform.position.z - 1);
        GameObject currentBlock = Instantiate(blockPrefab, currentPosition, Quaternion.identity);
        currentBlock.name = "Block: " + x + ", " + y;
        currentBlock.transform.parent = transform;
        woodBlocks[y, x] = currentBlock;

        Block b = currentBlock.GetComponent<Block>();
        b.xIndex = x;
        b.yIndex = y;
        blocks[y, x] = b;
    }
    public Block GetBlockFromPosition( Vector3 position ){
        Vector2 indices = GameData.GetIndicesOfBlock(position);
        // Debug.Log("x: " + position.x + ", y:" + position.y );
        int xIndex = (int) indices.x;
        int yIndex = (int) indices.y;
        // Debug.Log("xIndex: " + xIndex + " yIndex: " + yIndex );
        // check if block is in range of size
        if( xIndex < size &&  yIndex < size && xIndex > -1 && yIndex > -1){
            return blocks[yIndex, xIndex];
        }
        return null;
    }
    public void DarkenTheBlocks( List<Block> blocksUnderSelectedChunk ){
        foreach (Block block in blocksUnderSelectedChunk)
        {
            block.DarkenTheBlock();
        }
    }
    public void LightenTheBlocks( List<Block> blocksUnderSelectedChunk ){
        foreach (Block block in blocksUnderSelectedChunk)
        {
            block.LightenTheBlock();
        }
    }
}
