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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.Enchant"); }
        enchantType = Type;
        strength = Strength;
        CalculateStrength();
    }

    public Enchant(Enchant enchant)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.Enchant"); }
        enchantType = enchant.enchantType;
        effectValue = enchant.effectValue;
        effectDuration = enchant.effectDuration;
        strength = 1;
    }

    public void Upgrade()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.Upgrade"); }
        strength++;
        CalculateStrength();
    }

    public void Downgrade()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.Downgrade"); }
        strength--;
        CalculateStrength();
    }

    public void CalculateStrength()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.CalculateStrength"); }
        switch (enchantType)
        {
            case EnchantType.Bleed:
                effectDuration = strength * 10f;
                effectValue = strength * 1f;
                maxStack = ((int)(strength / 3f)) + 2;
                break;
            case EnchantType.Fire:
                effectDuration = (strength * 2f) + 1;
                effectValue = strength * 3f;
                maxStack = ((int)(strength / 3f)) + 2;
                break;
            case EnchantType.Frost:
                effectDuration = strength * 5f;
                effectValue = (float)Math.Pow(0.5f, strength);
                maxStack = ((int)(strength / 3f)) + 2;
                break;
            case EnchantType.Knockback:
                effectValue = strength;
                break;
            case EnchantType.Poison:
                effectDuration = strength * 30f;
                effectValue = (float)Math.Pow(2, strength);
                maxStack = ((int)(strength / 3f)) + 2;
                uses = strength * 5;
                break;
            case EnchantType.Sharpness:
                effectValue = strength * 15f;
                break;
            case EnchantType.Stun:
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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.ApplyEffect"); }
        switch (enchantType)
        {
            case EnchantType.Bleed:
                enemy.Hurt(effectValue * strength * Irbis.Irbis.DeltaTime);
                break;
            case EnchantType.Fire:
                enemy.Hurt(effectValue * strength * Irbis.Irbis.DeltaTime);
                break;
            case EnchantType.Frost:
                enemy.SpeedModifier = effectValue;
                break;
            case EnchantType.Poison:
                enemy.Hurt(effectValue * strength * Irbis.Irbis.DeltaTime);
                break;
        }
        effectDuration -= Irbis.Irbis.DeltaTime;
        if (effectDuration <= 0)
        {
            switch (enchantType)
            {
                case EnchantType.Frost:
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
    public void AddEffect(IEnemy enemy)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.AddEffect"); }
        int enchantIndex;
        switch (enchantType)
        {
            case EnchantType.Bleed:
                enchantIndex = Contains(enemy.ActiveEffects, EnchantType.Bleed);
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
            case EnchantType.Fire:
                enchantIndex = Contains(enemy.ActiveEffects, EnchantType.Fire);
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
            case EnchantType.Frost:
                if (!(Contains(enemy.ActiveEffects, EnchantType.Poison) >= 0))
                {
                    enemy.AddEffect(CloneOf(this));
                }
                break;
            case EnchantType.Knockback:
                //enemy.Knockback(Irbis.Irbis.jamie.direction, effectValue);
                break;
            case EnchantType.Poison:
                if (uses > 0)
                {
                    enchantIndex = Contains(enemy.ActiveEffects, EnchantType.Poison);
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
            case EnchantType.Sharpness:
                enemy.Hurt(effectValue);
                break;
            case EnchantType.Stun:
                enemy.Stun(effectDuration);
                break;
        }
    }

    public Enchant CloneOf(Enchant enchant)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.CloneOf"); }
        return new Enchant(enchant);
    }

    /// <summary>
    /// Returns the index of the first EnchantType Type in EnchantList
    /// Returns -1 if none
    /// </summary>
    public int Contains(List<Enchant> EnchantList, EnchantType Type)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enchant.Contains"); }
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

