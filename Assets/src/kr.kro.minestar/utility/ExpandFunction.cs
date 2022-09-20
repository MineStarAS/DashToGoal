using System;

namespace src.kr.kro.minestar.utility
{
    public static class ExpandFunction
    {
        public static void Debug(this object obj) => UnityEngine.Debug.Log(obj);
        
        public static void Debug(this object obj, string tag) => UnityEngine.Debug.Log(tag + obj);
        
        public static int ConvertToIntTime(this double doubleTime) => doubleTime <= 0 ? 0 : Convert.ToInt32(Math.Round(doubleTime, 2) * 100);
        
        public static double ConvertToDoubleTime(this int intTime) => Math.Round(intTime / 100.0, 1);
    }
}