using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Sprite[] ingredients;

    public Dictionary<Sprite, int> roundRecipe;

    public GameObject ingredientFrame;

    public Transform recipePanel;

    Dictionary<string, Sprite> m_ingredientsNameSprite;

    List<GameObject> recipeUIList;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDictionary();

        roundRecipe = new Dictionary<Sprite, int>();
        recipeUIList = new List<GameObject>();

        int rnd = Random.Range(2, 4);
        int i = 0;
        while(i < rnd)
        {
            int rndSprite = Random.Range(0, ingredients.Length - 1);
            int rndQuantity = Random.Range(1, 3);
            roundRecipe.Add(ingredients[rndSprite], rndQuantity);
            GameObject f = Instantiate(ingredientFrame, recipePanel);
            if(!recipeUIList.Contains(f))
            {
                f.GetComponentsInChildren<Image>()[1].sprite = ingredients[rndSprite];
                recipeUIList.Add(f);
                i++;
            }
            else
            {
                Destroy(f);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(GameObject go in recipeUIList)
        {
            go.GetComponentInChildren<TextMeshProUGUI>().text = ("x" + roundRecipe[go.GetComponentsInChildren<Image>()[1].sprite]);
        }
    }

    void InitializeDictionary()
    {
        m_ingredientsNameSprite = new Dictionary<string, Sprite>{
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
