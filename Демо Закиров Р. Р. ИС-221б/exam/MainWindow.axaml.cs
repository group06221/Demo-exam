using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Monit;

namespace exam;

public partial class MainWindow : Window
{
    private AppDbContext _context;

    public List<User> _usr;
    public MainWindow()
    {
        InitializeComponent();

        _context = new AppDbContext();

                    try
            {
                var photo = new Canvas
                {
                    Margin = new Thickness(10),
                    Width = 100,
                    Height = 100,
                    Background = new ImageBrush(new Bitmap($"Assets/icon.png"))
                };
                StackImage.Children.Add(photo);
            }
            catch
            {}

        EnterButton.Click += (_, __) => EnterButtonButton_Click();
        GuestButton.Click += (_, __) => GuestButtonButton_Click();

    }

    private void GuestButtonButton_Click()
    {
        var main = new SecondWindow(null);
        main.Show();
        Close();
    }
    
        private async Task EnterButtonButton_Click()
    {
        if (await _context.Users
           .FromSql($"SELECT * FROM public.Пользователи WHERE Логин = {LoginBox.Text}")
           .FirstOrDefaultAsync() != null)
        {
            User Current_user = await _context.Users
           .FromSql($"SELECT * FROM public.Пользователи WHERE Логин = {LoginBox.Text} and Пароль = {PasswordBox.Text}")
           .FirstOrDefaultAsync();

            if (Current_user != null)
            {
                    var main = new SecondWindow(Current_user);
                    main.Show();
                    Close();
            }
                
        }

    }
}