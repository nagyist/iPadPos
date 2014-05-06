using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Text;

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

		//const string baseUrl = "http://10.0.1.36/pos/api/";
		const string baseUrl = "http://10.0.1.14:32021/api/";

		public async Task<List<Customer>> SearchCustomer(string cust)
		{
			return await GetGenericList<Customer>("customer");
		}

		public async Task<Item> GetItem(string id)
		{
			return await Get<Item>("items",id);
		}
		public async Task<Customer> GetCustomer(string id)
		{
			return await Get<Customer>("Customer",id);
		}
	

		public async Task<List<T>> GetGenericList<T>(string path)
		{
			var items = await Get<List<T>>(path);

			return items;
		}


		public Task<Stream> GetUrlStream(string path)
		{
			var client = CreateClient ();
			return client.GetStreamAsync (path);
		}


		public Task<string> GetUrl(string path)
		{
			var client = CreateClient ();
			return client.GetStringAsync (path);
		}

		HttpClient CreateClient()
		{
			var client = new HttpClient ();
			client.BaseAddress = new Uri(baseUrl);
			return client;
		}

		public async Task<bool> PostInvoice(Invoice invoice)
		{
			var client = CreateClient ();
			var json = Newtonsoft.Json.JsonConvert.SerializeObject (invoice);
			var respons = await client.PostAsync ("Invoice", new StringContent (json, Encoding.UTF8,"application/json"));
			var success = bool.Parse(await respons.Content.ReadAsStringAsync ());
			return success;
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

