using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Games{
    public class CorrectButton : MonoBehaviour{
        private readonly Subject<Unit> onClick = new Subject<Unit>();
        public IObservable<Unit> OnClick => onClick;

        private void Start(){
            GetComponent<Button>()
                .OnClickAsObservable()
                .Subscribe(_ => {
                    onClick.OnNext(Unit.Default);
                }).AddTo(this);
        }
    }
}
