using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public enum EffectType
    {
        None,
        Poison,
        Freeze,
        Burn,
        ChainLightning
    }

    public EffectType _type;
    public float _duration;
    public int _maxChainTargets;
    
    public Effect(EffectType type, float duration = 0f, float damageModifier = 0f, float speedModifier = 0f, int maxChainTargets = 0)
    {
        _type = type;
        _duration = duration;
        _maxChainTargets = maxChainTargets;
    }
    
    public void ApplyEffect(GameObject target)
    {
        switch (_type)
        {
            case EffectType.Poison:
                ApplyPoison(target);
                break;
            case EffectType.Freeze:
                ApplyFreeze(target);
                break;
            case EffectType.Burn:
                ApplyBurn(target);
                break;
            case EffectType.ChainLightning:
                ApplyChainLightning(target);
                break;
            default:
                Debug.Log("None Effect");
                break;
        }
    }
    
    private void ApplyPoison(GameObject target)
    {
        Debug.Log($"Poison applied to {target.name} for {_duration} seconds.");
        target.GetComponent<Mob>().ReduceSpeed(30, _duration);
        target.GetComponent<Mob>().StartCoroutine(InflictPoisonDamage(target, 10, 1f, _duration));
    }
    
    private IEnumerator InflictPoisonDamage(GameObject target, int damagePerTick, float tickInterval, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            target.GetComponent<Mob>().RemoveHealth(damagePerTick);
            elapsedTime += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private void ApplyFreeze(GameObject target)
    {
        Debug.Log($"Freeze applied to {target.name} for {_duration} seconds.");
        target.GetComponent<Mob>().ReduceSpeed(100, _duration);
    }

    private void ApplyBurn(GameObject target)
    {
        Debug.Log($"Burn applied to {target.name} for {_duration} seconds.");
        target.GetComponent<Mob>().StartCoroutine(InflictBurnDamage(target, 5, 0.2f, _duration));
    }
    
    private IEnumerator InflictBurnDamage(GameObject target, int damagePerTick, float tickInterval, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            target.GetComponent<Mob>().RemoveHealth(damagePerTick);
            elapsedTime += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private void ApplyChainLightning(GameObject target)
    {
        Debug.Log($"Chain Lightning applied to {target.name} with max targets: {_maxChainTargets}.");
        // Implémentation de la décharge en chaîne
    }
}
