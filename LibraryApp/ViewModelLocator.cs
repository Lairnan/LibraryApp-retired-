﻿using LibraryApp.ViewModels;
using LibraryApp.ViewModels.Add;
using LibraryApp.ViewModels.Edit;
using LibraryApp.ViewModels.PageViews;

namespace LibraryApp;

public class ViewModelLocator
{
    // View model windows
    public static MainViewModel MainViewModel => Ioc.Resolve<MainViewModel>();
    public static DbSettingViewModel DbSettingViewModel => Ioc.Resolve<DbSettingViewModel>();
    
    // View model pages
    public static MainPageViewModel MainPageViewModel => Ioc.Resolve<MainPageViewModel>();
    public static PageBooksViewModel PageBooksViewModel => Ioc.Resolve<PageBooksViewModel>();
    
    // View model edits
    public static EditBookViewModel EditBookViewModel => Ioc.Resolve<EditBookViewModel>();
    
    // View model adds
    public static AddBookViewModel AddBookViewModel => Ioc.Resolve<AddBookViewModel>();
}