namespace ConsoleApplication1
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class MethodInvoker
    {
        public void Invoke(object instance)
        {
            MethodInfo[] methodInfos = instance.GetType().GetMethods();

            var methodsToexecute = 
                from methodInfo in methodInfos
                where methodInfo.GetCustomAttributes(typeof(InvokeAttribute), true).Count() > 0
                select methodInfo;

            foreach (var methodInfo in methodsToexecute)
            {
                methodInfo.Invoke(instance, null);
            }
        }

        public void Invoke(Type type)
        {
            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            var methodsToexecute =
                from methodInfo in methodInfos
                where methodInfo.GetCustomAttributes(typeof(InvokeAttribute), true).Count() > 0
                select methodInfo;

            var instance = Activator.CreateInstance(type);

            foreach (var methodInfo in methodsToexecute)
            {
                methodInfo.Invoke(instance, null);
            }
        }
    }
}