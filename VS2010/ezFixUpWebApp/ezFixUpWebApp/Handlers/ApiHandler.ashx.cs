using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for ApiHandler
    /// </summary>
    public class ApiHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Dictionary<string, object> dicParams = new Dictionary<string, object>();
            var keys = context.Request.Params.AllKeys.Where(k=>k!=null);
            foreach (var key in keys)
            {
                dicParams.Add(key, context.Request.Params[key]);
            }
            object res = MethodInvoker(dicParams);
            Type type1 = res.GetType();
            string retval = JsonConvert.SerializeObject(Convert.ChangeType(res, type1));
            context.Response.ContentType = "application/json";
            context.Response.Write(retval);
        }

        private object MethodInvoker(Dictionary<string,object> parameters)
        {
            Type type1 = Type.GetType("ezFixUp.Classes."+parameters["invoker"]);
            object obj = Activator.CreateInstance(type1);
            PropertyInfo[] arrPropInfo = type1.GetProperties();
            var paramsKeys = parameters.Keys;
            foreach (var pi in arrPropInfo)
	        {
                if (paramsKeys.Contains(pi.Name))
                {
                    if (pi.PropertyType.IsEnum)
                    {
                        var enumVal = Enum.Parse(pi.PropertyType, (string)parameters[pi.Name], true);
                        pi.SetValue(obj, enumVal, null);
                    }
                    else
                    {
                        Type t = pi.PropertyType;
                        Type u = Nullable.GetUnderlyingType(t);

                        if (u != null)
                        { 
                            if (parameters[pi.Name] == null)
                                pi.SetValue(obj, null, null); //default(t);
                            else
                                pi.SetValue(obj, Convert.ChangeType(parameters[pi.Name], u), null);                            
                        }
                        else
                        {
                            pi.SetValue(obj, Convert.ChangeType(parameters[pi.Name], t), null);
                        }
                        
                    }
                }
            }
            
            string methodName = (string)parameters["command"];
            MethodInfo m = type1.GetMethod(methodName);
            ParameterInfo[] arrParametersInfo = m.GetParameters().OrderBy(i => i.Position).ToArray();
            List<object> lstParameters = new List<object>();
            foreach (var p in arrParametersInfo)
            {
                lstParameters.Add(parameters[p.Name]);
            }
            object res = type1.InvokeMember(methodName, BindingFlags.InvokeMethod,
                                               null, obj, lstParameters.ToArray());
            return res;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        
    }
}