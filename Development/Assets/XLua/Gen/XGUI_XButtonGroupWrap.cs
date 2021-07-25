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
    public class XGUIXButtonGroupWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XGUI.XButtonGroup);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NotifyToggleOn", _m_NotifyToggleOn);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnregisterToggle", _m_UnregisterToggle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterToggle", _m_RegisterToggle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AnyTogglesOn", _m_AnyTogglesOn);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ActiveToggles", _m_ActiveToggles);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAllTogglesOff", _m_SetAllTogglesOff);
			
			
			
			
			
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
					
					XGUI.XButtonGroup gen_ret = new XGUI.XButtonGroup();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XGUI.XButtonGroup constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NotifyToggleOn(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XButtonGroup gen_to_be_invoked = (XGUI.XButtonGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    XGUI.XButton _button = (XGUI.XButton)translator.GetObject(L, 2, typeof(XGUI.XButton));
                    
                    gen_to_be_invoked.NotifyToggleOn( _button );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnregisterToggle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XButtonGroup gen_to_be_invoked = (XGUI.XButtonGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    XGUI.XButton _button = (XGUI.XButton)translator.GetObject(L, 2, typeof(XGUI.XButton));
                    
                    gen_to_be_invoked.UnregisterToggle( _button );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterToggle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XButtonGroup gen_to_be_invoked = (XGUI.XButtonGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    XGUI.XButton _button = (XGUI.XButton)translator.GetObject(L, 2, typeof(XGUI.XButton));
                    
                    gen_to_be_invoked.RegisterToggle( _button );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AnyTogglesOn(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XButtonGroup gen_to_be_invoked = (XGUI.XButtonGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.AnyTogglesOn(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ActiveToggles(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XButtonGroup gen_to_be_invoked = (XGUI.XButtonGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.IEnumerable<XGUI.XButton> gen_ret = gen_to_be_invoked.ActiveToggles(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAllTogglesOff(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XGUI.XButtonGroup gen_to_be_invoked = (XGUI.XButtonGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SetAllTogglesOff(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
