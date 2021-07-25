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
    public class XGUITreeItemWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XGUI.TreeItem);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 18, 16);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Show", _m_Show);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayShowEffect", _m_PlayShowEffect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLastFirstInfo", _m_GetLastFirstInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Hide", _m_Hide);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyDotweenPos", _m_ApplyDotweenPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyPos", _m_ApplyPos);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "info", _g_get_info);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "enableSelect", _g_get_enableSelect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isSelected", _g_get_isSelected);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "oldSelected", _g_get_oldSelected);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "with", _g_get_with);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "height", _g_get_height);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isOpen", _g_get_isOpen);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "b_Selected", _g_get_b_Selected);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectIndex", _g_get_selectIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "visible", _g_get_visible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "needFresh", _g_get_needFresh);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fPosition", _g_get_fPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "transfrom", _g_get_transfrom);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gameObject", _g_get_gameObject);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "defaultSize", _g_get_defaultSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gap", _g_get_gap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "button", _g_get_button);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onClick", _g_get_onClick);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "info", _s_set_info);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "enableSelect", _s_set_enableSelect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isSelected", _s_set_isSelected);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "oldSelected", _s_set_oldSelected);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isOpen", _s_set_isOpen);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "b_Selected", _s_set_b_Selected);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectIndex", _s_set_selectIndex);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "visible", _s_set_visible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "needFresh", _s_set_needFresh);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fPosition", _s_set_fPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "transfrom", _s_set_transfrom);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gameObject", _s_set_gameObject);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "defaultSize", _s_set_defaultSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gap", _s_set_gap);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "button", _s_set_button);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onClick", _s_set_onClick);
            
			
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
					
					XGUI.TreeItem gen_ret = new XGUI.TreeItem();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XGUI.TreeItem constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Show(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _gmObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    bool _needEffect = LuaAPI.lua_toboolean(L, 4);
                    UnityEngine.Rect _viewRect;translator.Get(L, 5, out _viewRect);
                    bool _invaild_scroll = LuaAPI.lua_toboolean(L, 6);
                    
                    gen_to_be_invoked.Show( _gmObject, _parent, _needEffect, _viewRect, _invaild_scroll );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayShowEffect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    bool _needEffect = LuaAPI.lua_toboolean(L, 3);
                    UnityEngine.Rect _viewRect;translator.Get(L, 4, out _viewRect);
                    bool _invaild_scroll = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.PlayShowEffect( _parent, _needEffect, _viewRect, _invaild_scroll );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLastFirstInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        XGUI.TreeItemInfo gen_ret = gen_to_be_invoked.GetLastFirstInfo(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Hide(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Hide(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyDotweenPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ApplyDotweenPos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ApplyPos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_info(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.info);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_enableSelect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.enableSelect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isSelected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isSelected);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_oldSelected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.oldSelected);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_with(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.with);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_height(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.height);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isOpen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isOpen);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_b_Selected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.b_Selected);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.selectIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.visible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_needFresh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.needFresh);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.fPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_transfrom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.transfrom);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gameObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.gameObject);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_defaultSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.defaultSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.gap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_button(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.button);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onClick(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onClick);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_info(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.info = (XGUI.TreeItemInfo)translator.GetObject(L, 2, typeof(XGUI.TreeItemInfo));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_enableSelect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.enableSelect = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isSelected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isSelected = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_oldSelected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.oldSelected = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isOpen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isOpen = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_b_Selected(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.b_Selected = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.selectIndex = (System.Collections.Generic.List<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.visible = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_needFresh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.needFresh = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.fPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_transfrom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.transfrom = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gameObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.gameObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_defaultSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.defaultSize = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.gap = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_button(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.button = (XGUI.XButton)translator.GetObject(L, 2, typeof(XGUI.XButton));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onClick(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XGUI.TreeItem gen_to_be_invoked = (XGUI.TreeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onClick = translator.GetDelegate<System.Action<XGUI.TreeItem>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
