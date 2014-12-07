﻿namespace ArcGIS.ServiceModel
{
    using ArcGIS.ServiceModel.Operation;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class FederatedTokenProvider : ITokenProvider, IDisposable
    {
        HttpClient _httpClient;
        protected readonly GenerateFederatedToken TokenRequest;
        Token _token;

        /// <summary>
        /// Create a token provider to authenticate against an ArcGIS Server that is federated
        /// </summary>
        /// <param name="tokenProvider"></param>
        /// <param name="rootUrl"></param>
        /// <param name="serverUrl"></param>
        /// <param name="serializer">Used to (de)serialize requests and responses</param>
        /// <param name="referer">Referer url to use for the token generation. For federated servers this will be the portal rootUrl</param>
        public FederatedTokenProvider(ITokenProvider tokenProvider, string rootUrl, string serverUrl, ISerializer serializer = null, string referer = null)
        {
            Guard.AgainstNullArgument("tokenProvider", tokenProvider);
            if (string.IsNullOrWhiteSpace(rootUrl)) throw new ArgumentNullException("rootUrl", "rootUrl is null.");
            Guard.AgainstNullArgument("serverUrl", serverUrl);

            Serializer = serializer ?? SerializerFactory.Get();
            if (Serializer == null) throw new ArgumentNullException("serializer", "Serializer has not been set.");

            RootUrl = rootUrl.AsRootUrl();
            _httpClient = HttpClientFactory.Get();
            TokenRequest = new GenerateFederatedToken(serverUrl, tokenProvider) { Referer = referer };
        }

        ~FederatedTokenProvider()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                    _httpClient = null;
                }
                _token = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ISerializer Serializer { get; private set; }

        public ICryptoProvider CryptoProvider { get { return null; } }

        public string RootUrl { get; private set; }

        public string UserName { get { return null; } }

        public async Task<Token> CheckGenerateToken(CancellationToken ct)
        {
            if (TokenRequest == null) return null;

            if (_token != null && !_token.IsExpired) return _token;

            _token = null; // reset the Token

            TokenRequest.FederatedToken = await TokenRequest.TokenProvider.CheckGenerateToken(ct).ConfigureAwait(false);

            HttpContent content = new FormUrlEncodedContent(Serializer.AsDictionary(TokenRequest));

            _httpClient.CancelPendingRequests();

            var url = TokenRequest.BuildAbsoluteUrl(RootUrl).Split('?').FirstOrDefault();
            Uri uri;
            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uri);
            if (!validUrl)
                throw new HttpRequestException(string.Format("Not a valid url: {0}", url));

            string resultString = string.Empty;
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content, ct).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return null;
            }

            var result = Serializer.AsPortalResponse<Token>(resultString);

            if (result.Error != null) throw new InvalidOperationException(result.Error.ToString());

            _token = result;
            return _token;
        }
    }
}