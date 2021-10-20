using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IListenerAddable, IOneIntListenerAddable
{
    [SerializeField] BaseGrid baseGrid;
    [SerializeField] TopGrid topGrid;
    [SerializeField] ChunkPad chunkPad;
    [SerializeField] GameObject background;
    [SerializeField] GameOverMenu gameOverMenu;
    [SerializeField] HUD hud;
    Chunk selectedChunkScript;
    public Chunk SelectedChunkScript{
        set{
            selectedChunkScript = value;
        }
    }
    public bool doesChunkFit;
    List<Block> blocksUnderSelectedChunk = new List<Block>();
    int score; int remainingChunks;
    UpdateScoreEvent updateScoreEvent;
    ChunkPadIsEmptyEvent chunkPadIsEmptyEvent;
    ChunkIsPlacedOnGridEvent chunkIsPlacedOnGridEvent;
    GameOverEvent gameOverEvent;

    private void Awake() {
        updateScoreEvent = new UpdateScoreEvent();
        chunkPadIsEmptyEvent = new ChunkPadIsEmptyEvent();
        chunkIsPlacedOnGridEvent = new ChunkIsPlacedOnGridEvent();
        gameOverEvent = new GameOverEvent();
    }
    void Start()
    {
        score = 0;
        remainingChunks = 3;
        EventsManager.chunkIsReleased.AddListener( PlaceTheSelectedChunk );


        EventsManager.chunkIsEmptied.AddEventAndInvoker(chunkPadIsEmptyEvent, this);
        EventsManager.chunkIsPlacedOnGridEvent.AddEventAndInvoker( chunkIsPlacedOnGridEvent, this );
        EventsManager.gameOverEvent.AddEventAndInvoker(gameOverEvent, this);
        EventsManager.updateScoreEvent.AddEventAndInvoker(updateScoreEvent, this);
    }
    void Update()
    {
        if(selectedChunkScript != null){
            if( IsSelectedChunkInGrid() ){
                UpdateBaseGrid();
            }
            else{
                baseGrid.LightenTheBlocks(blocksUnderSelectedChunk);
                doesChunkFit = false;
            }
        }
    }
    void OnGameOver(){
        Debug.Log("Game over");
        gameOverMenu.OnGameOver(hud.score, hud.bestScore);
        gameOverMenu.gameObject.SetActive(true);
    }
    void PlaceTheSelectedChunk(){
        if(doesChunkFit){
            foreach (Block block in selectedChunkScript.woodBlockScripts)
            {
                if(block.gameObject.activeSelf){
                    Vector2 indices = GameData.GetIndicesOfBlock(block.transform.position);
                    topGrid.UpdateBlock( (int)indices.x, (int)indices.y);
                }
            }
            remainingChunks--;
            if(remainingChunks == 0){
                chunkPadIsEmptyEvent.Invoke();
                remainingChunks = 3;
            }
            chunkPad.DealWithChunkPlaced(selectedChunkScript);
            baseGrid.LightenTheBlocks(blocksUnderSelectedChunk);

            topGrid.CheckAndUpdate();
            score += topGrid.CalculateScore();
            hud.UpdateScore(score);

            blocksUnderSelectedChunk.Clear();

            bool isGameOver = !CanChunksOnScreenBePlaced();
            if( isGameOver ){
                OnGameOver();
                gameOverEvent.Invoke();
            }
        }
        else{
            selectedChunkScript.BackToOrigin();
        }
        selectedChunkScript = null;
    }
    void UpdateBaseGrid(){
        //clear the last update blocks
        baseGrid.LightenTheBlocks(blocksUnderSelectedChunk);
        blocksUnderSelectedChunk.Clear();

        //find indices of first block of current chunk on grid
        Block firstBlock = selectedChunkScript.woodBlockScripts[0, 0];
        Block firstBaseBlock = baseGrid.GetBlockFromPosition(firstBlock.transform.position);
        if(!firstBaseBlock) return;
        int x = firstBaseBlock.xIndex;
        int y = firstBaseBlock.yIndex;

        if( DoesChunkFit( selectedChunkScript, x, y ) ){
            doesChunkFit = true;
            foreach (Block block in selectedChunkScript.woodBlockScripts)
            {
                if(block.gameObject.activeSelf){
                    Block baseBlock = baseGrid.GetBlockFromPosition(block.transform.position);
                    blocksUnderSelectedChunk.Add(baseBlock);
                }
            }
            baseGrid.DarkenTheBlocks(blocksUnderSelectedChunk);
        }
        else{
            doesChunkFit = false;
            baseGrid.LightenTheBlocks(blocksUnderSelectedChunk);
        }
    }
    bool IsSelectedChunkInGrid(){
        if(!selectedChunkScript){
            return false;
        }
        float xRange = GameData.blockSize * (GameData.gridSize/2 - (float)selectedChunkScript.xSize / 2) + GameData.blockSize/2 ;
        float yRange = GameData.blockSize * (GameData.gridSize/2 - (float)selectedChunkScript.ySize / 2) + GameData.blockSize/2 ;

        float diffX = Mathf.Abs(selectedChunkScript.transform.position.x - baseGrid.transform.position.x);
        float diffY = Mathf.Abs(selectedChunkScript.transform.position.y - baseGrid.transform.position.y);

        if(diffX > xRange || diffY > yRange) return false;
        return true;
    }
    bool CanChunksOnScreenBePlaced(){
        bool atLeastOneChunkIsPlacable = false;
        int i = 0;

        foreach (Chunk chunk in chunkPad.chunksOnScreenScripts)
        {
            if(chunk == null){
                // Debug.Log( "Chunk: " + i + " is null");
            }
            else if( CanChunkBePlaced(chunk ) ){
                // Debug.Log( "Chunk: " + i + " can be placed");
                if(!chunk.isPlacable){
                    chunk.isPlacable = true;
                    chunk.UpdateChunk();
                }
                atLeastOneChunkIsPlacable = true;
            }
            else{
                // Debug.Log( "Chunk: " + i + " can not be placed");
                chunk.isPlacable = false;
                chunk.UpdateChunk();
            }
            i++;
        }
        return atLeastOneChunkIsPlacable;
    }
    bool CanChunkBePlaced( Chunk chunk ){
        int xRange = GameData.gridSize - chunk.xSize;
        int yRange = GameData.gridSize - chunk.ySize;
        for (int y = 0; y <= yRange; y++)
        {
            for (int x = 0; x <= xRange; x++)
            {
                if( DoesChunkFit(chunk, x, y) ){
                    // Debug.Log(chunk.gameObject.name + " fits at: " + x + ", " + y);
                    return true;
                }
            }
        }
        return false;
    }
    bool DoesChunkFit( Chunk chunk, int _x, int _y){
        
        for (int y = 0; y < chunk.ySize; y++)
        {
            for (int x = 0; x < chunk.xSize; x++)
            {
                int X = _x + x;
                int Y = _y + y;
                if( chunk.woodBlocks[y, x].activeSelf ){
                    if( Y >=GameData.gridSize || X >= GameData.gridSize || X < 0 || Y < 0) return false;
                    if( topGrid.topBlocks[Y, X].isPlaced ){
                        // if(debug) Debug.Log("filled at: " + X + ", " + Y);
                        return false;
                    }
                    // if(debug) Debug.Log(chunk.woodBlocks[y, x].name + " is active");
                }
                else{
                    // if(debug) Debug.Log(chunk.woodBlocks[y, x].name + " is not active");
                }
            }
        }
        return true;
    }
    public void AddListener( UnityEvent unityEvent, UnityAction listener ){
        if( unityEvent.GetType() == gameOverEvent.GetType()) gameOverEvent.AddListener(listener);
        else if( unityEvent.GetType() == chunkPadIsEmptyEvent.GetType()) chunkPadIsEmptyEvent.AddListener(listener);
        else if( unityEvent.GetType() == chunkIsPlacedOnGridEvent.GetType()) chunkIsPlacedOnGridEvent.AddListener(listener);
    }
    public void AddListener( UnityEvent<int> unityEvent, UnityAction<int> listener){
        if( unityEvent.GetType() == updateScoreEvent.GetType()) updateScoreEvent.AddListener(listener);
    }

    public void AddUpdateScoreListener( UnityAction<int> listener ){
        updateScoreEvent.AddListener(listener);
    }
    public void AddGameOverEventListener( UnityAction listener){
        gameOverEvent.AddListener(listener);
    }
    public void AddChunkPadIsEmptyEventListener( UnityAction listener){
        chunkPadIsEmptyEvent.AddListener(listener);
    }
    public void AddChunkIsPlacedOnGridEventListener( UnityAction listener ){
        chunkIsPlacedOnGridEvent.AddListener(listener);
    }
}
