
using System.Text;
using UnityEngine;

namespace DIY.Debug
{
    /// <summary>
    /// 重新封装日志功能，这个很重要，比如：编辑器下需要正常输出，
    /// apk测试下需要正常输出，但是apk的release版本就需要上传报错日志，但是前端没有输出
    /// 输出格式：
    /// </summary>
    public static class Looog
    {
        //TODO: 需要接一下Debugly和画圈

        public static void Warn(params object[] warns) {
            StringBuilder string_warns = new StringBuilder();
            for (int i = 0; i < warns.Length; i++)
            {
                string_warns.Append(warns[i]);
            }
            UnityEngine.Debug.Log(string.Format("<color=yellow>{0}</color>",string_warns));
        }

        public static void Error(params object[] errors)
        {
            StringBuilder string_errors = new StringBuilder();
            for (int i = 0; i < errors.Length; i++)
            {
                string_errors.Append(errors[i]);
            }
            UnityEngine.Debug.Log(string.Format("<color=red>{0}</color>", string_errors));
        }


    }
}
