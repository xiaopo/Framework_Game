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
    public class XGUIXListViewWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XGUI.XListView);
			Utils.BeginObjectRegister(type, L, translator, 0, 11, 18, 8);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Start", _m_Start);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemRenderer", _m_GetItemRenderer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemByInstanceId", _m_GetItemByInstanceId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemPositionByIndex", _m_GetItemPositionByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScrollToIndex", _m_ScrollToIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadTemplateAsset", _m_LoadTemplateAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceRefresh", _m_ForceRefresh);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearAll", _m_ClearAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearEvent", _m_ClearEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroy", _m_OnDestroy);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "xScrollRect", _g_get_xScrollRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "template", _g_get_template);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "templateAsset", _g_get_templateAsset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layout", _g_get_layout);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "horizontalSpacing", _g_get_horizontalSpacing);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "verticalSpacing", _g_get_verticalSpacing);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "dataCount", _g_get_dataCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "listItems", _g_get_listItems);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scrollOffset", _g_get_scrollOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "content", _g_get_content);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "viewRect", _g_get_viewRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "velocity", _g_get_velocity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onUpdateRenderer", _g_get_onUpdateRenderer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onUpdateRendererLua", _g_get_onUpdateRendererLua);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onRecycleRenderer", _g_get_onRecycleRenderer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onRecycleRendererLua", _g_get_onRecycleRendererLua);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onCreateRenderer", _g_get_onCreateRenderer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onUpdatePost", _g_get_onUpdatePost);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "xScrollRect", _s_set_xScrollRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "template", _s_set_template);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "templateAsset", _s_set_templateAsset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layout", _s_set_layout);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "horizontalSpacing", _s_set_horizontalSpacing);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "verticalSpacing", _s_set_verticalSpacing);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "dataCount", _s_set_dataCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "velocity", _s_set_velocity);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "GloablFrameCreateCount", _g_get_GloablFrameCreateCount);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "GloablFrameCreateCount", _s_set_GloablFrameCreateCount);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XGUI.XListView gen_ret = new XGUI.XListView();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XGUI.XListView constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Start(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Start(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemRenderer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        XGUI.XListView.ListItemRenderer gen_ret = gen_to_be_invoked.GetItemRenderer( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemByInstanceId(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _instanceID = LuaAPI.xlua_tointeger(L, 2);
                    
                        XGUI.XListView.ListItemRenderer gen_ret = gen_to_be_invoked.GetItemByInstanceId( _instanceID );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemPositionByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Vector2 gen_ret = gen_to_be_invoked.GetItemPositionByIndex( _index );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScrollToIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    float _smoothTime = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _center = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.ScrollToIndex( _index, _smoothTime, _center );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    float _smoothTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.ScrollToIndex( _index, _smoothTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ScrollToIndex( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to XGUI.XListView.ScrollToIndex!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadTemplateAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadTemplateAsset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceRefresh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ForceRefresh(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearEvent(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDestroy(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_xScrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.xScrollRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_template(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.template);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_templateAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.templateAsset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layout(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layout);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_horizontalSpacing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.horizontalSpacing);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_verticalSpacing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.verticalSpacing);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dataCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.dataCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_listItems(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.listItems);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scrollOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.scrollOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_content(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.content);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_viewRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.viewRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_velocity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.velocity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onUpdateRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onUpdateRenderer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onUpdateRendererLua(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onUpdateRendererLua);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onRecycleRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onRecycleRenderer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onRecycleRendererLua(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onRecycleRendererLua);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onCreateRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onCreateRenderer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onUpdatePost(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onUpdatePost);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GloablFrameCreateCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, XGUI.XListView.GloablFrameCreateCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_xScrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.xScrollRect = (XGUI.XScrollRect)translator.GetObject(L, 2, typeof(XGUI.XScrollRect));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_template(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.template = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_templateAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.templateAsset = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layout(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                XGUI.XListView.ListLayout gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.layout = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_horizontalSpacing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.horizontalSpacing = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_verticalSpacing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.verticalSpacing = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_dataCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.dataCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_velocity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListView gen_to_be_invoked = (XGUI.XListView)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.velocity = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GloablFrameCreateCount(RealStatePtr L)
        {
		    try {
                
			    XGUI.XListView.GloablFrameCreateCount = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
