using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Simpler.Data;

namespace WebApplication1
{

	class SharedDb
	{
		public static SharedDb PosimDb
		{
			get { return posimDb ?? (posimDb = new SharedDb(() => new OdbcConnection(Default))); }
		}

		public static SharedDb SqlServer
		{
			get { return sqlServer ?? (sqlServer = new SharedDb(() => new SqlConnection(ConfigurationManager.ConnectionStrings["sql"].ConnectionString))); }
		}

		static string Default
		{
			get
			{
				var d = ConfigurationManager.ConnectionStrings["sybase"].ConnectionString;
				return d;
			}
		}

		//static string DefaltDatabaseConnection = "Driver=Adaptive Server Anywhere 6;Dsn=Posim;uid=dba;pwd=mtdew;";
		//static string DefaltDatabaseConnection = "Dsn=Posim;uid=dba;pwd=mtdew";
		//static string SqlDatabaseConnection = "Data Source=localhost;Initial Catalog=Affinity;Trusted_Connection=True;";
		static SharedDb posimDb;
		static SharedDb sqlServer;

		Func<IDbConnection> GetConnection { get; set; } 
		public SharedDb(Func<IDbConnection> getConnection)
		{
			this.GetConnection = getConnection;
		}

	
		public T Get<T>(string sql, object data = null)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var obj = Db.GetOne<T>(connection, sql, data);
					//if (obj is IUpdateable)
					//	(obj as IUpdateable).IsDirty = false;
					return obj;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message == "Sequence contains no elements")
					return default(T);
				throw;
			}
		}

		public string GetString(string sql, object data = null)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var obj = Db.GetScalar(connection, sql, data);
					return (string)obj;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message == "Sequence contains no elements" || ex.Message.Contains("DBNull"))
					return null;

				throw;
			}
		}

		public  int GetInt(string sql, object data = null)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var obj = Db.GetScalar(connection, sql, data);
					return (int)obj;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message == "Sequence contains no elements" || ex.Message.Contains("DBNull"))
					return 0;

				throw;
			}
		}


		public  object Get(string sql, object data = null)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var obj = Db.GetScalar(connection, sql, data);
					//if (obj is IUpdateable)
					//	(obj as IUpdateable).IsDirty = false;
					return obj;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message == "Sequence contains no elements")
					return null;
				throw ex;
			}
		}

		public int Execute(string sql, object data = null)
		{
			using (var connection = GetConnection())
			{
				return Db.GetResult(connection, sql, data);
			}
		}

		public T[] GetMany<T>(string sql, object data = null)
		{
			try
			{
				using (var connection = GetConnection())
				{
					var items = Db.GetMany<T>(connection, sql, data);
					//if (typeof(IUpdateable).IsAssignableFrom(typeof(T)))
					//{
					//	foreach (IUpdateable item in items)
					//	{
					//		item.IsDirty = false;
					//	}
					//}
					return items;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message == "Sequence contains no elements")
					return null;
				throw ex;
			}
		}

		//public static int Insert(string sql, object item)
		//{
		//	using (var connection = GetConnection())
		//	{
		//		sql += "\r\n";
		//		sql += "SELECT @@IDENTITY";
		//		var id = Db.GetScalar(connection, sql, item);
		//		return Convert.ToInt32(id);
		//	}
		//}

		//public DataSet GetDataSet(string sql)
		//{
		//	using (var connection = GetConnection())
		//	{
		//		using (var da = new OdbcDataAdapter(sql, connection))
		//		{
		//			DataSet ds = new DataSet();

		//			connection.Open();
		//			da.Fill(ds);
		//			connection.Close();
		//			return ds;
		//		}

		//	}

		//}
	}
}
