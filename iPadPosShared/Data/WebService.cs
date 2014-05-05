using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;

namespace iPadPos
{
	public class WebService 
	{
		static WebService main;
		public static WebService Main {
			get {
				return main ?? (main = new WebService());
			}
			set {
				main = value;
			}
		}
		public WebService ()
		{

		}
		//const string baseUrl = "http://clancey.dyndns.org:83/pos/api/";

		const string baseUrl = "http://10.0.1.36/pos/api/";

		public async Task<List<Customer>> SearchCustomer(string cust)
		{
			return await GetGenericList<Customer>("customer");
		}

		public async Task<Item> GetItem(string id)
		{
			return await Get<Item>("items",id);
		}

	

		public async Task<List<T>> GetGenericList<T>(string path)
		{
			var items = await Get<List<T>>(path);

			return items;
		}


		public Task<Stream> GetUrlStream(string path)
		{
			var client = new HttpClient ();
			client.BaseAddress = new Uri(baseUrl);
			return client.GetStreamAsync (path);
		}


		public Task<string> GetUrl(string path)
		{
			var client = new HttpClient ();
			client.BaseAddress = new Uri(baseUrl);
			return client.GetStringAsync (path);
		}
		public async Task<T> Get<T>(string path,string id = "")
		{
			try{
				Console.WriteLine(path);
				var data = await GetUrl (Path.Combine(path,id));
			Console.WriteLine (data);
			return await Task.Factory.StartNew(()=>{ return Deserialize<T>(data);});
			}
			catch(Exception ex) {
				Console.WriteLine (ex);
				return default(T);
			}
		}
		T Deserialize<T>(string data)
		{
			try{
				return Newtonsoft.Json.JsonConvert.DeserializeObject<T> (data);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
			return default(T);
		}
	}
}

