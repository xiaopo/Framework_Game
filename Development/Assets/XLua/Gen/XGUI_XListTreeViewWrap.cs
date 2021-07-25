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
    public class XGUIXListTreeViewWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XGUI.XListTreeView);
			Utils.BeginObjectRegister(type, L, translator, 0, 13, 11, 11);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Start", _m_Start);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearEvent", _m_ClearEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroy", _m_OnDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearItems", _m_ClearItems);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetData", _m_SetData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTreeItem", _m_GetTreeItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CancelSelected", _m_CancelSelected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSeleIndex", _m_SetSeleIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LayerNotClick", _m_LayerNotClick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceCountLayer", _m_ForceCountLayer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnScrollToItem", _m_OnScrollToItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScrollToTop", _m_ScrollToTop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScrollToBottom", _m_ScrollToBottom);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "templates", _g_get_templates);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_scrollRect", _g_get_m_scrollRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_content", _g_get_m_content);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_viewPort", _g_get_m_viewPort);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_autoSelFirst", _g_get_m_autoSelFirst);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_scrollToSelect", _g_get_m_scrollToSelect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onItemClick", _g_get_onItemClick);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onItemSelected", _g_get_onItemSelected);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onItemPlayTween", _g_get_onItemPlayTween);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onShowItem", _g_get_onShowItem);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onCircleItem", _g_get_onCircleItem);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "templates", _s_set_templates);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_scrollRect", _s_set_m_scrollRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_content", _s_set_m_content);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_viewPort", _s_set_m_viewPort);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_autoSelFirst", _s_set_m_autoSelFirst);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_scrollToSelect", _s_set_m_scrollToSelect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onItemClick", _s_set_onItemClick);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onItemSelected", _s_set_onItemSelected);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onItemPlayTween", _s_set_onItemPlayTween);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onShowItem", _s_set_onShowItem);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onCircleItem", _s_set_onCircleItem);
            
			
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
					
					XGUI.XListTreeView gen_ret = new XGUI.XListTreeView();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XGUI.XListTreeView constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Start(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Start(  );
                    
                    
                    
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
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDestroy(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearItems(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearItems(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _listJson = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SetData( _listJson );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTreeItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                        XGUI.TreeItem gen_ret = gen_to_be_invoked.GetTreeItem( _key );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CancelSelected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CancelSelected(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSeleIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SetSeleIndex( _key );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LayerNotClick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layer = LuaAPI.xlua_tointeger(L, 2);
                    bool _sele = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.LayerNotClick( _layer, _sele );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceCountLayer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ForceCountLayer(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnScrollToItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OnScrollToItem( _key );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScrollToTop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float _smothTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.ScrollToTop( _smothTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.ScrollToTop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to XGUI.XListTreeView.ScrollToTop!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScrollToBottom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float _smothTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.ScrollToBottom( _smothTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.ScrollToBottom(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to XGUI.XListTreeView.ScrollToBottom!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_templates(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.templates);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_scrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_scrollRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_content(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_content);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_viewPort(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_viewPort);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_autoSelFirst(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_autoSelFirst);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_scrollToSelect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_scrollToSelect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onItemClick(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onItemClick);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onItemSelected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onItemSelected);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onItemPlayTween(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onItemPlayTween);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onShowItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onShowItem);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onCircleItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onCircleItem);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_templates(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.templates = (System.Collections.Generic.List<XGUI.ListTreeObj>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<XGUI.ListTreeObj>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_scrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_scrollRect = (XGUI.XScrollRect)translator.GetObject(L, 2, typeof(XGUI.XScrollRect));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_content(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_content = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_viewPort(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_viewPort = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_autoSelFirst(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_autoSelFirst = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_scrollToSelect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_scrollToSelect = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onItemClick(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onItemClick = translator.GetDelegate<System.Action<int>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onItemSelected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onItemSelected = translator.GetDelegate<System.Action<int, bool, bool>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onItemPlayTween(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onItemPlayTween = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onShowItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onShowItem = translator.GetDelegate<System.Action<string>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onCircleItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.XListTreeView gen_to_be_invoked = (XGUI.XListTreeView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onCircleItem = translator.GetDelegate<System.Action<int>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
