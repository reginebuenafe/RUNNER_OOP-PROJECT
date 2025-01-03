using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _collectablePrefabs;

    public void SpawnCollectable(Vector2 position){
        int index = Random.Range(0, _collectablePrefabs.Count);
        var SelectedCollectable = _collectablePrefabs[index];

        Instantiate(SelectedCollectable, position, Quaternion.identity);
    }
}
