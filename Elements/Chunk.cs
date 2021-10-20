using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Range(0,8)]
    public int chunkId;
    public float scale = 1f;
    public bool isPlacable;
    public int xSize;
    public int ySize;
    [SerializeField]
    GameObject blockPrefab;
    public Vector3 originalPosition;
    BoxCollider2D box;
    string shape; //shapeFormat: 3x4,101010110100: size and active blocks, left bottom to right top
    // [SerializeField]
    public GameObject[,] woodBlocks;
    public Block[,] woodBlockScripts;
    public void Start() {
        // GenerateShape();
    }
    public void SetShape(){
        shape = GameData.chunkShapes[chunkId];
        // Debug.Log(shape);
        originalPosition = transform.position;
        xSize = int.Parse(shape[2].ToString());
        ySize = int.Parse(shape[0].ToString());
        woodBlocks = new GameObject[ySize, xSize];
        woodBlockScripts = new Block[ySize, xSize];
        isPlacable = true;
        // Debug.Log("xSize: " + xSize + " ySize: " + ySize);
    }
    public void GenerateShape(){
        //this code is for size < 9
        SetShape();
        
        // woodBlocks = new GameObject[ySize, xSize];
        // woodBlockScripts = new Block[ySize, xSize];
        // isPlacable = true;

        int currentX, currentY;
        for (int i = 0; i < shape.Length - 4; i++)
        {
            bool currentBlockState = shape[i+4] == '1';
            currentX = i % xSize;
            currentY = i / xSize;

            Vector3 currentPosition = transform.position + scale * GameData.GetOffset(xSize, ySize)
                                    + new Vector3( currentX * GameData.blockSize * scale, currentY * GameData.blockSize * scale, transform.position.z + 1);
            GameObject currentWoodBlock = Instantiate(blockPrefab, currentPosition, Quaternion.identity);
            currentWoodBlock.transform.localScale = Vector3.one * scale;
            currentWoodBlock.name = "Puzzle Block: " + currentY + ", " + currentX;
            currentWoodBlock.transform.parent = transform;

            woodBlocks[currentY, currentX] = currentWoodBlock;
            woodBlockScripts[currentY, currentX] = currentWoodBlock.GetComponent<Block>();
            if(!currentBlockState){    
                currentWoodBlock.SetActive(false);
            }
        }

        //set up boxCollider
        box = gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2( GameData.blockSize * xSize, GameData.blockSize * ySize);
    }
    public void GetReferences(){
        SetShape();
        for( int i = 0; i<transform.childCount; i++)
        {
            Transform currentChild = transform.GetChild(i);
            int x = i % xSize;
            int y = i / xSize;
            woodBlocks[y, x] = currentChild.gameObject;
            // Debug.Log(woodBlocks[y, x].name);
            woodBlockScripts[y, x] = currentChild.GetComponent<Block>();
        }
    }
    public void BackToOrigin(){
        transform.position = originalPosition;
        transform.localScale =  Vector3.one * scale;
    }
    public void UpdateChunk(){
        //updates chunk if its not placable
        if(null == woodBlockScripts) return;
        if(!isPlacable){
            // Debug.Log(gameObject.name + " is not placable");
            foreach (Block block in woodBlockScripts)
            {
                if(block.gameObject.activeSelf)
                block.DisableTheBlock();
            }
        }
        else{
            // Debug.Log(gameObject.name +  " is placable");
            // Debug.Log(woodBlockScripts.GetType());
            foreach (Block block in woodBlockScripts)
            {
                if(block.gameObject.activeSelf)
                block.EnableTheBlock();
            }
        }
    }

}
