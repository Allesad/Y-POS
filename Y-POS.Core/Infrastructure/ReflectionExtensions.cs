using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Y_POS.Core.Infrastructure
{
    public static class ReflectionExtensions
    {
        public static bool IsNullableType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsVoid(this Type type)
        {
            return type == typeof(void);
        }

        public static string GetGenericTypeSignature(this Type type)
        {
            if (type.IsNullableType())
            {
                return type.GenericTypeArguments[0].Name + "?";
            }
            StringBuilder sb = new StringBuilder(type.GetTypeInfo().BaseType.FullName);
            sb.Append("<");
            foreach (Type t in type.GenericTypeArguments)
            {
                sb.Append(t.IsGenericParameter ? t.GetGenericTypeSignature() : t.FullName);
            }
            sb.Append(">");
            return sb.ToString();
        }

        public static string GenerateMethodSignature(this MethodInfo method, bool isAsync = true)
        {
            if (method.IsSpecialName && method.Name.StartsWith("set_") || method.Name.StartsWith("get_"))
            {
                throw new ArgumentException("Method does not accept getters and setters methods!");
            }

            StringBuilder sb = new StringBuilder(GetMethodAccessModifier(method));
            sb.Append(isAsync ? " async " : " ");
            sb.Append(method.GetMethodReturnType());
            sb.Append(" ");
            sb.Append(method.GetMethodName());
            sb.Append("(");
            sb.Append(method.GetMethodParameters());
            sb.Append(")");

            return sb.ToString();
        }

        public static string GetMethodAccessModifier(this MethodInfo method)
        {
            return "public";
        }

        public static string GetMethodReturnType(this MethodInfo method)
        {
            if (method.ReturnType.GetTypeInfo().IsGenericType)
            {
                return GetGenericTypeSignature(method.ReturnType);
            }
            return method.ReturnType.IsVoid() ? "void" : method.ReturnType.GetTypeInfo().BaseType.FullName;
        }

        public static string GetMethodName(this MethodInfo method)
        {
            return method.Name;
        }

        public static string GetMethodParameters(this MethodInfo method)
        {
            ParameterInfo[] p = method.GetParameters();
            string[] parameters = new string[p.Length];

            foreach (ParameterInfo info in p.OrderBy(i => i.Position))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(info.ParameterType.GetTypeInfo().IsGenericType
                    ? info.ParameterType.GetGenericTypeSignature()
                    : info.ParameterType.FullName);
                sb.Append(" ");
                sb.Append(info.Name);
                parameters[info.Position] = sb.ToString();
            }
            return string.Join(", ", parameters);
        }

        public static string GetMethodBody(this MethodInfo method, string template, string placeholder)
        {
            StringBuilder sb = new StringBuilder(method.Name);
            sb.Append("(");
            sb.Append(string.Join(", ", method.GetParameters().Select(p => p.Name)));
            sb.Append(")");
            return template.Replace(placeholder, sb.ToString());
        }
    }
}
