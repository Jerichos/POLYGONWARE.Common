namespace POLYGONWARE.Common.Util
{
    public delegate void VoidDelegate();

    public delegate void GenericDelegate<in T>(T value);
    public delegate void BoolDelegate(bool value);
    public delegate void IntDelegate(int value);
}