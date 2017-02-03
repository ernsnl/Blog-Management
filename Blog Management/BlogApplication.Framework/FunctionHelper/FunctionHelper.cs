using System.Diagnostics;

namespace BlogApplication.Framework.FunctionHelper
{
    public static class FunctionHelper
    {
        public static string  getFunctionInfo(StackTrace st)
        {
            return st.GetFrame(0).GetMethod().Name;
        }
    }
}