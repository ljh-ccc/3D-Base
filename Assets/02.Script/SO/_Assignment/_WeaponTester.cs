using UnityEngine;

public class _WeaponTester
{
    public _PlayerData playerData;

    public _WeaponData weaponData;
    public int currentAmmo;
    public bool canUse;

    public _WeaponTester(_PlayerData playerData, _WeaponData weaponData)
    {
        this.playerData = playerData;
        this.weaponData = weaponData;

        if (weaponData.useAmmo) { currentAmmo = weaponData.magazineSize; }
        else { currentAmmo = 0; }

        if (ArrayContains(playerData.jobType)) { canUse = true; }
        else 
        { 
            canUse = false;

            Debug.Log($"[_WeaponTester] Can't use this weapon. \n" +
                $"Player Job : <color=red>{playerData.jobType}</color> " +
                $"Weapon Name : <color=blue>{weaponData.itemName}</color> " +
                $"Can Use Job : <color=white>{ArrayRound()}</color> "); 
        }
    }

    bool ArrayContains(_JobType job)
    {
        foreach(_JobType j in weaponData.canUseJob)
        {
            if(j == job) { return true; }
        }
        return false;
    }

    string ArrayRound()
    {
        string msg = "";

        foreach (_JobType j in weaponData.canUseJob)
        {
            msg += $"[{j}] ";
        }
        return msg;
    }

    public bool HasAmmo()
    {
        if (!weaponData.useAmmo) { return true; }

        return currentAmmo > 0;
    }

    public void ConsumeAmmo()
    {
        if (!weaponData.useAmmo) { return; }

        currentAmmo--;
    }

    public void Reload()
    {
        if (!weaponData.useAmmo) { return; }

        currentAmmo = weaponData.magazineSize;
    }
}
