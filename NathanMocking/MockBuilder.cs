using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace NathanMocking
{
  public static class MockBuilder
  {
    public static dynamic CreateMock<TClassToBeMocked>()
    {
      var myType = CompileResultType<TClassToBeMocked>();
      var ret = Activator.CreateInstance(myType);
      return ret;
    }

    private static Type CompileResultType<TClassToBeMocked>()
    {
      var methods = GetMethods<TClassToBeMocked>();

      TypeBuilder tb = GetTypeBuilder<TClassToBeMocked>();
      tb.DefineDefaultConstructor(MethodAttributes.Private | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

      var constructor = tb.DefineConstructor(
        MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
        CallingConventions.Standard,
        null);

      var constructorILGenerator = constructor.GetILGenerator();
      constructorILGenerator.Emit(OpCodes.Ldarg_0);
      var objectCtor = typeof(TClassToBeMocked).GetConstructor(Type.EmptyTypes);
      constructorILGenerator.Emit(OpCodes.Call, objectCtor);
      constructorILGenerator.Emit(OpCodes.Ret);

      foreach (var method in methods)
      {
        var parameters = method.ParameterTypes.Select(p => p.ParameterType).Select(Expression.Parameter).ToArray();
        var body = Expression.Default(method.ReturnType);
        var lambda = Expression.Lambda(body, false, parameters);
        var methodType = lambda.Type;

        var field = tb.DefineField($"{method.Name}PropertyBacker", methodType, FieldAttributes.Private);
        var propertyBuilder = tb.DefineProperty($"{method.Name}Func", PropertyAttributes.None, methodType, null);

        var propertyGet = tb.DefineMethod($"get_{method.Name}Func", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, methodType, Type.EmptyTypes);
        var getterIL = propertyGet.GetILGenerator();
        getterIL.Emit(OpCodes.Ldarg_0);
        getterIL.Emit(OpCodes.Ldfld, field);
        getterIL.Emit(OpCodes.Ret);
        propertyBuilder.SetGetMethod(propertyGet);

        var propertySet = tb.DefineMethod($"set_{method.Name}Func", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { methodType });
        var setterIL = propertySet.GetILGenerator();
        setterIL.Emit(OpCodes.Ldarg_0);
        setterIL.Emit(OpCodes.Ldarg_1);
        setterIL.Emit(OpCodes.Stfld, field);
        setterIL.Emit(OpCodes.Ret);
        propertyBuilder.SetSetMethod(propertySet);

        var mockedMethod = tb.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.HideBySig, method.ReturnType, method.ParameterTypes.Select(p => p.ParameterType).ToArray());
        var mockedMethodIL = mockedMethod.GetILGenerator();
        mockedMethodIL.Emit(OpCodes.Ldarg_0);
        mockedMethodIL.Emit(OpCodes.Call, propertyBuilder.GetGetMethod());

        for (var i = 0; i < method.ParameterTypes.Length; i++)
        {
          mockedMethodIL.Emit(OpCodes.Ldarg, i + 1);
        }

        mockedMethodIL.Emit(OpCodes.Callvirt, methodType.GetMethod("Invoke"));
        mockedMethodIL.Emit(OpCodes.Ret);
      }

      Type objectType = tb.CreateType();
      return objectType;
    }

    private static IEnumerable<Method> GetMethods<TClassToBeMocked>()
    {
      return typeof(TClassToBeMocked)
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Select(methodToBeMocked => new Method(methodToBeMocked.Name, methodToBeMocked.ReturnType, methodToBeMocked.GetParameters()));
    }

    private static TypeBuilder GetTypeBuilder<TClassToBeMocked>()
    {
      var an = new AssemblyName($"NathanMockedAssembly");
      AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

      ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule($"NathanMockeAssembly.Mock{typeof(TClassToBeMocked).Name}");

      TypeBuilder tb = moduleBuilder.DefineType($"NathanMockeAssembly.Mock{typeof(TClassToBeMocked).Name}", typeof(TClassToBeMocked).Attributes, typeof(TClassToBeMocked));
      return tb;
    }
  }
}