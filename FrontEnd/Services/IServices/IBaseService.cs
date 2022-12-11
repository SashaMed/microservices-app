using FrontEnd.Models;
using FrontEnd.Models.Dto;

namespace FrontEnd.Services.IServices
{
	public interface IBaseService : IDisposable
	{
		ResponceDto ResponceModel { get; set; }

		Task<T> SendAsync<T>(ApiRequest apiRequest);
	}
}
