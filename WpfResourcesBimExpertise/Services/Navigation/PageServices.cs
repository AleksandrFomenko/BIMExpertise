﻿using System.Windows;
using Wpf.Ui.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace WpfResourcesBimExpertise.Services.Navigation;

public class PageService : INavigationViewPageProvider
{
    private readonly IServiceProvider _serviceProvider;
    
    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public object GetPage(Type pageType)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return _serviceProvider.GetRequiredService(pageType) as FrameworkElement;
    }
}