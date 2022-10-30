namespace POLYGONWARE.Common.Util
{
    public delegate void VoidDelegate();
    public delegate void GenericDelegate<in T>(T value);

    public delegate void GenericDelegate<in T1, in T2>(T1 value1, T2 value2);
    public delegate void Generic2Delegate<in T>(T value1, T value2);
}