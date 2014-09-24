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
	void Awake () 
	{
		if (text == null)
		{
			text = GameObject.FindGameObjectWithTag("ConversationTitle").GetComponent<Text>();
		}
		if (player == null && text != null)
		{
			Transform maybePlayer = transform;
			while (maybePlayer.tag != "Converser" && maybePlayer != transform.root)
			{
				maybePlayer = maybePlayer.parent;
			}
			if (maybePlayer.tag == "Converser")
			{
				player = maybePlayer.gameObject;
			}
		}

		if (text != null)
		{
			text.color = new Color(1f, 0f, 1f, 0);
			text.text = "";
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(player != null)
		{
			Conversation conversation = ConversationManger.Instance.FindConversation(transform.parent.GetComponent<PartnerLink>(), player.GetComponent<PartnerLink>());
			if (conversation != null)
			{
				var distance = Vector3.Distance(player.transform.position, transform.position);

				if (text != null)
				{
					if (!convoStart)
					{
						alpha = Mathf.Clamp(1 - (distance / (conversation.breakingDistance)), 0, 1);
						if (distance <= (conversation.breakingDistance))
						{
							text.color = new Color(1f, 0f, 1f, alpha);

						}
						if (distance < conversation.initiateDistance)
						{
							convoStart = true;
						}
					}

					if (alpha > 0 && convoStart)
					{
						alpha = Mathf.Max(alpha - Time.deltaTime, 0);
						text.color = new Color(1f, 0.92f, 0.016f, alpha);
					}
				}
			}
		}
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Converser" && other.gameObject != transform.parent.gameObject)
		{
			player = other.gameObject;

			if (text != null)
			{
				Conversation conversation = ConversationManger.Instance.FindConversation(transform.parent.GetComponent<PartnerLink>(), player.GetComponent<PartnerLink>());
				if (conversation != null)
				{
					text.text = conversation.title;
				}
				else
				{
					text.text = "";
				}
			}
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
