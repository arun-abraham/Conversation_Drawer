using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFading : MonoBehaviour {
	
	public Text text;
	private float alpha = 0;
	public PartnerLink partnerLink;
	public PartnerLink player;
	private Conversation conversation;
	private GameObject converser;
	private bool convoStart = false;

	// Use this for initialization
	void Awake () 
	{
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
		if (text == null)
		{
			text = GameObject.FindGameObjectWithTag("ConversationTitle").GetComponent<Text>();
		}
		if (player == null && text != null)
		{
			Transform maybePlayer = text.transform;
			while (maybePlayer.tag != "Converser" && maybePlayer != transform.root)
			{
				maybePlayer = maybePlayer.parent;
			}
			if (maybePlayer.tag == "Converser")
			{
				player = maybePlayer.GetComponent<PartnerLink>();
			}
		}

		if (text != null)
		{
			text.color = new Color(1f, 0f, 1f, 0);
			text.text = "";
		}
	}

	void Start()
	{
		conversation = ConversationManger.Instance.FindConversation(partnerLink, player);
	}
	
	// Update is called once per frame
	void Update () {

		if(player != null)
		{
<<<<<<< HEAD
			
=======
>>>>>>> b18246a33356c4de08256a233f49aa5b0444e615
			if (conversation != null)
			{
				var distance = Vector3.Distance(player.transform.position, transform.position);

				if (text != null)
				{
					if (!convoStart)
					{
						alpha = Mathf.Clamp(1 - (distance / (conversation.breakingDistance)), 0, 1);
						if (distance <= conversation.initiateDistance)
						{
							convoStart = true;
							text.text = conversation.title;
						}
						else if (distance <= (conversation.breakingDistance))
						{
							text.color = new Color(1f, 0f, 1f, alpha);
							text.text = conversation.title;
						}
						
						else
						{
							text.text = "";
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
<<<<<<< HEAD
=======

	void UnlinkPartner()
	{
		convoStart = false;
	}
>>>>>>> b18246a33356c4de08256a233f49aa5b0444e615
}
