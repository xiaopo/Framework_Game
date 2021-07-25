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
    public class AssetManagementAssetLoaderParcelWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(AssetManagement.AssetLoaderParcel);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 9, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Progress", _m_Progress);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsDone", _m_IsDone);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsSucceed", _m_IsSucceed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Instantiate", _m_Instantiate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CancelLoading", _m_CancelLoading);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PDispose", _m_PDispose);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "assetName", _g_get_assetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isFailed", _g_get_isFailed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsPreload", _g_get_IsPreload);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Message", _g_get_Message);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsDownloading", _g_get_IsDownloading);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLoading", _g_get_IsLoading);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isActive", _g_get_isActive);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "loadSceneMode", _g_get_loadSceneMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onComplete", _g_get_onComplete);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "isFailed", _s_set_isFailed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsPreload", _s_set_IsPreload);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Message", _s_set_Message);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isActive", _s_set_isActive);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "loadSceneMode", _s_set_loadSceneMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onComplete", _s_set_onComplete);
            
			
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
				if(LuaAPI.lua_gettop(L) == 3 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && translator.Assignable<System.Type>(L, 3))
				{
					string _assetName = LuaAPI.lua_tostring(L, 2);
					System.Type _type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
					
					AssetManagement.AssetLoaderParcel gen_ret = new AssetManagement.AssetLoaderParcel(_assetName, _type);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to AssetManagement.AssetLoaderParcel constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Progress(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.Progress(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsDone(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsDone(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsSucceed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsSucceed(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Instantiate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Transform>(L, 2)) 
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.Instantiate( _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.Instantiate(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to AssetManagement.AssetLoaderParcel.Instantiate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CancelLoading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CancelLoading(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PDispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    AssetManagement.AssetsGetManger _protect = (AssetManagement.AssetsGetManger)translator.GetObject(L, 2, typeof(AssetManagement.AssetsGetManger));
                    
                    gen_to_be_invoked.PDispose( _protect );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_assetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.assetName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isFailed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isFailed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsPreload(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsPreload);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Message(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Message);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsDownloading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsDownloading);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsLoading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsLoading);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isActive(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isActive);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loadSceneMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.loadSceneMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onComplete(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onComplete);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isFailed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isFailed = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsPreload(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsPreload = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Message(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Message = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isActive(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isActive = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_loadSceneMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                UnityEngine.SceneManagement.LoadSceneMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.loadSceneMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onComplete(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetManagement.AssetLoaderParcel gen_to_be_invoked = (AssetManagement.AssetLoaderParcel)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onComplete = translator.GetDelegate<System.Action<AssetManagement.AssetLoaderParcel>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
