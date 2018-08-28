using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class PooledObjectDefinition{
	public GameObject prefab;
	public int initialCount;
	public bool isFixedSize = false;
}
public class Pool{
	private PooledObjectDefinition m_definition;
	private HashSet<GameObject> m_pool;
	public Pool(PooledObjectDefinition definition){
		m_pool = new HashSet<GameObject>();
		m_definition = definition;
		for(int i = 0; i < m_definition.initialCount; i++){
			InstantiateObject();
		}
	}
	GameObject InstantiateObject(){
		GameObject newObject = MonoBehaviour.Instantiate(m_definition.prefab);
		newObject.gameObject.SetActive(false);
		m_pool.Add(newObject);
		return newObject;
	}
	public GameObject GetObject(){
		GameObject first = m_pool.FirstOrDefault(x => !x.activeInHierarchy);
		if(first == null && !m_definition.isFixedSize){
			first = InstantiateObject();
		}
		return first;
	}
}
public class UberPool : MonoBehaviour {
	[SerializeField]
	private PooledObjectDefinition [] m_definitions;
	private Dictionary<int, Pool> m_pools;
	private static UberPool m_instance;
	public static UberPool Instance{
		get{
			return m_instance;
		}
	}
	private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        } else {
            m_instance = this;
        }
		m_pools = new Dictionary<int, Pool>();
		foreach(PooledObjectDefinition definition in m_definitions){
			m_pools.Add(definition.prefab.GetInstanceID(), new Pool(definition));
		}
    }
	public GameObject GetObject(GameObject prefab){
		Pool pool;
		GameObject retObj = null;
		if(m_pools.TryGetValue(prefab.GetInstanceID(), out pool)){
			retObj = pool.GetObject();
		}
		return retObj;
	}
}
