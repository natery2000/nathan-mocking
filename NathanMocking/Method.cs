using System;
using System.Linq;
using System.Reflection;

namespace NathanMocking
{
  internal class Method
  {
    internal string Name { get; }
    internal Type ReturnType { get; }
    internal ParameterInfo[] ParameterTypes { get; }
    internal Type ConstructorType { get; }

    internal Method(string name, Type returnType, ParameterInfo[] parameterTypes)
    {
      Name = name;
      ReturnType = returnType;
      ParameterTypes = parameterTypes;

      ConstructorType = returnType == null ? typeof(Func<>) : typeof(Action<>);
      var parameters = parameterTypes?.Select(p => p.ParameterType).ToArray();
      if (parameters?.Length > 0)
        ConstructorType = ConstructorType.MakeGenericType(parameters);
    }
  }
}