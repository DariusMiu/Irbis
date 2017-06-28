using System;
using Irbis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Enchant
{
    public EnchantType enchantType;
    public int strength;
    public int maxStack;
    private int uses;
    public float effectValue;
    public float effectDuration;

    public Enchant(EnchantType Type, int Strength)
    {
        enchantType = Type;
        strength = Strength;
        CalculateStrength();
    }

    public Enchant(Enchant enchant)
    {
        enchantType = enchant.enchantType;
        effectValue = enchant.effectValue;
        effectDuration = enchant.effectDuration;
        strength = 1;
    }

    public void Upgrade()
    {
        strength++;
        CalculateStrength();
    }

    public void Downgrade()
    {
        strength--;
        CalculateStrength();
    }

    public void CalculateStrength()
    {
        switch (enchantType)
        {
            case EnchantType.bleed:
                effectDuration = strength * 10f;
                effectValue = strength * 1f;
                maxStack = ((int)(strength / 3f)) + 2;
                break;
            case EnchantType.fire:
                effectDuration = (strength * 2f) + 1;
                effectValue = strength * 3f;
                maxStack = ((int)(strength / 3f)) + 2;
                break;
            case EnchantType.frost:
                effectDuration = strength * 5f;
                effectValue = 0.5f;
                maxStack = ((int)(strength / 3f)) + 2;
                break;
            case EnchantType.knockback:
                effectValue = strength;
                break;
            case EnchantType.poison:
                effectDuration = strength * 30f;
                effectValue = strength * 3f;
                maxStack = ((int)(strength / 3f)) + 2;
                uses = strength * 5;
                break;
            case EnchantType.sharpness:
                effectValue = strength * 15f;
                break;
            case EnchantType.stun:
                effectDuration = strength * 1.5f;
                break;
        }
    }

    /// <summary>
    /// ApplyEffect deals damage or slows depending on the effect type
    /// (use this to actually render the Enchant's effect from the activeEffects list)
    /// Returns whether or not the effect should be removed from activeEffects
    /// </summary>
    public bool ApplyEffect(IEnemy enemy)
    {
        switch (enchantType)
        {
            case EnchantType.bleed:
                enemy.Hurt(effectValue * strength * Irbis.Irbis.DeltaTime);
                break;
            case EnchantType.fire:
                enemy.Hurt(effectValue * strength * Irbis.Irbis.DeltaTime);
                break;
            case EnchantType.frost:
                enemy.SpeedModifier = effectValue;
                break;
            case EnchantType.poison:
                enemy.Hurt(effectValue * strength * Irbis.Irbis.DeltaTime);
                break;
        }
        effectDuration -= Irbis.Irbis.DeltaTime;
        if (effectDuration <= 0)
        {
            switch (enchantType)
            {
                case EnchantType.frost:
                    enemy.SpeedModifier = 1f;
                    break;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// AddEffect adds the effect to the enemy's activeEffects list or just applies the effect if it is on hit
    /// </summary>
    public void AddEffect(Player player, IEnemy enemy)
    {
        int enchantIndex;
        switch (enchantType)
        {
            case EnchantType.bleed:
                enchantIndex = Contains(enemy.ActiveEffects, EnchantType.bleed);
                if (enchantIndex >= 0)
                {
                    if (enemy.ActiveEffects[enchantIndex].strength < maxStack)
                    {
                        enemy.UpgradeEffect(enchantIndex, effectDuration);
                    }
                }
                else
                {
                    enemy.AddEffect(CloneOf(this));
                }
                break;
            case EnchantType.fire:
                enchantIndex = Contains(enemy.ActiveEffects, EnchantType.fire);
                if (enchantIndex >= 0)
                {
                    if (enemy.ActiveEffects[enchantIndex].strength < maxStack)
                    {
                        enemy.UpgradeEffect(enchantIndex, effectDuration);
                    }
                }
                else
                {
                    enemy.AddEffect(CloneOf(this));
                }
                break;
            case EnchantType.frost:
                if (!(Contains(enemy.ActiveEffects, EnchantType.poison) >= 0))
                {
                    enemy.AddEffect(CloneOf(this));
                }
                break;
            case EnchantType.knockback:
                enemy.Knockback(player, effectValue);
                break;
            case EnchantType.poison:
                if (uses > 0)
                {
                    enchantIndex = Contains(enemy.ActiveEffects, EnchantType.poison);
                    if (enchantIndex >= 0)
                    {
                        if (enemy.ActiveEffects[enchantIndex].strength < maxStack)
                        {
                            enemy.UpgradeEffect(enchantIndex, effectDuration);
                        }
                    }
                    else
                    {
                        enemy.AddEffect(CloneOf(this));
                    }
                    uses--;
                }
                break;
            case EnchantType.sharpness:
                enemy.Hurt(effectValue);
                break;
            case EnchantType.stun:
                enemy.Stun(effectDuration);
                break;
        }
    }

    public Enchant CloneOf(Enchant enchant)
    {
        return new Enchant(enchant);
    }

    /// <summary>
    /// Returns the index of the first EnchantType Type in EnchantList
    /// Returns -1 if none
    /// </summary>
    public int Contains(List<Enchant> EnchantList, EnchantType Type)
    {
        for (int i = 0; i < EnchantList.Count; i++)
        {
            if (EnchantList[i].enchantType == Type)
            {
                return i;
            }
        }
        return -1;
    }
}

