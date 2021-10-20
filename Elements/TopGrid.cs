using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TopGrid : BaseGrid, IListenerAddable
{
    List<int> filledRows, filledColumns;
    [SerializeField] Material particleMaterial;
    public TopBlock[, ] topBlocks;
    private void Awake() {
    }
    private void Start() {
        CreateGrid();
        EventsManager.gameOverEvent.AddListener(DisableGrid);
    }
    void Update()
    {
    }
    protected override void CreateGrid(){
        size = GameData.gridSize;
        blockSize = GameData.blockSize;
        topBlocks = new TopBlock[size, size];
        base.CreateGrid();
        filledRows = new List<int>();
        filledColumns = new List<int>();
    }
    protected override void PlaceANewBlock( int x, int y){
        Vector3 currentPosition = transform.position + offset + new Vector3( x * blockSize, y * blockSize, transform.position.z - 1);
        GameObject currentBlock = Instantiate(blockPrefab, currentPosition, Quaternion.identity);
        currentBlock.name = "Top Block: " + x + ", " + y;
        currentBlock.transform.parent = transform;
        woodBlocks[y, x] = currentBlock;

        TopBlock b = currentBlock.GetComponent<TopBlock>();
        b.xIndex = x;
        b.yIndex = y;
        topBlocks[y, x] = b;
        topBlocks[y, x].DisappearBlock();

    }
    public void UpdateBlock( int x, int y){
        if(topBlocks[y, x].isPlaced){
            topBlocks[y, x].DisappearBlock();
        }
        else{
            topBlocks[y, x].ReappearBlock();
        }
    }
    public void CheckAndUpdate(){   
        CheckGrid();
        UpdateGrid();
    }
    public int CalculateScore(){
        return size*(filledColumns.Count + filledRows.Count) - filledRows.Count*filledColumns.Count;
    }
    void CheckGrid(){
        filledColumns.Clear();
        filledRows.Clear();
        for (int i = 0; i < size; i++)
        {
            if( IsRawFilled(i) ) filledRows.Add(i);
            if( IsColumnFilled(i) ) filledColumns.Add(i);
        }
    }
    void UpdateGrid(){
        foreach (int i in filledRows)
        {
            DeactivateRow(i);
        }
        foreach (int i in filledColumns)
        {
            DeactivateColumn(i);
        }
    }
    void DisableGrid(){
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if(topBlocks[y, x].isPlaced)
                // if(woodBlocks[y, x].activeSelf)
                    if(topBlocks[y, x] == null)
                        Debug.Log("c: " + x + ", "+ y);
                    else
                        topBlocks[y, x].DisableTheBlock();
                        topBlocks[y, x].oneBlockFall.GetComponent<Renderer>().material = particleMaterial;
            }
        }
        StartCoroutine(DisappearBlockRandomly());
    }
    bool IsRawFilled( int r){
        for(int i=0; i<size; i++){
            // if( !woodBlocks[r, i].activeSelf ){
            if( !topBlocks[r, i].isPlaced ){
                return false;
            }
        }
        return true;
    }
    bool IsColumnFilled( int c){
        for(int i=0; i<size; i++){
            // if( !woodBlocks[i, c].activeSelf){
            if( !topBlocks[i, c].isPlaced ){
                return false;
            }
        }
        return true;
    }
    
    void DeactivateRow( int r){
        for(int i=0; i<size; i++){
            topBlocks[r, i].isPlaced = false;
        }
        StartCoroutine(DisappearRow(r));
    }
    void DeactivateColumn( int c){
        for(int i=0; i<size; i++){
            topBlocks[i, c].isPlaced = false;
        }
        StartCoroutine(DisappearColumn(c));
    }
    IEnumerator DisappearBlockRandomly(){
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if( topBlocks[i, j].isPlaced ){
                    yield return new WaitForSeconds(Random.Range(0.05f,0.1f));
                    topBlocks[i, j].BlockFall();
                }
            }
        }
        yield return null;
    }
    IEnumerator DisappearRow( int r ){
        for (int i = 0; i < size; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.01f,0.05f));
            topBlocks[r, i].BlockFall();
        }
        yield return null;
    }
    IEnumerator DisappearColumn( int c ){
        for (int i = 0; i < size; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.01f,0.05f));
            topBlocks[i, c].BlockFall();
        }
        yield return null;
    }
    
    public void AddListener( UnityEvent unityEvent, UnityAction listener ){
        unityEvent.AddListener(listener);
    }
}
