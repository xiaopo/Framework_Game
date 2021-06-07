using System;
using System.Collections.Generic;
using XLua;
using UnityEngine;

public class CSharpLuaInterface
{
    [CSharpCallLua]
    public delegate string Language(int id);
    static Language s_Language;
    public static string GetLanguage(int id) { return s_Language != null ? s_Language(id) : ""; }
    public static void InitAllInterface(LuaEnv luaEnv)
    {
        luaEnv.Global.Get("get_static", out s_Language);
    }

    public static void UnAllInterface()
    {
        s_Language = null;
    }
}

