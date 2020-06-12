using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UniRx.Async;
using UnityEngine;

namespace Systems{
    public static class Picture{
        private static Queue<Texture2D>[] assets =
            {new Queue<Texture2D>(), new Queue<Texture2D>(), new Queue<Texture2D>()};

        private static async UniTask<IEnumerable<string>> Load(CancellationToken token){
            return await UniTask.Run(() => {
                using(var ofd = new OpenFileDialog()){
                    ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
                    ofd.Title = "画像ファイルを指定してください";
                    ofd.ShowDialog();
                    token.ThrowIfCancellationRequested();
                    return ofd.FileNames;
                }
            });
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
                texture = new Texture2D(width, height);
                texture.LoadImage(readBinary);
            }

            return texture;
        }
    }
}
