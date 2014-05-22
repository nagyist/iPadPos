using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class Signature
	{
		public string Token { get; set; }

		public Signature()
		{
			Points = new PointF[0];
		}

		PointF[] points;
		[Newtonsoft.Json.JsonIgnore]
		public PointF[] Points
		{
			get
			{
				return points;
			}
			set
			{
				points = value;
				data = Newtonsoft.Json.JsonConvert.SerializeObject(value);
			}
		}

		public bool IsValid
		{
			get { return Points.Length > 2; }
		}

		string data;

		public string Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
				if (string.IsNullOrEmpty(value))
					Points = new PointF[0];
				else
					points = Newtonsoft.Json.JsonConvert.DeserializeObject<PointF[]>(value);
			}
		}
	}
}