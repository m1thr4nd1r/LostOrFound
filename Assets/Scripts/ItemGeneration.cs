using System.Linq;
using UnityEngine;

public class ItemGeneration : MonoBehaviour
{
    public int _maxObjects = 50;
    public GameObject _prefab;
    
    readonly int[] _filteredSpriteIds = new int[] {
                                    15, 25, 26, 27, 28, 29, 30, 47, 55, 56,
                                    57, 58, 59, 60, 61, 62, 63, 87, 89, 89
                                 };
    Object[] _sprites;

    public Sprite GetSprite() => (Sprite)_sprites[_filteredSpriteIds[Random.Range(0, _filteredSpriteIds.Length)]];

    void Start()
    {
        var minBounds = (Vector2)gameObject.transform.position - (Vector2)gameObject.transform.localScale / 2;
        var maxBounds = (Vector2)gameObject.transform.position + (Vector2)gameObject.transform.localScale / 2;
        var range = maxBounds - minBounds;
        
        if (Debug.isDebugBuild)
            Debug.Log("MinBounds:" + minBounds + " MaxBounds: " + maxBounds + " Range: " + range);

        _sprites = Resources.LoadAll<Sprite>("0x72_16x16DungeonTileset.v3");

        for (int i = 0; i < _maxObjects; i++)
        {
            var random = new Vector2(Random.value, Random.value);
            var position = random * range + minBounds;

            var obj = Instantiate(_prefab, position, Quaternion.identity);
            obj.transform.SetParent(gameObject.transform);

            var renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = GetSprite();
            if (Debug.isDebugBuild) 
                Debug.Log("Random: " + random + " Position: " + position + "\n" +
                          "Sprite: " + renderer.sprite + "Index: " + i);
        }
    }

    internal bool IsSpriteOnArea(Sprite chosenSprite)
    {
        return GetComponentsInChildren<SpriteRenderer>().AsEnumerable()
               .Any(render => render.sprite.name.Equals(chosenSprite.name));
    }
}
