using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<Sprite> roundRecipe;

    public Sprite[] ingredients;

    private Dictionary<string, Sprite> ingredientsNameSprite;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeDictionary();

        int rnd = Random.Range(2, 4);
        for(int i = 0; i < rnd; ++i)
        {
            int rnd2 = Random.Range(0, ingredients.Length);
            roundRecipe.Add(ingredients[rnd2]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeDictionary()
    {
        ingredientsNameSprite = new Dictionary<string, Sprite>{
        {"cherry", ingredients[0] },
        {"orange", ingredients[1] },
        {"spring_onion", ingredients[2] },
        {"apple", ingredients[3] },
        {"green_pepper", ingredients[4] },
        {"peach", ingredients[5] },
        {"carrot", ingredients[6] },
        {"cucumber", ingredients[7] },
        {"strawberry", ingredients[8] },
        {"brocoli", ingredients[9] },
        {"pineaple", ingredients[10] },
        {"watermelon", ingredients[11] },
        {"radish", ingredients[12] },
        {"potato", ingredients[13] },
        {"banana", ingredients[14] },
        {"lettuce", ingredients[15] },
        {"kaki", ingredients[16] },
        {"chilli", ingredients[17] },
        {"corn", ingredients[18] },
        {"unknown_name_1", ingredients[19] },
        {"eggplant", ingredients[20] },
        {"unknown_name_2", ingredients[21] },
        {"red_pepper", ingredients[22] },
        {"bigshroom", ingredients[23] },
        {"onion", ingredients[24] },
        {"red_grapes", ingredients[25] },
        {"tomatoe", ingredients[26] },
        {"cabbage", ingredients[27] },
        {"mushroom", ingredients[28] },
        {"lemon", ingredients[29] },
        {"green_grapes", ingredients[30] },
        {"champignon", ingredients[31] },
        {"melon", ingredients[32] },
        {"pinecone", ingredients[33] },
        {"round_melon", ingredients[34] },
        {"celery", ingredients[35] },
        {"onigiri", ingredients[36] }};
    }
}
