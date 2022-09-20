using System;
using Microsoft.CSharp;

namespace src.kr.kro.minestar.utility
{
    public static class ExpandFunction
    {
        public static string Debug(this string str)
        {
            UnityEngine.Debug.Log(str);
            return str;
        }
        
        public static T Debug<T>(this object obj)
        {
            UnityEngine.Debug.Log(obj);
            return obj is T obj1 ? obj1 : default;
        }

        public static T Debug<T>(this object obj, string tag)
        {
            UnityEngine.Debug.Log($"{tag}{obj}");
            return obj is T obj1 ? obj1 : default;
        }
    }
}