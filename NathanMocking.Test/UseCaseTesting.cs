using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NathanMocking.Test
{
  [TestClass]
  public class UseCaseTesting
  {
    [TestMethod]
    public void MockingFuncString()
    {
      var classThatCanBeMockedFuncString = new ClassThatCanBeMocked();
      Assert.AreEqual("FuncString not mocked", classThatCanBeMockedFuncString.FuncString());

      var classThatWasMockedFuncString = MockBuilder.CreateMock<ClassThatCanBeMocked>();
      classThatWasMockedFuncString.FuncStringFunc = (Func<string>)(() => "FuncString was mocked");
      Assert.AreEqual("FuncString was mocked", classThatWasMockedFuncString.FuncString());
    }

    [TestMethod]
    public void MockingFuncStringString()
    { 
      var classThatCanBeMockedFuncStringString = new ClassThatCanBeMocked();
      var funcStringStringFirstParam = "param";
      Assert.AreEqual($"FuncStringString not mocked with {funcStringStringFirstParam}", classThatCanBeMockedFuncStringString.FuncStringString(funcStringStringFirstParam));

      var classThatWasMockedFuncStringString = MockBuilder.CreateMock<ClassThatCanBeMocked>();
      classThatWasMockedFuncStringString.FuncStringStringFunc = (Func<string, string>)((param) => $"FuncStringString mocked with {param}");
      var mockFuncStringStringFirstParam = "mocked param";
      Assert.AreEqual($"FuncStringString mocked with {mockFuncStringStringFirstParam}", classThatWasMockedFuncStringString.FuncStringString(mockFuncStringStringFirstParam));
    }

    [TestMethod]
    public void MockingFuncIntString()
    {
      var classThatCanBeMockedFuncStringString = new ClassThatCanBeMocked();
      var funcIntStringFirstParam = 2;
      Assert.AreEqual($"FuncIntString not mocked with {funcIntStringFirstParam}", classThatCanBeMockedFuncStringString.FuncIntString(funcIntStringFirstParam));

      var classThatWasMockedFuncIntString = MockBuilder.CreateMock<ClassThatCanBeMocked>();
      classThatWasMockedFuncIntString.FuncIntStringFunc = (Func<int, string>)((param) => $"FuncIntString mocked with {param}");
      var mockFuncIntStringFirstParam = 2;
      Assert.AreEqual($"FuncIntString mocked with {mockFuncIntStringFirstParam}", classThatWasMockedFuncIntString.FuncIntString(mockFuncIntStringFirstParam));
    }

    [TestMethod]
    public void MockingFuncStringInt()
    {
      var classThatCanBeMockedFuncOfStringInt = new ClassThatCanBeMocked();
      var funcOfStringIntFirstParam = "param";
      Assert.AreEqual(1, classThatCanBeMockedFuncOfStringInt.FuncStringInt(funcOfStringIntFirstParam));

      var classThatWasMockedFuncOfStringInt = MockBuilder.CreateMock<ClassThatCanBeMocked>();
      var mockFuncOfStringIntFirstParam = "param";
      classThatWasMockedFuncOfStringInt.FuncStringIntFunc = (Func<string, int>)((param) => param == mockFuncOfStringIntFirstParam ? 2 : -1);
      Assert.AreEqual(2, classThatWasMockedFuncOfStringInt.FuncStringInt(mockFuncOfStringIntFirstParam));
    }
  }
}
