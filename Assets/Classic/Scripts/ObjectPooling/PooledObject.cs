using UnityEngine;

public class PooledObject : MonoBehaviour {

	[System.NonSerialized]
	ObjectPool poolInstanceForPrefab;

	public T GetPooledInstance<T> () where T : PooledObject {
		if (!poolInstanceForPrefab) {
			poolInstanceForPrefab = ObjectPool.GetPool(this);
		}
		return (T)poolInstanceForPrefab.GetObject();
	}

	public ObjectPool Pool { get; set; }

	public void ReturnToPool () {
		if (Pool) {
			Pool.AddObject(this);
			this.transform.position = Vector3.zero;
			this.transform.SetParent(Pool.transform);
		}
		else {
			Debug.Log("I die!");
			Destroy(gameObject);
		}
	}
}