using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SrLib
{
    public static class SrDialogFlow
    {
        /// <summary>
        /// メッセージダイアログを表示する
        /// </summary>
        public static async UniTask ShowMessageDialog(SrContext context, CancellationToken cancellationToken, string message)
        {
            // ダイアログが出せない場合
            if (!context.MessageDialogPrefab)
            {
                // ログを表示する
                SrLogCore.Log(message);
                
                // ログコンポーネントにメッセージを追加する
                //if (context.LogPageComponent)
                //    context.LogPageComponent.AddLog(message);
            }
            else
            {
                if (context.MessageDialogObject)
                    Object.Destroy(context.MessageDialogObject);

                // エラーダイアログのプレハブからインスタンスを生成
                var decidedId = 0;
                context.MessageDialogObject = SrContextUtility.Instantiate(context, context.MessageDialogPrefab, context.MainObject);
                context.MessageDialogObject.name = "MessageDialog";
                var messageDialogComponent = context.MessageDialogObject.GetComponent<SrMessageDialogComponent>();
                messageDialogComponent.SetData(message, new List<SrButtonData>
                {
                    new () { Text = "OK", OnClick = () => { decidedId = 1; } },
                });
                
                // ダイアログのボタン処理
                while (true)
                {
                    if (decidedId > 0)
                        break;

                    await UniTask.Yield(cancellationToken: cancellationToken);
                }
                
                // ダイアログを閉じる
                Object.Destroy(context.MessageDialogObject);
                context.MessageDialogObject = null;
            }
        }
    }
}