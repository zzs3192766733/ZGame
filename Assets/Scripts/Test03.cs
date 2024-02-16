using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Test03 : MonoBehaviour
{
    private void Start()
    {
        this.OnDestroyAsObservable().Subscribe(_ => { Debug.Log("123"); });
    }
}
