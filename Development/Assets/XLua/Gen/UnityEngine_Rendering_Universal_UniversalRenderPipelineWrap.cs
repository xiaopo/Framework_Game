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
    public class UnityEngineRenderingUniversalUniversalRenderPipelineWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Rendering.Universal.UniversalRenderPipeline);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 6, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "RenderSingleCamera", _m_RenderSingleCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsGameCamera", _m_IsGameCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLightAttenuationAndSpotDirection", _m_GetLightAttenuationAndSpotDirection_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitializeLightConstants_Common", _m_InitializeLightConstants_Common_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "k_ShaderTagName", UnityEngine.Rendering.Universal.UniversalRenderPipeline.k_ShaderTagName);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "maxShadowBias", _g_get_maxShadowBias);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "minRenderScale", _g_get_minRenderScale);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "maxRenderScale", _g_get_maxRenderScale);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "maxPerObjectLights", _g_get_maxPerObjectLights);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "maxVisibleAdditionalLights", _g_get_maxVisibleAdditionalLights);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "asset", _g_get_asset);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset>(L, 2))
				{
					UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset _asset = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)translator.GetObject(L, 2, typeof(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset));
					
					UnityEngine.Rendering.Universal.UniversalRenderPipeline gen_ret = new UnityEngine.Rendering.Universal.UniversalRenderPipeline(_asset);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Rendering.Universal.UniversalRenderPipeline constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderSingleCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Rendering.ScriptableRenderContext _context;translator.Get(L, 1, out _context);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                    UnityEngine.Rendering.Universal.UniversalRenderPipeline.RenderSingleCamera( _context, _camera );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsGameCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    
                        bool gen_ret = UnityEngine.Rendering.Universal.UniversalRenderPipeline.IsGameCamera( _camera );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLightAttenuationAndSpotDirection_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.LightType _lightType;translator.Get(L, 1, out _lightType);
                    float _lightRange = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Matrix4x4 _lightLocalToWorldMatrix;translator.Get(L, 3, out _lightLocalToWorldMatrix);
                    float _spotAngle = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Nullable<float> _innerSpotAngle;translator.Get(L, 5, out _innerSpotAngle);
                    UnityEngine.Vector4 _lightAttenuation;
                    UnityEngine.Vector4 _lightSpotDir;
                    
                    UnityEngine.Rendering.Universal.UniversalRenderPipeline.GetLightAttenuationAndSpotDirection( _lightType, _lightRange, _lightLocalToWorldMatrix, _spotAngle, _innerSpotAngle, out _lightAttenuation, out _lightSpotDir );
                    translator.PushUnityEngineVector4(L, _lightAttenuation);
                        
                    translator.PushUnityEngineVector4(L, _lightSpotDir);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitializeLightConstants_Common_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Unity.Collections.NativeArray<UnityEngine.Rendering.VisibleLight> _lights;translator.Get(L, 1, out _lights);
                    int _lightIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector4 _lightPos;
                    UnityEngine.Vector4 _lightColor;
                    UnityEngine.Vector4 _lightAttenuation;
                    UnityEngine.Vector4 _lightSpotDir;
                    UnityEngine.Vector4 _lightOcclusionProbeChannel;
                    
                    UnityEngine.Rendering.Universal.UniversalRenderPipeline.InitializeLightConstants_Common( _lights, _lightIndex, out _lightPos, out _lightColor, out _lightAttenuation, out _lightSpotDir, out _lightOcclusionProbeChannel );
                    translator.PushUnityEngineVector4(L, _lightPos);
                        
                    translator.PushUnityEngineVector4(L, _lightColor);
                        
                    translator.PushUnityEngineVector4(L, _lightAttenuation);
                        
                    translator.PushUnityEngineVector4(L, _lightSpotDir);
                        
                    translator.PushUnityEngineVector4(L, _lightOcclusionProbeChannel);
                        
                    
                    
                    
                    return 5;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxShadowBias(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Rendering.Universal.UniversalRenderPipeline.maxShadowBias);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_minRenderScale(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Rendering.Universal.UniversalRenderPipeline.minRenderScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxRenderScale(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, UnityEngine.Rendering.Universal.UniversalRenderPipeline.maxRenderScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxPerObjectLights(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Rendering.Universal.UniversalRenderPipeline.maxPerObjectLights);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxVisibleAdditionalLights(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Rendering.Universal.UniversalRenderPipeline.maxVisibleAdditionalLights);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_asset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, UnityEngine.Rendering.Universal.UniversalRenderPipeline.asset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
