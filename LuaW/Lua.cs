using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LuaW
{
    // https://ttuxen.wordpress.com/2009/11/03/embedding-lua-in-dotnet/
    public static class Lua
    {
        /*
        ** basic types
        */
        const int LUA_TNONE = (-1);

        const int LUA_TNIL = 0;
        const int LUA_TBOOLEAN = 1;
        const int LUA_TLIGHTUSERDATA = 2;
        const int LUA_TNUMBER = 3;
        const int LUA_TSTRING = 4;
        const int LUA_TTABLE = 5;
        const int LUA_TFUNCTION = 6;
        const int LUA_TUSERDATA = 7;
        const int LUA_TTHREAD = 8;

        const int LUAI_MAXSTACK = 1000000;
        const int LUAI_FIRSTPSEUDOIDX = (-LUAI_MAXSTACK - 1000);
        const int LUA_REGISTRYINDEX = LUAI_FIRSTPSEUDOIDX;

        /* predefined values in the registry */
        const int LUA_RIDX_MAINTHREAD = 1;
        const int LUA_RIDX_GLOBALS = 2;
        const int LUA_RIDX_LAST = LUA_RIDX_GLOBALS;

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

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isnumber(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isstring(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_iscfunction(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isinteger(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isuserdata(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_type(IntPtr L, int idx);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern String lua_typename(IntPtr L, int tp);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double lua_tonumberx(IntPtr L, int idx, IntPtr isnum);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 lua_tointegerx(IntPtr L, int idx, IntPtr isnum);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_toboolean(IntPtr L, int idx);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr lua_tolstring(IntPtr L, int idx, out uint len);

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
        
        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern String lua_pushlstring(IntPtr L, String s, Int32 len);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr lua_pushstring(IntPtr L, String s);

        //public static extern  String lua_pushvfstring (IntPtr L, String fmt, va_list argp);
        //public static extern  String (lua_pushfstring) (IntPtr L, String fmt, ...);
        //public static extern  void  (lua_pushcclosure) (IntPtr L, lua_CFunction fn, int n);
        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushboolean(IntPtr L, int b);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushlightuserdata(IntPtr L, IntPtr p);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_pushthread(IntPtr L);


        /*
        ** get functions (Lua -> stack)
        */
        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int lua_getglobal(IntPtr L, String name);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gettable(IntPtr L, int idx);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
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
        [DllImport("lua53.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setglobal(IntPtr L, String name);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settable(IntPtr L, int idx);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
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
            lua_callk(L, (n), (r), IntPtr.Zero, IntPtr.Zero);
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

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
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

        //#define lua_pushcfunction(L,f)	lua_pushcclosure(L, (f), 0)
        
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

        public static IntPtr lua_tostring(IntPtr L, int i)
        {
            uint j;
            return lua_tolstring(L, (i), out j);
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
        public static extern void luaL_checkversion(IntPtr L, Double ver, Int32 sz);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_checkversion(IntPtr L);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_getmetafield(IntPtr L, int obj, String e);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_callmeta(IntPtr L, int obj, String e);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern String luaL_tolstring(IntPtr L, int idx, Int32 len);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_argerror(IntPtr L, int arg, String extramsg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern String luaL_checklstring(IntPtr L, int arg, Int32 l);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern String luaL_optlstring(IntPtr L, int arg, String def, Int32 l);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Double luaL_checknumber(IntPtr L, int arg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Double luaL_optnumber(IntPtr L, int arg, Double def);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 luaL_checkinteger(IntPtr L, int arg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 luaL_optinteger(IntPtr L, int arg, Int64 def);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern void luaL_checkstack(IntPtr L, int sz, String msg);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_checktype(IntPtr L, int arg, int t);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_checkany(IntPtr L, int arg);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_newmetatable(IntPtr L, String tname);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern void luaL_setmetatable(IntPtr L, String tname);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr luaL_testudata(IntPtr L, int ud, String tname);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr luaL_checkudata(IntPtr L, int ud, String tname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_where(IntPtr L, int lvl);

        //[DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        //public static extern int luaL_error (IntPtr L, String fmt, ...);

        //[DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        //public static extern int luaL_checkoption (IntPtr L, int arg, String def, String lst[]);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_fileresult(IntPtr L, int stat, String fname);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_execresult(IntPtr L, int stat);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_ref(IntPtr L, int t);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_unref(IntPtr L, int t, int r);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_loadfilex(IntPtr L, String filename, String mode);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_loadfile(IntPtr L, String f);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_loadbufferx(IntPtr L, String buff, Int32 sz, String name, String mode);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_loadstring(IntPtr L, String s);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 luaL_len(IntPtr L, int idx);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern String luaL_gsub(IntPtr L, String s, String p, String r);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_getsubtable(IntPtr L, int idx, String fname);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern void luaL_traceback(IntPtr L, IntPtr L1, String msg, int level);

        [DllImport("lua53.dll", CharSet = CharSet.Ansi)]
        public static extern int luaL_dostring(IntPtr L, String s);

        [DllImport("lua53.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_getmetatable(IntPtr L, int n);
    }
}