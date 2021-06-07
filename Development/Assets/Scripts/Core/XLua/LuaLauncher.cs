using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaLauncher:MonoBehaviour
{
  
    protected bool isEditorLua;

    private static LuaLauncher m_Instance;
    public static LuaLauncher Instance { 
        get 
        {
            if (m_Instance == null)
            {
                GameObject luaObj = new GameObject("LuaEnvironment");
                DontDestroyOnLoad(luaObj);
                m_Instance = luaObj.AddComponent<LuaLauncher>();
            }
            return m_Instance; 
        } 
    }

    public XLua.LuaEnv LuaEnv { get { return m_LuaEnv; } }
    private static LuaEnv m_LuaEnv;
    private string m_init = @"
    local logger = CS.XLogger
    _G.print_info = function(str)logger.INFO('[Lua]: '..str)end
    _G.print_err  = function(str)logger.ERROR('[Lua]: '..str)end
    _G.print_war  = function(str)logger.WARNING('[Lua]: '..str)end
    _G.reportException  = function(name,message,stackTrace)logger.ReportException(name,message,stackTrace)end

    function class( base ,className)
            local c = {}
            if type(base) == 'table' then
                for k, v in pairs( base ) do
                    c[k] = v
                end
                c.base = base
                c.__className = className;
            elseif type(base) == 'string' then
                c.__className = base
            end 

            c.__index = c

            local mt = {}
            mt.__call = function( class_tbl , ... )
                local obj = {}
                setmetatable( obj, c ) 
                if obj.ctor then
                    obj:ctor(...)
                end
                return obj
            end 
            setmetatable( c, mt )
       return c
    end";


    public IEnumerator InitLuaEnv()
    {
        m_Instance = this;
        m_LuaEnv = new LuaEnv();
        LuaLoader lloader = new LuaLoader();
        m_LuaEnv.AddLoader(lloader);

        //¼ÓÔØ assetBundle
        yield return lloader.LoadLuaAssetBundle();

        isEditorLua = lloader.isEditorLua;
    }

    public void EnterGame()
    {
        m_LuaEnv.DoString(m_init, "m_init");

        GameDebug.Log("XLua Environment Started!");

        string mainfun = string.Format(@" local launcher = require 'game.main' launcher.main() {0}", isEditorLua ? "print('Editor Lua Running ±à¼­Æ÷Ä£Ê½')" : "");

        m_LuaEnv.DoString(mainfun, "launcher");

    }


    protected void Update()
    {
        if(m_LuaEnv != null)
            m_LuaEnv.Tick();
    }

    public void OnDestroy()
    {
        if (m_LuaEnv == null)
            return;

        m_LuaEnv.Dispose();
        m_LuaEnv = null;
    }

}
