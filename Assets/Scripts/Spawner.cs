using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SpawnDefinition
{
    public GameObject spawnPrefab;
    public uint spawnWeight;
}
public class Spawner : MonoBehaviour {
	public Vector3 m_spawnVolume;
	public float m_spawnRate;
	public float m_spawnRateGrowth;
	public uint m_mobSize;
	public uint m_mobSizeRange;
	public SpawnDefinition[] m_spawnDefinition;
	private uint m_totalSpawnWeight = 0;
	private float m_spawnTime;
	// Use this for initialization
	void Start () {
		UpdateSpawnTime();
		InvokeRepeating("UpdateSpawnRate", 0, m_spawnTime);
		foreach(SpawnDefinition def in m_spawnDefinition){
			m_totalSpawnWeight += def.spawnWeight;
		}
		StartCoroutine(StartSpawn());
	}
	IEnumerator StartSpawn(){
		while(true){
			SpawnObjects();
			yield return new WaitForSeconds(m_spawnTime);
		}
	}
	void SpawnObjects(){
		uint lowerBound = (uint)Mathf.Max(1, m_mobSize - m_mobSizeRange);
		uint spawnCount = (uint)UnityEngine.Random.Range((int)lowerBound, (int)(m_mobSize + m_mobSizeRange + 1));
		for(int i = 0; i < spawnCount; i++){
			SpawnOneObject();
		}
	}
	void SpawnOneObject(){
		int spawnNumber = UnityEngine.Random.Range(0, (int)m_totalSpawnWeight);
		GameObject selectedPrefab = null;
		foreach(SpawnDefinition def in m_spawnDefinition){
			spawnNumber -= (int)def.spawnWeight;
			if(spawnNumber < 0){
				selectedPrefab = def.spawnPrefab;
				break;
			}
		}
		if(selectedPrefab != null){
            Vector3 spawnPosition = new Vector3(
                UnityEngine.Random.Range(transform.position.x - m_spawnVolume.x, transform.position.x + m_spawnVolume.x) * 0.5f,
                UnityEngine.Random.Range(transform.position.y - m_spawnVolume.y, transform.position.y + m_spawnVolume.y) * 0.5f, 
                UnityEngine.Random.Range(transform.position.z - m_spawnVolume.z, transform.position.z + m_spawnVolume.z) * 0.5f
            );
            Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
		}
	}
	void UpdateSpawnRate(){
		m_spawnRate *= m_spawnRateGrowth;
		UpdateSpawnTime();
	}
	void UpdateSpawnTime(){
		m_spawnTime = 1/m_spawnRate;
	}
	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0, 1, 0, 1);
		Gizmos.DrawWireCube(transform.position, m_spawnVolume);
	}
}
