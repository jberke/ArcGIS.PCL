﻿using ArcGIS.ServiceModel;
using ArcGIS.ServiceModel.Common;
using ArcGIS.ServiceModel.Operation;
using ArcGIS.ServiceModel.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ArcGIS.Test
{
    public class ArcGISOnlineTests
    {
        //[Fact]
        //public async Task CanSearchForHostedFeatureServices()
        //{
        //    var serializer = new ServiceStackSerializer();
        //    var gateway = new ArcGISOnlineGateway(serializer, new ArcGISOnlineTokenProvider("", "", serializer));

        //    var hostedServices = await gateway.DescribeSite();

        //    Assert.NotNull(hostedServices);
        //    Assert.Null(hostedServices.Error);
        //    Assert.NotNull(hostedServices.Results);
        //    Assert.NotEmpty(hostedServices.Results);
        //    Assert.False(hostedServices.Results.All(s => String.IsNullOrWhiteSpace(s.Id)));
        //}
        
        //[Fact]
        //public async Task OAuthTokenCanBeGenerated()
        //{
        //    // Set your client Id and secret here
        //    var tokenProvider = new ArcGISOnlineAppLoginOAuthProvider("", "", _serviceStackSerializer);

        //    var token = await tokenProvider.CheckGenerateToken();

        //    Assert.NotNull(token);
        //    Assert.NotNull(token.Value);
        //    Assert.False(token.IsExpired);
        //    Assert.Null(token.Error);
        //}
    }
}