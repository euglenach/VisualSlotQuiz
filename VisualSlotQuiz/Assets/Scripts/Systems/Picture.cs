using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SFB;
using UniRx.Async;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems{
    public static class Picture{
        private static Queue<Texture2D>[] assets =
            {new Queue<Texture2D>(), new Queue<Texture2D>(), new Queue<Texture2D>()};

        private static Texture2D clearTexture;
        public static Texture2D ClearTexture => clearTexture;

        public static Texture2D GetNextTex2D(Number number){
            return assets[(int)number].Dequeue();
        }
        public static async UniTask LoadAssets(Number number,CancellationToken token){
            foreach(var path in await Load(token)){
                Debug.Log(path);
                token.ThrowIfCancellationRequested();
                var tex2D = FilePath2Tex2D(path);
                token.ThrowIfCancellationRequested();
                if(tex2D==null){continue;}
                assets[(int)number].Enqueue(tex2D);
            }
            clearTexture = Resources.Load<Texture2D>("clear");
        }

        public static bool HasNextTex2D(Number number){
            return assets[(int)number].Any();
        }

        public static void UnloadAssets(){
            Object.DestroyImmediate(clearTexture,true);
            foreach(var asset in assets){
                foreach(var tex in asset){
                    Debug.Log(tex);
                    Object.DestroyImmediate(tex,true);
                }
            }
            Resources.UnloadUnusedAssets();
        }

        private static async UniTask<IEnumerable<string>> Load(CancellationToken token){
            // return await UniTask.Run(() => {
            //     // using(var ofd = new OpenFileDialog()){
            //     //     ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            //     //     ofd.Title = "画像ファイルを指定してください";
            //     //     ofd.ShowDialog();
            //     //     token.ThrowIfCancellationRequested();
            //     //     return ofd.FileNames;
            //     // }
            //     var extensions = new[]{
            //         new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
            //     };
            //     
            //     var path = StandaloneFileBrowser.OpenFilePanel("画像ファイルを指定してください", "", extensions, true);
            //     return path;
            // });
            var extensions = new[]{
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
            };
                
            var path = StandaloneFileBrowser.OpenFilePanel("画像ファイルを指定してください", "", extensions, true);
            return path;
        }
        

        private static Texture2D FilePath2Tex2D(string path){
            Texture2D texture;
            if(!File.Exists(path)){ return null;}

            using(var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read)){
                var bin = new BinaryReader(fileStream);
                var readBinary = bin.ReadBytes((int)bin.BaseStream.Length);
                bin.Close();
                //横サイズ
                var pos = 16;
                var width = 0;
                for(var i = 0; i < 4; i++){ width = width * 256 + readBinary[pos++]; }

                //縦サイズ
                var height = 0;
                for(var i = 0; i < 4; i++){ height = height * 256 + readBinary[pos++]; }

                //byteからTexture2D作成
                texture = new Texture2D(1,1);
                texture.LoadImage(readBinary);
            }

            return texture;
        }
        
    }
}
