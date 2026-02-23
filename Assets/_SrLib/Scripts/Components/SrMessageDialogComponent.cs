using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SrLib
{
    public class SrButtonData
    {
        public string Text;
        public Action OnClick;
    }
    
    public class SrMessageDialogComponent : SrComponentBase
    {
        public TextMeshProUGUI descriptionText;
        public GameObject buttonObject1;
        public GameObject buttonObject2;
        
        /// <summary>
        /// データを設定する
        /// </summary>
        public void SetData(string description, List<SrButtonData> buttonDatas)
        {
            descriptionText.text = description;
            
            if (buttonDatas == null || buttonDatas.Count == 0)
            {
                buttonObject1.SetActive(false);
                buttonObject2.SetActive(false);
            }
            else
            {
                // ボタンが１つの場合は２つめのボタンを非表示にする
                if (buttonDatas.Count == 1)
                    buttonObject2.SetActive(false);

                for (var i = 0; i < buttonDatas.Count; i++)
                {
                    var buttonIndex = i;
                    
                    // ３つめ以降のボタンはプレハブに無いので新規作成
                    var buttonObject = i >= 2 ? Instantiate(buttonObject1, buttonObject1.transform.parent, false)
                        : i == 1 ? buttonObject2 : buttonObject1;

                    buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = buttonDatas[buttonIndex].Text;
                    buttonObject.GetComponentInChildren<Button>().onClick.AddListener(() => buttonDatas[buttonIndex].OnClick?.Invoke());
                }
            }
        }
    }
}
