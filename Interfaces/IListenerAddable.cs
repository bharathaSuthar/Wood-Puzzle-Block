using UnityEngine.Events;
public interface IListenerAddable {
    void AddListener(UnityEvent unityEvent, UnityAction unityAction);
}
public interface ITwoIntListenerAddable {
    void AddListener(UnityEvent<int,int> unityEvent, UnityAction<int,int> unityAction);
}
public interface IOneIntListenerAddable {
     void AddListener(UnityEvent<int> unityEvent, UnityAction<int> unityAction);
}
