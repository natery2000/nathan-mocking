using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NathanMocking.Test
{
  [TestClass]
  public class UseCaseTesting
  {
    [TestMethod]
    public void UseCaseTests()
    {
      #region Func<string>
      
      var classThatCanBeMocked = new ClassThatCanBeMocked();
      Assert.AreEqual("FunctionGetStringZeroParameters not mocked", classThatCanBeMocked.FunctionGetStringZeroParameters());

      var classThatWasMocked = MockBuilder.CreateMock<ClassThatCanBeMocked>();
      classThatWasMocked.FunctionGetStringZeroParametersFunc = (Func<string>)(() => "FunctionGetStringZeroParameters was mocked");
      Assert.AreEqual("FunctionGetStringZeroParameters was mocked", classThatWasMocked.FunctionGetStringZeroParameters());

      #endregion
    }
  }
}
