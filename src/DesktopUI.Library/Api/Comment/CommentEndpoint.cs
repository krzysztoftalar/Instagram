using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Comment
{
    public class CommentEndpoint : ICommentEndpoint
    {
        private readonly IChatHelper _chat;
        private readonly IApiHelper _apiHelper;

        public CommentEndpoint(IChatHelper chat, IApiHelper apiHelper)
        {
            _chat = chat;
            _apiHelper = apiHelper;
        }

        public async Task AddCommentAsync(Models.DbModels.Comment comment)
        {
            try
            {
                if (_chat.Connection.State == HubConnectionState.Connected)
                {
                    await _chat.Connection.InvokeAsync("SendCommentAsync", comment);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CommentsEnvelope> LoadCommentsAsync(string id, int? skip, int? limit)
        {
            using (HttpResponseMessage response =
              await _apiHelper.ApiClient.GetAsync($"/api/comments/{id}?skip={skip}&limit={limit}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<CommentsEnvelope>();
                }

                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
