namespace POLYGONWARE.Common.Util
{
    public delegate void VoidDelegate();
    public delegate void GenericDelegate<in T>(T value);
    public delegate void Generic2Delegate<T>(T value1, T value2);
}