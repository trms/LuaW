using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaW;
using System.Runtime.InteropServices;
using System.Reflection;

namespace LuaWTests
{
    [TestClass]
    public class APIUnitTests
    {
        IntPtr L { get; set; }

        public APIUnitTests()
        {
            L = IntPtr.Zero;
        }

        public int Callback(IntPtr state)
        {
            return 0;
        }

        [TestInitialize]
        public void InitializeLua()
        {
            L = Lua.luaL_newstate();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (L != IntPtr.Zero)
                Lua.lua_close(L);
            L = IntPtr.Zero;
        }

        [TestMethod]
        public void luaL_newstate()
        {
            // there should be a state
            Assert.IsNotNull(L);
        }

        [TestMethod]
        public void lua_close()
        {
            // success -> this should not throw
            if (L != null)
                Lua.lua_close(L);
            L = IntPtr.Zero;
        }

        [TestMethod]
        public void lua_pushstring()
        {
            var s = Lua.lua_pushstring(L, "hello");

            Assert.AreEqual("hello", s);
        }

        [TestMethod]
        public void lua_gettop()
        {
            var s = Lua.lua_pushstring(L, "hello");

            var t = Lua.lua_gettop(L);

            Assert.AreEqual(1, t);
        }

        [TestMethod]
        public void lua_tostring()
        {
            var s = Lua.lua_pushstring(L, "hello");

            var s2 = Lua.lua_tostring(L, -1);

            Assert.AreEqual(s, s2);
        }

        [TestMethod]
        public void lua_type()
        {
            PopulateStack(new IntPtr());

            Assert.AreEqual(Lua.LUA_TNUMBER, Lua.lua_type(L, 1));
            Assert.AreEqual(Lua.LUA_TSTRING, Lua.lua_type(L, 2));
            Assert.AreEqual(Lua.LUA_TBOOLEAN, Lua.lua_type(L, 3));
            Assert.AreEqual(Lua.LUA_TLIGHTUSERDATA, Lua.lua_type(L, 5));
            Assert.AreEqual(Lua.LUA_TFUNCTION, Lua.lua_type(L, 6));
        }

        [TestMethod]
        public void lua_typename()
        {
            PopulateStack(new IntPtr());

            Assert.AreEqual("number", Lua.lua_typename(L, Lua.lua_type(L, 1)));
            Assert.AreEqual("string", Lua.lua_typename(L, Lua.lua_type(L, 2)));
            Assert.AreEqual("boolean", Lua.lua_typename(L, Lua.lua_type(L, 3)));
            Assert.AreEqual("userdata", Lua.lua_typename(L, Lua.lua_type(L, 5)));
            Assert.AreEqual("function", Lua.lua_typename(L, Lua.lua_type(L, 6)));
        }

        [TestMethod]
        public void lua_newthread()
        {
            // index must be >0
            var t = Lua.lua_newthread(L);
            Lua.DebugStackDump(L);
            Assert.AreNotEqual(L, t);
            Assert.AreEqual("thread", Lua.lua_typename(L, Lua.lua_type(L, Lua.lua_gettop(L))));
            Assert.IsTrue(Lua.lua_gettop(t) == 0);
        }

        [TestMethod]
        public void lua_version()
        {
            Double d = Lua.lua_version(L);
            Assert.IsTrue(d >= 503);
        }

        [TestMethod]
        public void lua_settop()
        {
            Lua.lua_pushstring(L, "1");
            Lua.lua_pushnumber(L, 1);
            Assert.AreEqual(2, Lua.lua_gettop(L));

            Lua.lua_settop(L, 1);
            Assert.AreEqual(1, Lua.lua_gettop(L) );
        }

        [TestMethod]
        public void lua_pushvalue()
        {
            Lua.lua_pushnumber(L, 1);
            Lua.lua_pushnumber(L, 2);
            Lua.lua_pushnumber(L, 3);
            Assert.AreEqual(3, Lua.lua_tonumber(L, -1));

            Lua.lua_pushvalue(L, 2);
            Assert.AreEqual(2, Lua.lua_tonumber(L, -1));
        }

        [TestMethod]
        public void lua_copy()
        {
            Lua.lua_pushnumber(L, 1);
            Lua.lua_pushnumber(L, 2);
            Lua.lua_pushnumber(L, 3);
            Assert.AreEqual(3, Lua.lua_tonumber(L, -1));

            Lua.lua_copy(L, 1, 3);
            Assert.AreEqual(1, Lua.lua_tonumber(L, -1));
        }

        [TestMethod]
        public void lua_xmove()
        {
            var t = Lua.lua_newthread(L);
            Assert.IsTrue(Lua.lua_gettop(t) == 0);

            Lua.lua_pushnumber(L, 1);
            Lua.lua_pushnumber(L, 2);
            Assert.AreEqual(2, Lua.lua_tonumber(L, -1));
            
            Lua.lua_xmove(L, t, 2);
            Assert.IsTrue(Lua.lua_gettop(L) == 1); // just the thread left
            Assert.AreEqual(2, Lua.lua_tonumber(t, -1));
        }

        private void PopulateStack(IntPtr p)
        {
            Lua.lua_pushnumber(L, 1);
            Lua.lua_pushstring(L, "hello");
            Lua.lua_pushboolean(L, 1);
            Lua.lua_pushnil(L);
            Lua.lua_pushlightuserdata(L, p);
            Lua.lua_pushcfunction(L, Callback);
            //Lua.lua_pushinteger(L, 1);
        }

        private void lua_is(String type, int index)
        {
            PopulateStack(new IntPtr());

            var m = typeof(Lua).GetMethod(type);
            
            var t = Lua.lua_gettop(L);

            for (var i = 1; i <= Lua.lua_gettop(L); i++)
            {
                Boolean? b = m.Invoke(L, new object[] { L, i }) as Boolean?;
                Assert.IsTrue(b.HasValue);

                if (i == index)
                    Assert.IsTrue(b.Value);
                else
                    Assert.IsFalse(b.Value);
            }
        }

        [TestMethod]
        public void lua_isnumber()
        {
            lua_is("lua_isnumber", 1);
        }

        [TestMethod]
        public void lua_isstring()
        {
            lua_is("lua_isstring", 2);
        }

        [TestMethod]
        public void lua_isboolean()
        {
            lua_is("lua_isboolean", 3);
        }

        [TestMethod]
        public void lua_isnil()
        {
            lua_is("lua_isnil", 4);
        }

        [TestMethod]
        public void lua_isuserdata()
        {
            lua_is("lua_isuserdata", 5);
        }

        [TestMethod]
        public void lua_iscfunction()
        {
            lua_is("lua_iscfunction", 6);
        }
    }
}