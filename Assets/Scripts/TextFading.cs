using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFading : MonoBehaviour {
	
	public Text text;
	
	
	private float alpha = 0;
	
	public GameObject player;

	private GameObject converser;

	private bool convoStart = false;

	// Use this for initialization
	void Start () {
		
		text.color = new Color(1f, 0f, 1f, 0);
	}
	
	// Update is called once per frame
	void Update () {

		if(player != null)
		{
			var distance = Vector3.Distance(player.transform.position, transform.position) - 5f;

			if(!convoStart)
			{
				alpha = 1 - (distance/20);

				if(distance <=20)
				{
					text.color = new Color(1f, 0f, 1f, alpha);
					
				}
				if(distance < 2f)
				{
					convoStart = true;
				}
			}
			
			if(alpha > 0 && convoStart)
			{
				alpha -= .01f;
				text.color = new Color(1f, 0.92f, 0.016f, alpha);
			}
		}
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Converser" && other.gameObject != transform.parent.gameObject)
		{
			player = other.gameObject;

			Conversation conversation = ConversationManger.Instance.FindConversation(transform.parent.GetComponent<PartnerLink>(), player.GetComponent<PartnerLink>());
			text.text = conversation.title;
		}		
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Converser" && other.gameObject != transform.parent.gameObject)
		{
			player = null;
			convoStart = false;
		}

	}

}
