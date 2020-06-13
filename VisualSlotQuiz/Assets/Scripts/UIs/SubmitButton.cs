using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIs{
    public class SubmitButton : MonoBehaviour{
        void Start(){
            GetComponent<Button>()
                .OnClickAsObservable()
                .First()
                .Subscribe(_ => {SceneManager.LoadScene(1);})
                .AddTo(this);
        }

    }
}
