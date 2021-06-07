
using UnityEngine;

[XLua.LuaCallCSharp]
public delegate void ActionX();

[XLua.LuaCallCSharp]
public delegate void ActionX<T>(T par);

[XLua.CSharpCallLua]
public delegate void ActionXT(Transform par);

[XLua.LuaCallCSharp]
public delegate void ActionX<T, U>(T par1, U par2);

[XLua.LuaCallCSharp]
public delegate void ActionX<T, U, O>(T par1, U par2, O par32);