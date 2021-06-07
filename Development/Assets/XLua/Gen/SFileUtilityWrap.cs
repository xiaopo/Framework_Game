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
    public class SFileUtilityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SFileUtility);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 10, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadStreamingImg", _m_ReadStreamingImg_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadStreamingFile", _m_ReadStreamingFile_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadStreamingAssetBundle", _m_ReadStreamingAssetBundle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadStreamingImgEx", _m_ReadStreamingImgEx_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadStreamingAssetBundleEx", _m_ReadStreamingAssetBundleEx_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadPersistentAssetBundle", _m_ReadPersistentAssetBundle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WriteText", _m_WriteText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WriteBytes", _m_WriteBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FileMd5", _m_FileMd5_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					SFileUtility gen_ret = new SFileUtility();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SFileUtility constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadStreamingImg_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _error;
                    
                        UnityEngine.Sprite gen_ret = SFileUtility.ReadStreamingImg( _path, out _error );
                        translator.Push(L, gen_ret);
                    LuaAPI.lua_pushstring(L, _error);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadStreamingFile_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _error;
                    
                        string gen_ret = SFileUtility.ReadStreamingFile( _path, out _error );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    LuaAPI.lua_pushstring(L, _error);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadStreamingAssetBundle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _error;
                    
                        UnityEngine.AssetBundle gen_ret = SFileUtility.ReadStreamingAssetBundle( _path, out _error );
                        translator.Push(L, gen_ret);
                    LuaAPI.lua_pushstring(L, _error);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadStreamingImgEx_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _fileName = LuaAPI.lua_tostring(L, 1);
                    string _error;
                    
                        UnityEngine.Sprite gen_ret = SFileUtility.ReadStreamingImgEx( _fileName, out _error );
                        translator.Push(L, gen_ret);
                    LuaAPI.lua_pushstring(L, _error);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadStreamingAssetBundleEx_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _fileName = LuaAPI.lua_tostring(L, 1);
                    string _error;
                    
                        UnityEngine.AssetBundle gen_ret = SFileUtility.ReadStreamingAssetBundleEx( _fileName, out _error );
                        translator.Push(L, gen_ret);
                    LuaAPI.lua_pushstring(L, _error);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadPersistentAssetBundle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _fileName = LuaAPI.lua_tostring(L, 1);
                    string _error;
                    
                        UnityEngine.AssetBundle gen_ret = SFileUtility.ReadPersistentAssetBundle( _fileName, out _error );
                        translator.Push(L, gen_ret);
                    LuaAPI.lua_pushstring(L, _error);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteText_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _data = LuaAPI.lua_tostring(L, 2);
                    
                    SFileUtility.WriteText( _path, _data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteBytes_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 2);
                    
                    SFileUtility.WriteBytes( _path, _bytes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FileMd5_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _file = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = SFileUtility.FileMd5( _file );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
