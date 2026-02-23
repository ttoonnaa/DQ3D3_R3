using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SrLib
{
    public class SrCameraManager
    {
        private readonly SrContext _context;
        private GameObject _cameraObject;
        private Camera _baseCamera;
        
        public SrCameraManager(SrContext context)
        {
            _context = context;
        }

        /// <summary>
        /// CreateObject
        /// </summary>
        public void CreateObject()
        {
            var cameraPrefab = Resources.Load<GameObject>("SrRes_BaseCamera");
            _cameraObject = SrContextUtility.Instantiate(_context, cameraPrefab, _context.MainObject);
            _cameraObject.name = "BaseCamera";
            _baseCamera = _cameraObject.GetComponent<Camera>();
        }

        /// <summary>
        /// オーバーレイカメラをリセットする
        /// </summary>
        public void ResetOverlayCameras()
        {
            if (!_baseCamera)
                return;

            var baseCameraData = _baseCamera.GetUniversalAdditionalCameraData();
            if (!baseCameraData)
                return;
            
            // 一度空にする
            baseCameraData.cameraStack.Clear();
        }
        
        /// <summary>
        /// オーバーレイカメラを設定する
        /// </summary>
        public void SetOverlayCameras(IEnumerable<Camera> overlayCameras)
        {
            if (!_baseCamera)
                return;

            var baseCameraData = _baseCamera.GetUniversalAdditionalCameraData();
            if (!baseCameraData)
                return;

            // 一度空にする
            baseCameraData.cameraStack.Clear();

            // カメラを追加する
            foreach (var overlayCamera in overlayCameras)
                baseCameraData.cameraStack.Add(overlayCamera);
        }
    }
}
