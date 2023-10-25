using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Tkfkadlsi
{
    [System.Serializable]
    public class PoolingObject
    {
        public GameObject objectPrefab;
        public string objectName;
        public int objectAmount;
    }

    public class PoolManager : MonoBehaviour
    {
        public List<PoolingObject> poolingObjects = new List<PoolingObject>();
        public Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

        private void Awake()
        {
            foreach (PoolingObject @object in poolingObjects)
            {
                pools.Add(@object.objectName, new Queue<GameObject>());

                CreateObjectInPool(@object.objectName, @object.objectAmount, @object.objectPrefab);
            }
        }

        private void CreateObjectInPool(string objectName, int count, GameObject objectPrefab)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject newObj = Instantiate(objectPrefab);
                newObj.transform.SetParent(transform);
                newObj.SetActive(false);
                pools[objectName].Enqueue(newObj);
            }
        }

        private void PlusObjectInPool(string objectName)
        {
            foreach (PoolingObject @object in poolingObjects)
            {
                if (@object.objectName != objectName) return;

                CreateObjectInPool(objectName, 1, @object.objectPrefab);
            }
        }
        private bool CheckInPool(string objectName)
        {
            if (pools[objectName].Count <= 0)
            {
                PlusObjectInPool(objectName);
                return false;
            }
            else
            {
                return true;
            }
        }

        public GameObject GetObject(string objectName)
        {
            if (!CheckInPool(objectName)) return null;

            GameObject outObj = pools[objectName].Dequeue();
            outObj.transform.SetParent(null);
            outObj.SetActive(true);
            return outObj;
        }
        public GameObject GetObject(string objectName, Vector3 position)
        {
            if (!CheckInPool(objectName)) return null;

            GameObject outObj = pools[objectName].Dequeue();
            outObj.transform.position = position;
            outObj.transform.SetParent(null);
            outObj.SetActive(true);
            return outObj;
        }
        public GameObject GetObject(string objectName, Vector3 position, Transform parent)
        {
            if (!CheckInPool(objectName)) return null;

            GameObject outObj = pools[objectName].Dequeue();
            outObj.transform.position = position;
            outObj.transform.SetParent(parent);
            outObj.SetActive(true);
            return outObj;
        }

        public void ReturnObject(string objectName, GameObject inObj)
        {
            inObj.SetActive(false);
            inObj.transform.SetParent(transform);
            inObj.transform.position = Vector3.zero;

            pools[objectName].Enqueue(inObj);
        }
    }
}