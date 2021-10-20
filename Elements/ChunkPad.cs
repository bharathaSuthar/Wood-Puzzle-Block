using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPad : MonoBehaviour
{
    int chunksOnScreenCounts;
    float scale;
    [SerializeField]
    GameObject[] chunkPrefabs;
    GameObject[] chunksOnScreen;
    public Chunk[] chunksOnScreenScripts;
    private void Awake() {
        chunksOnScreenCounts = 3;
        scale = 1f;
    }
    private void Start() {
        InitializeChunkPad();
        GetNextChunks();
        EventsManager.chunkIsEmptied.AddListener(GetNextChunks);
    }
    private void Update() {
        
    }
    void InitializeChunkPad(){
        chunksOnScreen = new GameObject[chunksOnScreenCounts];
        chunksOnScreenScripts = new Chunk[chunksOnScreenCounts];
    }
    void GetNextChunks(){
        for (int i = 0; i < chunksOnScreenCounts; i++)
        {
            int randomIndex = Random.Range(0, 9);
            Vector3 position = transform.position + new Vector3( (i-1)*3*GameData.blockSize,0,0);
            GameObject currentChunk = Instantiate( chunkPrefabs[ randomIndex ] , position , Quaternion.identity );
            currentChunk.transform.parent = transform;
            currentChunk.transform.localScale = Vector3.one * scale;
            chunksOnScreen[i] = currentChunk;

            Chunk chunkScript = currentChunk.GetComponent<Chunk>();
            chunkScript.originalPosition = position;
            chunksOnScreenScripts[i] = chunkScript;

            chunkScript.GetReferences();
        }
    }
    public void DealWithChunkPlaced( Chunk placedChunk ){
        for (int i = 0; i < chunksOnScreenCounts; i++)
        {
            if( chunksOnScreenScripts[i] == placedChunk){
                chunksOnScreenScripts[i] = null;
                break;
            }
        }
        Destroy(placedChunk.gameObject);
    }
}
