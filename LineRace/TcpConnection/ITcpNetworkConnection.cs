using System;
using System.Threading.Tasks;

namespace LineRace
{
	public interface ITcpNetworkConnection
	{
		Task UpdateNetworkData<T>(T obj);
		event Action<object> OnGetNetworkData;
		void UnsubscribeActions();
	}
}
