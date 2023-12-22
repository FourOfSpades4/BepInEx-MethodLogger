using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using MagicalActual;

namespace MethodCallTracker
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class MethodCallTrackerPlugin : BasePlugin
    {
        private const string MyGUID = "com.spades.bepinex.methodtracker";
        private const string PluginName = "MethodCallTracker";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Logger = new ManualLogSource(PluginName);



        private static (
            Type type, 
            int minParamaters, 
            string[] invalidMethods,
            bool whitelist)[] types = { 
            //    (typeof(PlayerCharacter), 1, new string[]{ "FixedUpdate" }, false)   EXAMPLE
            };


        public static bool Prefix(MethodBase __originalMethod, object[] __args)
        {
            Logger.LogInfo(
                String.Format("{0}.{1} ({2})",
                    __originalMethod.ReflectedType.Name,
                    __originalMethod.Name,
                    String.Join(", ", __args)));

            return true;
        }

        public static bool ParametersHasType(ParameterInfo[] parameters, Type type)
        {
            return parameters.Aggregate(false,
                    (res, val) =>
                        res || val.ParameterType == type);
        }

        public override void Load()
        {
            Log.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");

            var mPrefix = SymbolExtensions.GetMethodInfo(
                (MethodBase __originalMethod, object[] __args)
                    => Prefix(__originalMethod, __args));

            foreach (var c in types) {
                foreach (var method in c.type.GetMethods(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    var parameters = method.GetParameters();

                    if (c.whitelist == c.invalidMethods.Aggregate(false,
                        (res, val) =>
                            res || method.Name.Contains(val)) &&
                        parameters.Length >= c.minParamaters)
                    {
                        Log.LogInfo(String.Format("Patching Method: {0}.{1}", method.DeclaringType, method.Name));
                        Harmony.Patch(method, new HarmonyMethod(mPrefix));
                    }
                }
            }


            Logger = Log;
        }
    }
}
