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
    public class TimerManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(TimerManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 1, 0);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AllTimer", _g_get_AllTimer);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "AddFrame", _m_AddFrame_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddTimer", _m_AddTimer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DelTimer", _m_DelTimer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ResetStart", _m_ResetStart_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddCoroutine", _m_AddCoroutine_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					TimerManager gen_ret = new TimerManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to TimerManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddFrame_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _durationCount = LuaAPI.xlua_tointeger(L, 3);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 4);
                    bool _runback = LuaAPI.lua_toboolean(L, 5);
                    
                        int gen_ret = TimerManager.AddFrame( _tick, _interval, _durationCount, _delay, _runback );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _durationCount = LuaAPI.xlua_tointeger(L, 3);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        int gen_ret = TimerManager.AddFrame( _tick, _interval, _durationCount, _delay );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _durationCount = LuaAPI.xlua_tointeger(L, 3);
                    
                        int gen_ret = TimerManager.AddFrame( _tick, _interval, _durationCount );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        int gen_ret = TimerManager.AddFrame( _tick, _interval );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<TimerManager.Tick>(L, 1)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    
                        int gen_ret = TimerManager.AddFrame( _tick );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TimerManager.AddFrame!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddTimer_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _durationCount = LuaAPI.xlua_tointeger(L, 3);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 4);
                    bool _runback = LuaAPI.lua_toboolean(L, 5);
                    
                        int gen_ret = TimerManager.AddTimer( _tick, _interval, _durationCount, _delay, _runback );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _durationCount = LuaAPI.xlua_tointeger(L, 3);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        int gen_ret = TimerManager.AddTimer( _tick, _interval, _durationCount, _delay );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _durationCount = LuaAPI.xlua_tointeger(L, 3);
                    
                        int gen_ret = TimerManager.AddTimer( _tick, _interval, _durationCount );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<TimerManager.Tick>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        int gen_ret = TimerManager.AddTimer( _tick, _interval );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<TimerManager.Tick>(L, 1)) 
                {
                    TimerManager.Tick _tick = translator.GetDelegate<TimerManager.Tick>(L, 1);
                    
                        int gen_ret = TimerManager.AddTimer( _tick );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TimerManager.AddTimer!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DelTimer_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 1);
                    
                    TimerManager.DelTimer( _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetStart_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 1);
                    
                        int gen_ret = TimerManager.ResetStart( _id );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCoroutine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.IEnumerator _routine = (System.Collections.IEnumerator)translator.GetObject(L, 1, typeof(System.Collections.IEnumerator));
                    
                        UnityEngine.Coroutine gen_ret = TimerManager.AddCoroutine( _routine );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AllTimer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TimerManager gen_to_be_invoked = (TimerManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AllTimer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
