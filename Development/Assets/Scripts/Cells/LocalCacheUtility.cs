using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalCacheUtility
{
    [Serializable]
    public class SaveData
    {
        public int time;
        public string data;
        public override string ToString()
        {
            return string.Format("SaveData[ time={0} data={1} ]", time, data);
        }
    }


    public static void SaveFloat(string key, float value) { PlayerPrefs.SetFloat(key, value); }
    public static float GetFloat(string key) { return PlayerPrefs.GetFloat(key); }
    public static void SaveInt(string key, int value) { PlayerPrefs.SetInt(key, value); }
    public static int GetInt(string key) { return PlayerPrefs.GetInt(key); }
    public static void SaveString(string key, string value) { PlayerPrefs.SetString(key, value); }
    public static string GetString(string key) { return PlayerPrefs.GetString(key); }
    public static bool HasKey(string key) { return PlayerPrefs.HasKey(key); }
    public static void DeleteKey(string key) { PlayerPrefs.DeleteKey(key); }
    public static void DeleteAll() { PlayerPrefs.DeleteAll(); }
    public static void SaveAll() { PlayerPrefs.Save(); }


    /// <summary> 
    /// 获取时间戳 
    /// </summary> 
    /// <returns></returns> 
    static int GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (int)ts.TotalSeconds;
    }

    static DateTime StampToDateTime(string timeStamp)
    {

        DateTime dateTimeStart = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);

        return dateTimeStart.Add(toNow);
    }

    /// <summary>
    /// 保存存在一定时间的本地数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="seconds"></param>
    public static void SaveTimeData(string key, string data, int seconds)
    {
        SaveData sdata = new SaveData();
        sdata.data = data;
        sdata.time = GetTimeStamp() + seconds;
        string json = JsonUtility.ToJson(sdata);
        SaveString(key, json);

        //Debug.Log(StampToDateTime(sdata.time.ToString()));
    }

    public static SaveData GetTimeData(string key)
    {
        string json = GetString(key);

        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            return saveData;
        }
        catch (Exception e)
        {
            Debug.LogError("LocalCacheUtility::GetTimeData() " + e.ToString());
        }

        return null;
    }


    /// <summary>
    /// 将缓存保存天数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="day"></param>
    public static void SaveDay(string key, int day)
    {
        SaveTimeData(key, "", (Mathf.Clamp(day - 1, 0, day)) * 86400);
    }

    //天数
    public static bool GetDay(string key)
    {
        int now = GetTimeStamp();
        SaveData sdata = GetTimeData(key);
        return sdata != null && sdata.time > now;
    }

    //是否为同一天
    public static bool GetSameDay(string key)
    {
        SaveData sdata = GetTimeData(key);
        if (sdata == null)
            return false;
        DateTime dateTime = StampToDateTime(sdata.time.ToString());
        DateTime dateTimeNow = StampToDateTime(GetTimeStamp().ToString());
        return dateTimeNow.Day == dateTime.Day;
    }
}
