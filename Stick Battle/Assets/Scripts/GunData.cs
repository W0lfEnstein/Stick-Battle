using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Gun Data")]
public class GunData : ScriptableObject
{
    #region Gun Info

    public string GunId;
    public string GunName;
    public GunType gunType;

    #endregion

    #region Gun Settings

    public float damage;
    public float distance;

    public float shotInterval;
    public float reloadTime;

    public int magCapacity;
    public int maxAmmo;

    #endregion

    public AudioClip[] Sounds;

    public enum GunType
    {
        Pistol,
        Revolver,
        Rifle,
        Shotgun,
        SMG,
        AssaultRifle,
        SniperRifle,
        MachineGun
    }
}
