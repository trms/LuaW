using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaW;
using System.Runtime.InteropServices;

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
            var s = Marshal.PtrToStringAnsi(Lua.lua_pushstring(L, "hello"));

            Assert.AreEqual(s, "hello");
        }

        [TestMethod]
        public void lua_gettop()
        {
            var s = Marshal.PtrToStringAnsi(Lua.lua_pushstring(L, "hello"));

            var t = Lua.lua_gettop(L);

            Assert.AreEqual(t, 1);
        }

        [TestMethod]
        public void lua_tostring()
        {
            var s = Marshal.PtrToStringAnsi(Lua.lua_pushstring(L, "hello"));

            var s2 = Marshal.PtrToStringAnsi(Lua.lua_tostring(L, -1));

            Assert.AreEqual(s, s2);
        }
    }
}