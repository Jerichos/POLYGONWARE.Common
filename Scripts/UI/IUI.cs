namespace POLYGONWARE.Common.UI
{
    public interface IUI
    {
        IUI Open();
        IUI Open<T>(T arg);
        IUI Close();
        IUI Toggle();
    }
}