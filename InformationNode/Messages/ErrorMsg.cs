﻿using Newtonsoft.Json;

namespace InformationNode.Messages
{
	class ErrorMsg:Message
	{
		public override string GetResponse()
		{
			return JsonConvert.SerializeObject(this);
		}

		public ErrorMsg(Message msg) 
		{
		}

		public ErrorMsg():base()
		{
			
		}
	}
}
