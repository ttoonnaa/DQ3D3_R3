using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace SrLib
{
    public static class SrLogFlow
    {
        /// <summary>
        /// ログを追加
        /// </summary>
        public static void AddLog(SrContext context, string log)
        {
            // エンジンのステートによって処理を分ける
            SrLogCore.Log(log);

            // ログコンポーネントに追加
            //if (context.LogPageComponent)
            //    context.LogPageComponent.AddLog(log);
        }
        
        /// <summary>
        /// ログを追加（重要）
        /// </summary>
        public static void AddLogImportant(SrContext context, string log)
        {
            // エンジンのステートによって処理を分ける
            SrLogCore.Important(log);

            // ログコンポーネントに追加
            //if (context.LogPageComponent)
            //    context.LogPageComponent.AddLog(log);
        }
        
        /// <summary>
        /// ログを追加（警告）
        /// </summary>
        public static void AddLogWarning(SrContext context, string log)
        {
            // エンジンのステートによって処理を分ける
            SrLogCore.Important(log);

            // ログコンポーネントに追加
            //if (context.LogPageComponent)
            //    context.LogPageComponent.AddLog(log);
        }
        
        /// <summary>
        /// エラーを追加
        /// </summary>
        public static void AddError(SrContext context, string log)
        {
            // エンジンのステートによって処理を分ける
            SrLogCore.Error(log);

            // ログコンポーネントに追加しつつ表示する
            //if (context.LogPageComponent)
            //{
            //    context.LogPageComponent.AddLog(log);
            //    context.LogPageComponent.gameObject.SetActive(true);
            //}
        }
        
        /// <summary>
        /// エラーを表示する（例外から）
        /// </summary>
        public static async UniTask ShowError(SrContext context, CancellationToken cancellationToken, Exception exception)
        {
            if (exception is SrException srException)
            {
                await ShowError(context, cancellationToken, srException);
            }
            else
            {
                await ShowError(context, cancellationToken, exception.ToString());
            }
        }
        
        /// <summary>
        /// エラーを表示する（例外から）
        /// </summary>
        public static async UniTask ShowError(SrContext context, CancellationToken cancellationToken, SrException exception)
        {
            // ネストメッセージを取得
            var messages = exception.GetNestedMessages();
            
            // ログはメッセージのみ
            SrLogCore.Log(string.Join("", messages));

            // ログコンポーネントにエラーを追加する
            //if (context.LogPageComponent)
            //    context.LogPageComponent.AddLog(exception.ToString());

            // ダイアログが出せる場合
            if (context.MessageDialogPrefab)
            {
                await _ShowMessageBox(context, cancellationToken, string.Join("\n", messages));
            }
            // ダイアログが出せない場合はログビューを表示する
            //else if (context.LogPageComponent)
            //{
            //    context.LogPageComponent.gameObject.SetActive(true);
            //}
        }
        
        /// <summary>
        /// エラーを表示する（文字列から）
        /// </summary>
        public static async UniTask ShowError(SrContext context, CancellationToken cancellationToken, string errorMessage)
        {
            // ログは必ず出す
            SrLogCore.Error(errorMessage);

            // ログコンポーネントにエラーを追加する
            //if (context.LogPageComponent)
            //    context.LogPageComponent.AddLog(errorMessage);

            // ダイアログが出せる場合
            if (context.MessageDialogPrefab)
            {
                await _ShowMessageBox(context, cancellationToken, errorMessage);
            }
            // ダイアログが出せない場合はログビューを表示する
            //else if (context.LogPageComponent)
            //{
            //    context.LogPageComponent.gameObject.SetActive(true);
            //}
        }
        
        /// <summary>
        /// 情報を表示する（文字列から）
        /// </summary>
        public static async UniTask ShowInfo(SrContext context, CancellationToken cancellationToken, string infoMessage)
        {
            // ログは必ず出す
            SrLogCore.Log(infoMessage);

            // ログコンポーネントにエラーを追加する
            //if (context.LogPageComponent)
            //    context.LogPageComponent.AddLog(infoMessage);

            // ダイアログが出せる場合
            if (context.MessageDialogPrefab)
            {
                await _ShowMessageBox(context, cancellationToken, infoMessage);
            }
        }

        /// <summary>
        /// メッセージボックスを表示
        /// </summary>
        private static async UniTask _ShowMessageBox(SrContext context, CancellationToken cancellationToken, string errorMessage)
        {
            if (context.MessageDialogObject)
                Object.Destroy(context.MessageDialogObject);
                
            // エラーダイアログのプレハブからインスタンスを生成
            var decidedId = 0;
            context.MessageDialogObject = SrContextUtility.Instantiate(context, context.MessageDialogPrefab, context.MainObject);
            context.MessageDialogObject.name = "MessageDialog";
            var messageDialogComponent = context.MessageDialogObject.GetComponent<SrMessageDialogComponent>();
            
            messageDialogComponent.SetData(errorMessage, new List<SrButtonData>
            {
                new () { Text = "OK", OnClick = () => { decidedId = 1; } },

#if DEVELOPMENT_BUILD || UNITY_EDITOR
                new () { Text = "ログを表示", OnClick = () => { decidedId = 2; } },
#endif
            });

            // エラーダイアログのボタン処理
            while (true)
            {
                if (decidedId == 2)
                {
                    //if (context.LogPageComponent)
                    //    context.LogPageComponent.gameObject.SetActive(true);
                }
                else if (decidedId > 0)
                {
                    break;
                }

                await UniTask.Yield(cancellationToken: cancellationToken);
            }

            // ダイアログを閉じる
            Object.Destroy(context.MessageDialogObject);
            context.MessageDialogObject = null;
        }
    }
}
