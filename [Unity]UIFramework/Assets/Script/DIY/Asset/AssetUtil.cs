using DIY.Debug;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DIY.Asset
{
    public static class AssetUtil
    {

        private static string _path_root;
        public static string path_root {
            get {
                if (_path_root==null)
                {
                    _path_root = Application.dataPath;
                }
                return _path_root;
            }

        }

        public static string CombinePath(string path,string name) {
            return Path.Combine(path_root, path, name);
        }
        
        /// <summary>
        /// 同步加载（编辑器下和真机下的路径需要区分下）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Load<T>(string path,string name) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
           T asset = AssetDatabase.LoadAssetAtPath<T>(CombinePath(path, name));
            if (asset == null)
            {
                Looog.Error("加载资源失败！ ", "读取路径：", path, name);
            }
            return asset;
#else
			
#endif
        }

        public static void Load_Asyn<T>(string path, string name,Action<T> action) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            T asset = AssetDatabase.LoadAssetAtPath<T>(CombinePath(path, name));
            if (asset == null)
            {
                Looog.Error("异步加载资源失败！ ", "读取路径：", path, name);
            }
            action(asset);
#else
			
#endif
        }
    }
}
