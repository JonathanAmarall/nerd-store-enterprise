﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using System;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            #region .: HTTP Services :.

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AddPolicyHandler(PollyExtensions.EsperarTentar())
                    .AddTransientHttpErrorPolicy
                    (
                        p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                    );

            services.AddHttpClient<IComprasBffService, ComprasBffService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AddPolicyHandler(PollyExtensions.EsperarTentar())
                    .AddTransientHttpErrorPolicy
                    (
                        p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                    );

            services.AddHttpClient<IClienteService, ClienteService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                    .AddPolicyHandler(PollyExtensions.EsperarTentar())
                    .AddTransientHttpErrorPolicy(
                        p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            #endregion

            #region Refit
            //services.AddHttpClient("Refit", options =>
            //        {
            //            options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            //        })
            //        .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //        .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>); 
            #endregion
        }
    }
}