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
    public class GameMSceneMapfightProgramWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.MScene.MapfightProgram);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 7, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AwakeMapFight", _m_AwakeMapFight);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityParent", _g_get_EntityParent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityDeathParent", _g_get_EntityDeathParent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityFllowPartParent", _g_get_EntityFllowPartParent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EffectParent", _g_get_EffectParent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EffectDead", _g_get_EffectDead);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BuffParent", _g_get_BuffParent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BuffDead", _g_get_BuffDead);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Game.MScene.MapfightProgram gen_ret = new Game.MScene.MapfightProgram();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Game.MScene.MapfightProgram constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AwakeMapFight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.AwakeMapFight(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EntityParent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityDeathParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EntityDeathParent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityFllowPartParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EntityFllowPartParent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EffectParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EffectParent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EffectDead(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EffectDead);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BuffParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BuffParent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BuffDead(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Game.MScene.MapfightProgram gen_to_be_invoked = (Game.MScene.MapfightProgram)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BuffDead);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
