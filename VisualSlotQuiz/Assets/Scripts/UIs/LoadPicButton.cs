using Systems;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace UIs{
    public class LoadPicButton : MonoBehaviour{
        [SerializeField] private Number number;
        
        private Button button;
        private bool canLoad;
        private void Start(){
            canLoad = true;
            button = GetComponent<Button>();
            button.OnClickAsObservable()
                  .Subscribe(async _ => {
                      canLoad = false;
                      await Picture.LoadAssets(number, this.GetCancellationTokenOnDestroy());
                      canLoad = true;
                  }).AddTo(this);
        }
    }
}
