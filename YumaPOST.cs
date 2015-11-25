using System;
using System.Collections.Generic;

namespace YumaWebLib.Http
{
	public class PostRequest : ParameterizedRequest
	{
		public PostRequest() : base () {}

		public new static PostRequest RequestBuilder() {
			return new PostRequest ();
		}

		public new PostRequest AddParam (string key, object value)
		{
			return (PostRequest) base.AddParam (key, value);
		}
	}
}

