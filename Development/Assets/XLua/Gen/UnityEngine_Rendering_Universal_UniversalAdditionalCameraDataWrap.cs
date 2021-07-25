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
    public class UnityEngineRenderingUniversalUniversalAdditionalCameraDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Rendering.Universal.UniversalAdditionalCameraData);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 18, 14);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetRenderer", _m_SetRenderer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBeforeSerialize", _m_OnBeforeSerialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnAfterDeserialize", _m_OnAfterDeserialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawGizmos", _m_OnDrawGizmos);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "version", _g_get_version);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "renderShadows", _g_get_renderShadows);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "requiresDepthOption", _g_get_requiresDepthOption);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "requiresColorOption", _g_get_requiresColorOption);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "renderType", _g_get_renderType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cameraStack", _g_get_cameraStack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clearDepth", _g_get_clearDepth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "requiresDepthTexture", _g_get_requiresDepthTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "requiresColorTexture", _g_get_requiresColorTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scriptableRenderer", _g_get_scriptableRenderer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "volumeLayerMask", _g_get_volumeLayerMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "volumeTrigger", _g_get_volumeTrigger);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "renderPostProcessing", _g_get_renderPostProcessing);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "antialiasing", _g_get_antialiasing);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "antialiasingQuality", _g_get_antialiasingQuality);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stopNaN", _g_get_stopNaN);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "dithering", _g_get_dithering);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "allowXRRendering", _g_get_allowXRRendering);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "renderShadows", _s_set_renderShadows);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "requiresDepthOption", _s_set_requiresDepthOption);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "requiresColorOption", _s_set_requiresColorOption);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "renderType", _s_set_renderType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "requiresDepthTexture", _s_set_requiresDepthTexture);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "requiresColorTexture", _s_set_requiresColorTexture);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "volumeLayerMask", _s_set_volumeLayerMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "volumeTrigger", _s_set_volumeTrigger);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "renderPostProcessing", _s_set_renderPostProcessing);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "antialiasing", _s_set_antialiasing);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "antialiasingQuality", _s_set_antialiasingQuality);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stopNaN", _s_set_stopNaN);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "dithering", _s_set_dithering);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "allowXRRendering", _s_set_allowXRRendering);
            
			
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
					
					UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_ret = new UnityEngine.Rendering.Universal.UniversalAdditionalCameraData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Rendering.Universal.UniversalAdditionalCameraData constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRenderer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetRenderer( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBeforeSerialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnBeforeSerialize(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnAfterDeserialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnAfterDeserialize(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawGizmos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDrawGizmos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_version(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.version);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_renderShadows(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.renderShadows);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_requiresDepthOption(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.requiresDepthOption);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_requiresColorOption(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.requiresColorOption);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_renderType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.renderType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cameraStack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cameraStack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clearDepth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.clearDepth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_requiresDepthTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.requiresDepthTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_requiresColorTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.requiresColorTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scriptableRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.scriptableRenderer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_volumeLayerMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.volumeLayerMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_volumeTrigger(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.volumeTrigger);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_renderPostProcessing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.renderPostProcessing);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_antialiasing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.antialiasing);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_antialiasingQuality(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.antialiasingQuality);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stopNaN(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.stopNaN);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dithering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.dithering);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_allowXRRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.allowXRRendering);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_renderShadows(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.renderShadows = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_requiresDepthOption(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.Universal.CameraOverrideOption gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.requiresDepthOption = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_requiresColorOption(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.Universal.CameraOverrideOption gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.requiresColorOption = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_renderType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.Universal.CameraRenderType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.renderType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_requiresDepthTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.requiresDepthTexture = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_requiresColorTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.requiresColorTexture = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_volumeLayerMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                UnityEngine.LayerMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.volumeLayerMask = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_volumeTrigger(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.volumeTrigger = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_renderPostProcessing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.renderPostProcessing = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_antialiasing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.Universal.AntialiasingMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.antialiasing = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_antialiasingQuality(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.Universal.AntialiasingQuality gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.antialiasingQuality = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stopNaN(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.stopNaN = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_dithering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.dithering = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_allowXRRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData gen_to_be_invoked = (UnityEngine.Rendering.Universal.UniversalAdditionalCameraData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.allowXRRendering = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
