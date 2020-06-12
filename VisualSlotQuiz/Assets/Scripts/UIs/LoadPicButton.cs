using System;
using Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIs{
    public class LoadPicButton : MonoBehaviour{
        [SerializeField] private Number number;
        
        private Button button;
        private void Start(){
            button = GetComponent<Button>();
            button.OnClickAsObservable()
                  .Subscribe(_ => {
                      
                  }).AddTo(this);
        }
    }
}
