using UnityEngine.Events;

public static class EventsManager
{
    public static SingleVoidEventManager<DragObject> 
        chunkIsPicked = new SingleVoidEventManager<DragObject>(),
        chunkIsReleased = new SingleVoidEventManager<DragObject>();

        
    public static OneIntEventManager<Player>
        updateScoreEvent = new OneIntEventManager<Player>();
    public static SingleVoidEventManager<Player>
        gameOverEvent = new SingleVoidEventManager<Player>(),
        chunkIsPlacedOnGridEvent = new SingleVoidEventManager<Player>(),
        chunkIsEmptied = new SingleVoidEventManager<Player>();
}
public class SingleVoidEventManager<T> where T: IListenerAddable
{
    T invoker;
    UnityAction listener;
    UnityEvent unityEvent;
    public UnityAction Listener{
        get{ return listener; }
    }
    public T Invoker{
        get {return invoker; }
    }
    public void AddEventAndInvoker( UnityEvent unityEvent, T invoker){
        this.unityEvent = unityEvent;
        this.invoker = invoker;
        if(listener!=null){
            invoker.AddListener(unityEvent, listener);
        }
    }
    public void AddListener( UnityAction listener ){
        this.listener = listener;
        if(invoker!=null){
            invoker.AddListener(unityEvent, listener);
        }
    }
}
public class OneIntEventManager<T> where T: IOneIntListenerAddable
{
    T invoker;
    UnityAction<int> listener;
    UnityEvent<int> unityEvent;
    public UnityAction<int> Listener{
        get{ return listener; }
    }
    public T Invoker{
        get {return invoker; }
    }
    public void AddEventAndInvoker( UnityEvent<int> unityEvent, T invoker){
        this.unityEvent = unityEvent;
        this.invoker = invoker;
        if(listener!=null){
            invoker.AddListener(unityEvent, listener);
        }
    }
    public void AddListener( UnityAction<int> listener ){
        this.listener = listener;
        if(invoker!=null){
            invoker.AddListener(unityEvent, listener);
        }
    }
}
public class TwoIntEventManager<T> where T: ITwoIntListenerAddable{
    T invoker;
    UnityAction<int,int> listener;
    UnityEvent<int,int> unityEvent;
    public UnityAction<int,int> Listener{
        get{ return listener; }
    }
    public T Invoker{
        get {return invoker; }
    }
    public void AddEventAndInvoker( UnityEvent<int,int> unityEvent, T invoker){
        this.unityEvent = unityEvent;
        this.invoker = invoker;
        if(listener!=null){
            invoker.AddListener(unityEvent, listener);
        }
    }
    public void AddListener( UnityAction<int,int> listener ){
        this.listener = listener;
        if(invoker!=null){
            invoker.AddListener(unityEvent, listener);
        }
    }
}
