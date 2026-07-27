namespace LCSoft.Framework.Scripting
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Scripting.ValueContainer;

    public static class ScriptingConfiguration
    {
        public delegate void BreakpointEvent(Context Context, String ID, List<ValueContainerBase> Values);

        internal static BreakpointEvent BreakpointHandler;

        public static void SetBreakpointHandler(BreakpointEvent BreakpointHandler)
        {
            ScriptingConfiguration.BreakpointHandler = BreakpointHandler;
        }
    }
}