using System;
using Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Games{
    public class AdvancePicture : MonoBehaviour{
        [SerializeField] private Number number;
        private bool isClear;
        private CorrectButton button;
        private RawImage image;

        private void Start(){
            button = GetComponentInChildren<CorrectButton>();
            image = GetComponentInChildren<RawImage>();
            
            Advance();
            
            button.OnClick
                  .Where(_ => !isClear)
                  .Subscribe(_ => {
                      Advance();
                  }).AddTo(this);
        }

        private void Advance(){
            if(!Picture.HasNextTex2D(number)){
                isClear = true;
                image.texture = Resources.Load<Texture2D>("clear");
                button.GetComponent<Button>().interactable = false;
                return;
            }
                    
            var tex = Picture.GetNextTex2D(number);
            image.texture = tex;
        }
    }
}
