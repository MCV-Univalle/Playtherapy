using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PutDataResults : MonoBehaviour {

	bool mostrar_propuesta1 =false;



	const int EXCELENT = 0;
	const int GOOD = 1;
	const int DEFICIENT = 2;
	Text txt_score_results;
	Text txt_best_score_results;



	Sprite[] sprites;
	string[] names_sprites;
	//para propuesta1
	string [] names_bands ;
	string [] names_medals;
    string minigame;
	Image band_medal;
	Image type_medal;


	//para propuesta2
	Image star1;
	Image star2;
	Image star3;
	string [] names_stars;

	// Use this for initialization
	void Start () {
        Debug.Log("Soy el Start del PutData");
		sprites = Resources.LoadAll<Sprite> ("Sprites/medals_flat") ;
		names_sprites = new string[sprites.Length];

		for(int i = 0; i < names_sprites.Length; i++) 
		{
			names_sprites[i] = sprites[i].name;
		}


		GameObject propuesta1 = GameObject.Find ("propuesta1");
		GameObject propuesta2 = GameObject.Find ("propuesta2");

        if (mostrar_propuesta1==true)
		{
			propuesta1.SetActive (true);
			propuesta2.SetActive (false);
			searchObjects1 ();
		} else {

			propuesta2.SetActive (true);

            propuesta1.SetActive (false);
			searchObjects2 ();
		}




	}
	void searchObjects1()
	{
		txt_score_results = GameObject.Find ("txt_score_results").GetComponent<Text>();

		txt_best_score_results= GameObject.Find ("txt_best_score_results").GetComponent<Text>();

		band_medal = GameObject.Find ("ImageBand").GetComponent<Image> ();
		type_medal = GameObject.Find ("ImageMedal").GetComponent<Image> ();





		names_bands = new string[2];
		names_bands[0]= "band1";
		names_bands[1]= "band2";


		names_medals = new string[3];
		names_medals[0]= "medal_gold";
		names_medals[1]= "medal_silver";
		names_medals[2]= "medal_bronce";



	}
	void searchObjects2()
	{
        Debug.Log("Llamado a la funcion searchObjects");
		txt_score_results = GameObject.Find("txt_score_results2").GetComponent<Text>();
        if (txt_score_results == null)
        {
            Debug.Log("No se encontro el objeto");
        }
        Debug.Log("Encontre los datos");
        txt_best_score_results = GameObject.Find("txt_best_score_results2").GetComponent<Text>();

		star1 = GameObject.Find("Star1").GetComponent<Image> ();
		star2 = GameObject.Find("Star2").GetComponent<Image> ();
		star3 = GameObject.Find("Star3").GetComponent<Image> ();


		names_stars = new string[2];
		names_stars[0]= "star_off";
		names_stars[1]= "star_on";

	}
	public Sprite getSpriteFromName(string name="")
	{
		int index = System.Array.IndexOf (names_sprites, name);

		return sprites [index];

	}
	/// <summary>
	/// Updates the data results
	/// </summary>
	/// <param name="percent">Percent. is the performance from 0 % to 100 % </param>
	/// <param name="best_percent">Best percent. is the BD loaded performance from 0 % to 100 % </param>
	public void updateData(int percent=0,int best_percent=0)
	{

        if (mostrar_propuesta1 == true)
        {
            searchObjects1();
        }
        else
        {
            searchObjects2();
        }
        Debug.Log("He entrado al Script con un performance de " + percent);
		int how_do_it;
		if (percent <= 25) {
			how_do_it = DEFICIENT;
		} else if (percent > 25 && percent <= 75) {
			how_do_it = GOOD;
		} else {
			how_do_it = EXCELENT;
		}
        Debug.Log("Lo hizo " + how_do_it);


        GameSessionDAO gameDao = new GameSessionDAO();
        if(gameDao != null)
        {
            Debug.Log("Se ha creado el DAO");
        }

        txt_score_results.text = "Desempeño: " + percent+"%";
        Debug.Log("El campo de texto de result se ha modificado con " + txt_score_results);
		txt_best_score_results.text = "Mejor Desempeño: " + gameDao.GetScore(this.Minigame)+"%";

        Debug.Log("Se han configurado los campos");


		if (mostrar_propuesta1 == true) 
		{
		
			if (how_do_it == EXCELENT) {
				type_medal.sprite = getSpriteFromName (names_bands [0]);
			} else {
				type_medal.sprite = getSpriteFromName (names_bands [1]);
			}
				
			type_medal.sprite = getSpriteFromName (names_medals [how_do_it]);
		}
		else
		{

			switch (how_do_it) {
			case DEFICIENT:
				star1.sprite = getSpriteFromName (names_stars [1]);
				star2.sprite = getSpriteFromName (names_stars [0]);
				star3.sprite = getSpriteFromName (names_stars [0]);
				break;
			case GOOD:
				star1.sprite = getSpriteFromName (names_stars [1]);
				star2.sprite = getSpriteFromName (names_stars [1]);
				star3.sprite = getSpriteFromName (names_stars [0]);
				break;
			case EXCELENT:
				star1.sprite = getSpriteFromName (names_stars [1]);
				star2.sprite = getSpriteFromName (names_stars [1]);
				star3.sprite = getSpriteFromName (names_stars [1]);
				break;

			default:
				star1.sprite = getSpriteFromName (names_stars [1]);
				star2.sprite = getSpriteFromName (names_stars [0]);
				star3.sprite = getSpriteFromName (names_stars [0]);
				break;
			}

		}

	}
	// Update is called once per frame
	void Update () {
		
	}

    public string Minigame
    {
        get
        {
            return minigame;
        }

        set
        {
            minigame = value;
        }
    }
}
