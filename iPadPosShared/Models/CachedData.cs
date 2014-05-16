using System;
using System.Threading.Tasks;

namespace iPadPos
{
	public class CachedData <T>
	{
		public CachedData ()
		{
		}

		public Task<T> LocalData { get; set; }

		public Task<T> RemoteData { get; set; }
	}
}

