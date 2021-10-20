using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragObject : MonoBehaviour, IListenerAddable, ITwoIntListenerAddable
{
    private Vector3 mouseOffset, touchOffset;
    [SerializeField] Player player;
    Bounds bounds;
    Chunk chunk;
    ChunkIsReleasedEvent chunkIsReleasedEvent;
    ChunkIsPickedEvent chunkIsPickedEvent;
    private void Awake() {
        chunkIsReleasedEvent = new ChunkIsReleasedEvent();
        chunkIsPickedEvent = new ChunkIsPickedEvent();
        player = FindObjectOfType<Player>();
        chunk = gameObject.GetComponent<Chunk>();
        bounds = new Bounds( transform.position, new Vector3(chunk.xSize, chunk.ySize, 0) );
    }
    private void Update() {
        if(Input.touchCount > 0 && chunk.isPlacable){
            Touch touch = Input.GetTouch(0);
            float touchDistance = Mathf.Sqrt( bounds.SqrDistance(touch.position));
            if(touchDistance < 0.01f ){
                OnTouchingTheObject(touch);
            }
        }
    }
    void OnTouchingTheObject( Touch touch ){
        // for mobile devices

        if(touch.phase == TouchPhase.Began){
            gameObject.transform.localScale = Vector3.one / 0.7f;
            player.SelectedChunkScript = chunk;
            touchOffset = transform.position - Camera.main.ScreenToWorldPoint( touch.position );
        }
        else if(touch.phase == TouchPhase.Moved){
            transform.position = GameData.GetMousePosition() + mouseOffset;
        }
        else if(touch.phase == TouchPhase.Ended){
            chunkIsReleasedEvent.Invoke();
        }
    }
    private void Start() {
        EventsManager.chunkIsReleased.AddEventAndInvoker(chunkIsReleasedEvent, this);
        EventsManager.chunkIsPicked.AddEventAndInvoker(chunkIsPickedEvent, this);
    }
    private void OnMouseDown() {
        if(!chunk.isPlacable) return;
        mouseOffset = transform.position - GameData.GetMousePosition();
        gameObject.transform.localScale = Vector3.one / 0.7f;
        if(player) player.SelectedChunkScript = chunk;
    }
    private void OnMouseUp() {
        if(!chunk.isPlacable) return;
        chunkIsReleasedEvent.Invoke();
    }
    private void OnMouseDrag() {
        if(!chunk.isPlacable) return;
        transform.position = GameData.GetMousePosition() + mouseOffset;
    }
    public void AddListener( UnityEvent unityEvent, UnityAction listener){
        if( unityEvent.GetType()==chunkIsReleasedEvent.GetType()) chunkIsReleasedEvent.AddListener(listener);
        else if( unityEvent.GetType() == chunkIsPickedEvent.GetType()) chunkIsPickedEvent.AddListener(listener);
    }
    public void AddListener( UnityEvent<int,int> unityEvent, UnityAction<int,int> listener){
        unityEvent.AddListener(listener);
    }
}
