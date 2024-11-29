using UnityEngine;

public class EnemyCollectableDrop : MonoBehaviour
{
  [SerializeField]
  private float _chanceOfCollectableDrop;

  private CollectableSpawner _collectableSpawner;

  private void Awake(){
    _collectableSpawner = Object.FindFirstObjectByType<CollectableSpawner>();
  }

  public void RandomDropCollectable(){
        float random = Random.Range(0f,1f);
        
        if(_chanceOfCollectableDrop >= random){
            _collectableSpawner.SpawnCollectable(transform.position);
        }
  }
}
