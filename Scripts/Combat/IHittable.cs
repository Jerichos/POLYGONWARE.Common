namespace POLYGONWARE.Common.Combat
{
    public interface IHittable
    {
        bool Hit(DamageData damageData);
    }
}