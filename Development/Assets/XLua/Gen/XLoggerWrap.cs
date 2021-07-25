#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class XLoggerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLogger);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 12, 2, 2);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLoggerHead", _m_GetLoggerHead_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "INFO", _m_INFO_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "INFO_Format", _m_INFO_Format_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DEBUG", _m_DEBUG_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DEBUG_Format", _m_DEBUG_Format_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WARNING", _m_WARNING_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WARNING_Format", _m_WARNING_Format_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ERROR", _m_ERROR_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ERROR_Format", _m_ERROR_Format_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RecordLog", _m_RecordLog_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReportException", _m_ReportException_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "s_MainThreadId", _g_get_s_MainThreadId);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "s_DebugLevel", _g_get_s_DebugLevel);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "s_MainThreadId", _s_set_s_MainThreadId);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "s_DebugLevel", _s_set_s_DebugLevel);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XLogger gen_ret = new XLogger();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLogger constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoggerHead_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = XLogger.GetLoggerHead( _tag );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_INFO_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _s = translator.GetObject(L, 1, typeof(object));
                    
                    XLogger.INFO( _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_INFO_Format_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _format = LuaAPI.lua_tostring(L, 1);
                    object[] _s = translator.GetParams<object>(L, 2);
                    
                    XLogger.INFO_Format( _format, _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DEBUG_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _s = translator.GetObject(L, 1, typeof(object));
                    
                    XLogger.DEBUG( _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DEBUG_Format_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _format = LuaAPI.lua_tostring(L, 1);
                    object[] _s = translator.GetParams<object>(L, 2);
                    
                    XLogger.DEBUG_Format( _format, _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WARNING_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _s = translator.GetObject(L, 1, typeof(object));
                    
                    XLogger.WARNING( _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WARNING_Format_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _format = LuaAPI.lua_tostring(L, 1);
                    object[] _s = translator.GetParams<object>(L, 2);
                    
                    XLogger.WARNING_Format( _format, _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ERROR_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _s = translator.GetObject(L, 1, typeof(object));
                    
                    XLogger.ERROR( _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ERROR_Format_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _format = LuaAPI.lua_tostring(L, 1);
                    object[] _s = translator.GetParams<object>(L, 2);
                    
                    XLogger.ERROR_Format( _format, _s );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecordLog_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _filePath = LuaAPI.lua_tostring(L, 1);
                    string _content = LuaAPI.lua_tostring(L, 2);
                    string _tag = LuaAPI.lua_tostring(L, 3);
                    
                    XLogger.RecordLog( _filePath, _content, _tag );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _filePath = LuaAPI.lua_tostring(L, 1);
                    string _content = LuaAPI.lua_tostring(L, 2);
                    
                    XLogger.RecordLog( _filePath, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to XLogger.RecordLog!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReportException_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    string _message = LuaAPI.lua_tostring(L, 2);
                    string _stackTrace = LuaAPI.lua_tostring(L, 3);
                    
                    XLogger.ReportException( _name, _message, _stackTrace );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_s_MainThreadId(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, XLogger.s_MainThreadId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_s_DebugLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, XLogger.s_DebugLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_s_MainThreadId(RealStatePtr L)
        {
		    try {
                
			    XLogger.s_MainThreadId = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_s_DebugLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			XLogger.DebugLevel gen_value;translator.Get(L, 1, out gen_value);
				XLogger.s_DebugLevel = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
