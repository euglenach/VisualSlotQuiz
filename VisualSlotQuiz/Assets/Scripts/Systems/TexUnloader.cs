using System;
using UnityEngine;

namespace Systems{
    public class TexUnloader : MonoBehaviour{
        private void Start(){
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy(){
            Picture.UnloadAssets();
        }
    }
}
