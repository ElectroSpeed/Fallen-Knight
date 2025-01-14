using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [Serializable]
    public struct Enemies
    {
        public Mob Zombie;
        [Space(1)]
        public Mob Berseker;
        [Space(1)]
        public Mob Ghost;
    }

    [SerializeField] private Enemies _typeEnemyToSpawn;

    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private float _timeBetweenMobSpawns;
    [SerializeField] private float _timeBetweenWaves;
    
    [SerializeField] private MenuEventsManager _menuManager;
    [SerializeField] private GameObject _winPanel;

    [SerializeField] private int _totalWavesCount;
    [SerializeField] private TMP_Text _WavesCount;

    private Dictionary<Mob, int> _mobsToSpawn = new Dictionary<Mob, int>();

    private int _waveCount = 1;

    private void Start()
    {
        InitializeEnemyDictionary();
        StartCoroutine(WaveRoutine());
    }

    private void InitializeEnemyDictionary()
    {
        _mobsToSpawn[_typeEnemyToSpawn.Berseker] = 0;
        _mobsToSpawn[_typeEnemyToSpawn.Ghost] = 0;
        _mobsToSpawn[_typeEnemyToSpawn.Zombie] = 0;
    }

    private IEnumerator WaveRoutine()
    {
        while (_waveCount <= _totalWavesCount)
        {
            CalculateWaveSpawns();
            _WavesCount.text = "Wave: " + _waveCount.ToString();
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(_timeBetweenWaves);
            _waveCount++;
        }

        _menuManager.PauseScene();
        _winPanel.SetActive(true);
    }

    private void CalculateWaveSpawns()
    {
        var keys = new List<Mob>(_mobsToSpawn.Keys);
        
        foreach (var key in keys)
        {
            _mobsToSpawn[key] = 0;
        }

        // Ghosts
        if (_waveCount >= 10)
        {
            if (_waveCount <= 18)
            {
                _mobsToSpawn[_typeEnemyToSpawn.Ghost] = 2 + ((_waveCount - 10) * 1);
            }
            else
            {
                _mobsToSpawn[_typeEnemyToSpawn.Ghost] = 2 + (8 * 1) + ((_waveCount - 18) * 2);
            }
        }

        // Bersekers
        if (_waveCount >= 20)
        {
            if (_waveCount <= 28)
            {
                _mobsToSpawn[_typeEnemyToSpawn.Berseker] = 2 + ((_waveCount - 20) * 1);
            }
            else
            {
                _mobsToSpawn[_typeEnemyToSpawn.Berseker] = 2 + (8 * 1) + ((_waveCount - 28) * 2);
            }
        }

        // Zombies
        if (_waveCount >= 1)
        {
            if (_waveCount <= 10)
            {
                _mobsToSpawn[_typeEnemyToSpawn.Zombie] = 2 + ((_waveCount - 1) * 1);
            }
            else
            {
                _mobsToSpawn[_typeEnemyToSpawn.Zombie] = 2 + (9 * 1) + ((_waveCount - 10) * 2);
            }
        }
    }


    private IEnumerator SpawnWave()
    {
        foreach (var mobEntry in _mobsToSpawn)
        {
            for (int i = 0; i < mobEntry.Value; i++)
            {
                SpawnMob(mobEntry.Key);
                yield return new WaitForSeconds(_timeBetweenMobSpawns);
            }
        }

        yield return new WaitUntil(() => AllMobsAreDead());
    }

    private void SpawnMob(Mob mobToSpawn)
    {
        var mob = Instantiate(mobToSpawn, _spawnPoint.position, _spawnPoint.rotation);
        mob._maxHealth += _waveCount * 10;
        mob._health += _waveCount * 10;
    }

    private bool AllMobsAreDead()
    {
        return FindObjectsOfType<Mob>().Length == 0;
    }
}