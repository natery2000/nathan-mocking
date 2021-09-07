namespace NathanMocking.Test
{
  public class ClassThatCanBeMocked
  {
    public virtual string FuncString()
    {
      return "FuncString not mocked";
    }

    public virtual string FuncStringString(string param)
    {
      return $"FuncStringString not mocked with {param}";
    }

    public virtual string FuncIntString(int param)
    {
      return $"FuncIntString not mocked with {param}";
    }

    public virtual int FuncStringInt(string param)
    {
      if (param == string.Empty) return -1;
      return 1;
    }
  }
}
