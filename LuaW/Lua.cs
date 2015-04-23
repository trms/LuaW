/*
Copyright (c) 2015, Tightrope Media Systems
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LuaW
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int lua_CFunction(IntPtr state);

    // https://ttuxen.wordpress.com/2009/11/03/embedding-lua-in-dotnet/
    public static class Lua
    {
        /*
        ** basic types
        */
        public const int LUA_TNONE = (-1);

        public const int LUA_TNIL = 0;
        public const int LUA_TBOOLEAN = 1;
        public const int LUA_TLIGHTUSERDATA = 2;
        public const int LUA_TNUMBER = 3;
        public const int LUA_TSTRING = 4;
        public const int LUA_TTABLE = 5;
        public const int LUA_TFUNCTION = 6;
        public const int LUA_TUSERDATA = 7;
        public const int LUA_TTHREAD = 8;

        public const int LUAI_MAXSTACK = 1000000;
        public const int LUAI_FIRSTPSEUDOIDX = (-LUAI_MAXSTACK - 1000);
        public const int LUA_REGISTRYINDEX = LUAI_FIRSTPSEUDOIDX;

        /* predefined values in the registry */
        public const int LUA_RIDX_MAINTHREAD = 1;
        public const int LUA_RIDX_GLOBALS = 2;
        public const int LUA_RIDX_LAST = LUA_RIDX_GLOBALS;

        /*
         * lua
         */
        //public static extern IntPtr lua_newstate (lua_Alloc f, void *ud);
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_close(IntPtr L);
        
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_newthread(IntPtr L);

        //public static extern lua_CFunction (lua_atpanic) (IntPtr L, lua_CFunction panicf);
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double lua_version(IntPtr L);

        /*
        ** basic stack manipulation
        */
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_absindex(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gettop(IntPtr L);
        
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settop(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushvalue(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rotate(IntPtr L, int idx, int n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_copy(IntPtr L, int fromidx, int toidx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_checkstack(IntPtr L, int n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_xmove(IntPtr from, IntPtr to, int n);


        /*
        ** access functions (stack -> C)
        */

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="lua_isnumber")]
        public static extern int _lua_isnumber(IntPtr L, int idx);
        public static Boolean lua_isnumber(IntPtr L, int idx)
        {
            return _lua_isnumber(L, idx) == 1;
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="lua_isstring")]
        public static extern int _lua_isstring(IntPtr L, int idx);
        public static Boolean lua_isstring(IntPtr L, int idx)
        {
            return _lua_isstring(L, idx) == 1;
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="lua_iscfunction")]
        public static extern int _lua_iscfunction(IntPtr L, int idx);
        public static Boolean lua_iscfunction(IntPtr L, int idx)
        {
            return _lua_iscfunction(L, idx) == 1;
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="lua_isinteger")]
        public static extern int _lua_isinteger(IntPtr L, int idx);
        public static Boolean lua_isinteger(IntPtr L, int idx)
        {
            return _lua_iscfunction(L, idx) == 1;
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="lua_isuserdata")]
        public static extern int _lua_isuserdata(IntPtr L, int idx);
        public static Boolean lua_isuserdata(IntPtr L, int idx)
        {
            return _lua_isuserdata(L, idx) == 1;
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_type(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_typename")]
        public static extern IntPtr _lua_typename(IntPtr L, int tp);
        public static String lua_typename(IntPtr L, int tp)
        {
            IntPtr p = L;
            int t = tp;
            return Marshal.PtrToStringAnsi(_lua_typename(p, t));
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double lua_tonumberx(IntPtr L, int idx, IntPtr isnum);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 lua_tointegerx(IntPtr L, int idx, IntPtr isnum);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_toboolean(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "lua_tolstring")]
        public static extern IntPtr _lua_tolstring(IntPtr L, int idx, ref uint len);
        public static String lua_tolstring(IntPtr L, int idx, ref uint len)
        {
            IntPtr p = L;
            int i = idx;
            uint l = len;
            String s = Marshal.PtrToStringAnsi(_lua_tolstring(p, i, ref l));
            len = l;
            return s;
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint lua_rawlen(IntPtr L, int idx);
        //public static extern  lua_CFunction   (lua_tocfunction) (IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_touserdata(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_tothread(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_topointer(IntPtr L, int idx);

        /*
        ** Comparison and arithmetic functions
        */
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_arith(IntPtr L, int op);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_rawequal(IntPtr L, int idx1, int idx2);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_compare(IntPtr L, int idx1, int idx2, int op);


        /*
        ** push functions (C -> stack)
        */

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnil(IntPtr L);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnumber(IntPtr L, double n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushinteger(IntPtr L, Int64 n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "lua_pushlstring")]
        public static extern IntPtr _lua_pushlstring(IntPtr L, String s, uint len);
        public static String lua_pushlstring(IntPtr L, String s, uint len)
        {
            IntPtr p = L;
            String s1 = s;
            uint l = len;
            return Marshal.PtrToStringAnsi(_lua_pushlstring(p, s1, l));
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "lua_pushstring")]
        public static extern IntPtr _lua_pushstring(IntPtr L, String s);
        public static String lua_pushstring(IntPtr L, String s)
        {
            IntPtr p = L;
            String s1 = s;
            return Marshal.PtrToStringAnsi(_lua_pushstring(p, s1));
        }

        //public static extern  String lua_pushvfstring (IntPtr L, String fmt, va_list argp);
        //public static extern  String (lua_pushfstring) (IntPtr L, String fmt, ...);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void lua_pushcclosure(IntPtr state, lua_CFunction fn, int n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushboolean(IntPtr L, int b);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushlightuserdata(IntPtr L, IntPtr p);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_pushthread(IntPtr L);


        /*
        ** get functions (Lua -> stack)
        */
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lua_getglobal(IntPtr L, String name);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gettable(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lua_getfield(IntPtr L, int idx, String k);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_geti(IntPtr L, int idx, Int64 n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_rawget(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_rawgeti(IntPtr L, int idx, Int64 n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_rawgetp(IntPtr L, int idx, IntPtr p);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_createtable(IntPtr L, int narr, int nrec);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_newuserdata(IntPtr L, uint sz);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_getmetatable(IntPtr L, int objindex);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_getuservalue(IntPtr L, int idx);

        /*
        ** set functions (stack -> Lua)
        */
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void lua_setglobal(IntPtr L, String name);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settable(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void lua_setfield(IntPtr L, int idx, String k);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_seti(IntPtr L, int idx, Int64 n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawset(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawseti(IntPtr L, int idx, Int64 n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawsetp(IntPtr L, int idx, IntPtr p);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_setmetatable(IntPtr L, int objindex);
        
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setuservalue(IntPtr L, int idx);


        /*
        ** 'load' and 'call' functions (load and run Lua code)
        */
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_callk (IntPtr L, int nargs, int nresults, IntPtr ctx, IntPtr k);
        public static void lua_call(IntPtr L, int n, int r)
        {
            lua_callk(L, n, r, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_pcallk (IntPtr L, int nargs, int nresults, int errfunc, IntPtr ctx, IntPtr k);
        public static int lua_pcall(IntPtr L, int n, int r, int f)
        {
            return lua_pcallk(L, (n), (r), (f), IntPtr.Zero, IntPtr.Zero);
        }

        //public static extern  int   lua_load (IntPtr L, lua_Reader reader, void *dt, String chunkname, String mode);
        //public static extern  int lua_dump (IntPtr L, lua_Writer writer, void *data, int strip);

        /*
        ** coroutine functions
        */
        //public static extern  int  lua_yieldk     (IntPtr L, int nresults, lua_KContext ctx, lua_KFunction k);
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_resume(IntPtr L, IntPtr from, int narg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_status(IntPtr L);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isyieldable(IntPtr L);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_yieldk(IntPtr L, int n, int i, IntPtr k);

        static public int lua_yield(IntPtr L, int n)
        {
            return lua_yieldk(L, n, 0, IntPtr.Zero);
        }

        /*
        ** garbage-collection function and options
        */

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gc(IntPtr L, int what, int data);


        /*
        ** miscellaneous functions
        */

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_error(IntPtr L);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_next(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_concat(IntPtr L, int n);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_len(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint lua_stringtonumber(IntPtr L, String s);

        //public static extern  lua_Alloc lua_getallocf (IntPtr L, IntPtr ud);
        //public static extern  void      lua_setallocf (IntPtr L, lua_Alloc f, void *ud);



        /*
        ** {==============================================================
        ** some useful macros
        ** ===============================================================
        */

        public static double lua_tonumber(IntPtr L, int i)
        {
            return lua_tonumberx(L, i, IntPtr.Zero);
        }

        public static Int64 lua_tointeger(IntPtr L, int i)
        {
            return lua_tointegerx(L, (i), IntPtr.Zero);
        }

        public static void lua_pop(IntPtr L, int n)
        {
            lua_settop(L, -(n) - 1);
        }

        public static void lua_newtable(IntPtr L)
        {
            lua_createtable(L, 0, 0);
        }

        //#define lua_register(L,n,f) (lua_pushcfunction(L, (f)), lua_setglobal(L, (n)))

        public static void lua_pushcfunction(IntPtr L, lua_CFunction f)
        {
            lua_pushcclosure(L, f, 0);
        }
        
        public static Boolean lua_isfunction(IntPtr L, int n)
        {
            return (lua_type(L, (n)) == LUA_TFUNCTION);
        }

        public static Boolean lua_istable(IntPtr L, int n)
        {
            return (lua_type(L, (n)) == LUA_TTABLE);
        }

        public static Boolean lua_islightuserdata(IntPtr L, int n)
        {
            return (lua_type(L, (n)) == LUA_TLIGHTUSERDATA);
        }

        public static Boolean lua_isnil(IntPtr L, int n)
        {
            return (lua_type(L, (n)) == LUA_TNIL);
        }

        public static Boolean lua_isboolean(IntPtr L, int n)
        {
            return (lua_type(L, (n)) == LUA_TBOOLEAN);
        }

        public static Boolean lua_isthread(IntPtr L, int n)
        {
            return (lua_type(L, (n)) == LUA_TTHREAD);
        }

        public static Boolean lua_isnone(IntPtr L, int n)
        {
            return (lua_type(L, (n)) == LUA_TNONE);
        }

        public static Boolean lua_isnoneornil(IntPtr L, int n)
        {
            return (lua_type(L, (n)) <= 0);
        }

        public static int lua_pushglobaltable(IntPtr L)
        {
            return lua_rawgeti(L, LUA_REGISTRYINDEX, LUA_RIDX_GLOBALS);
        }

        public static String lua_tostring(IntPtr L, int i)
        {
            uint j = 0;
            return lua_tolstring(L, i, ref j);
        }

        public static void lua_insert(IntPtr L, int idx)
        {
            lua_rotate(L, (idx), 1);
        }

        public static void lua_remove(IntPtr L, int idx)
        {
            lua_rotate(L, (idx), -1);
            lua_pop(L, 1);
        }

        public static void lua_replace(IntPtr L, int idx)
        {
            lua_copy(L, -1, (idx));
            lua_pop(L, 1);
        }

        /*
         * lauxlib
         */
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr luaL_newstate();

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_checkversion(IntPtr L, Double ver, uint sz);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_checkversion(IntPtr L);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_getmetafield(IntPtr L, int obj, String e);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_callmeta(IntPtr L, int obj, String e);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_tolstring")]
        public static extern IntPtr _luaL_tolstring(IntPtr L, int idx, uint len);
        public static String luaL_tolstring(IntPtr L, int idx, uint len)
        {
            IntPtr p = L;
            int i = idx;
            uint l = len;
            return Marshal.PtrToStringAnsi(_luaL_tolstring(p, i, l));
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_argerror(IntPtr L, int arg, String extramsg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_checklstring")]
        public static extern IntPtr _luaL_checklstring(IntPtr L, int arg, uint l);
        public static String luaL_checklstring(IntPtr L, int arg, uint l)
        {
            IntPtr p = L;
            int a = arg;
            uint len = l;
            return Marshal.PtrToStringAnsi(_luaL_checklstring(p, a, len));
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_optlstring")]
        public static extern IntPtr _luaL_optlstring(IntPtr L, int arg, String def, uint l);
        public static String luaL_optlstring(IntPtr L, int arg, String def, uint l)
        {
            IntPtr p = L;
            int a = arg;
            String d = def;
            uint len = l;
            return Marshal.PtrToStringAnsi(_luaL_optlstring(p, a, d, len));
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Double luaL_checknumber(IntPtr L, int arg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Double luaL_optnumber(IntPtr L, int arg, Double def);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 luaL_checkinteger(IntPtr L, int arg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 luaL_optinteger(IntPtr L, int arg, Int64 def);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void luaL_checkstack(IntPtr L, int sz, String msg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_checktype(IntPtr L, int arg, int t);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_checkany(IntPtr L, int arg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_newmetatable(IntPtr L, String tname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void luaL_setmetatable(IntPtr L, String tname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr luaL_testudata(IntPtr L, int ud, String tname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr luaL_checkudata(IntPtr L, int ud, String tname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_where(IntPtr L, int lvl);

        //[DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        //public static extern int luaL_error (IntPtr L, String fmt, ...);

        //[DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        //public static extern int luaL_checkoption (IntPtr L, int arg, String def, String lst[]);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_fileresult(IntPtr L, int stat, String fname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_execresult(IntPtr L, int stat);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_ref(IntPtr L, int t);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_unref(IntPtr L, int t, int r);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_loadfilex(IntPtr L, String filename, String mode);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_loadfile(IntPtr L, String f);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_loadbufferx(IntPtr L, String buff, uint sz, String name, String mode);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_loadstring(IntPtr L, String s);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 luaL_len(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_gsub")]
        public static extern IntPtr _luaL_gsub(IntPtr L, String s, String p, String r);
        public static String luaL_gsub(IntPtr L, String s, String p, String r)
        {
            IntPtr L1 = L;
            String s1 = s;
            String p1 = p;
            String r1 = r;
            return Marshal.PtrToStringAnsi(_luaL_gsub(L1, s1, p1, r1));
        }

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_getsubtable(IntPtr L, int idx, String fname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void luaL_traceback(IntPtr L, IntPtr L1, String msg, int level);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int luaL_dostring(IntPtr L, String s);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_getmetatable(IntPtr L, int n);

        /*
         * Helpers
         */
        public static void DebugStackDump(IntPtr L)
        {
            int i;
            int top = Lua.lua_gettop(L);
            String str = String.Format("\nStack {0}:", L);
            Debug.WriteLine(str);
            Console.WriteLine(str);
            for (i = 1; i <= top; i++)
            {  /* repeat for each level */
                int t = Lua.lua_type(L, i);
                switch (t)
                {

                    case Lua.LUA_TSTRING:  /* strings */
                        str = String.Format("L {0}: `{1}' --> string", i, Lua.lua_tostring(L, i));
                        break;

                    case Lua.LUA_TBOOLEAN:  /* booleans */
                        str = String.Format("L {0}: {1} --> boolean", i, Lua.lua_toboolean(L, i) == 1 ? "true" : "false");
                        break;

                    case Lua.LUA_TNUMBER:  /* numbers */
                        str = String.Format("L {0}: {1} --> number", i, Lua.lua_tonumber(L, i));
                        break;

                    case Lua.LUA_TUSERDATA:  /* user data */
                        str = String.Format("L {0}: {1} --> userdata", i, Lua.lua_touserdata(L, i));
                        break;

                    case Lua.LUA_TLIGHTUSERDATA:  /* light user data */
                        str = String.Format("L {0}: {1} --> lightuserdata", i, Lua.lua_touserdata(L, i));
                        break;

                    default:  /* other values */
                        str = String.Format("L {0}: {1}", i, Lua.lua_typename(L, t));
                        break;
                }
                Debug.WriteLine(str);
                Console.WriteLine(str);
            }
        }

    }
}