using FrontEnd.Models;
using FrontEnd.Models.Dto;
using FrontEnd.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FrontEnd.Services
{
	public class BaseService : IBaseService
	{
		public ResponceDto ResponceModel { get; set; }

		public IHttpClientFactory HttpClient { get; set; }

		public BaseService(IHttpClientFactory httpClient)
		{
			ResponceModel= new ResponceDto();
			HttpClient = httpClient;
		}


		public async Task<T> SendAsync<T>(ApiRequest apiRequest)
		{
			try
			{
				var client = HttpClient.CreateClient("RestaurantAPI");
				var message = new HttpRequestMessage();
				message.Headers.Add("Accept", "application/json");
				message.RequestUri = new Uri(apiRequest.Url);
				client.DefaultRequestHeaders.Clear();
				if (apiRequest.Data != null)
				{
					message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), 
						Encoding.UTF8, "application/json"); 
				}

				if (!string.IsNullOrEmpty(apiRequest.AccessToken))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
				}


				switch(apiRequest.ApiType)
				{
					case StaticData.ApiType.POST:
						message.Method = HttpMethod.Post; break;
					case StaticData.ApiType.PUT:
						message.Method = HttpMethod.Put; break;
					case StaticData.ApiType.DELETE:
						message.Method = HttpMethod.Delete; break;
					default	:
						message.Method = HttpMethod.Get; break;
				}

				var responce = await client.SendAsync(message);
				var apiContent = await responce.Content.ReadAsStringAsync();
				var apiResponceDto = JsonConvert.DeserializeObject<T>(apiContent);
				return apiResponceDto;
			}
			catch (Exception ex)
			{
				var dto = new ResponceDto()
				{
					IsSucces = false,
					DisplayMessage = "Error",
					ErrorMessages = new List<string> { Convert.ToString(ex.Message) }
				};
				var res = JsonConvert.SerializeObject(dto);
				var apiResponceDto = JsonConvert.DeserializeObject<T>(res);
				return apiResponceDto;
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(true);
		}
	}
}
