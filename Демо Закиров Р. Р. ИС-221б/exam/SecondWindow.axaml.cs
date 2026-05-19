using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Microsoft.EntityFrameworkCore;
using Monit;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using static Monit.AppDbContext;
namespace exam;

public partial class SecondWindow : Window
{
    private AppDbContext _context;


    public decimal min = 0;
    public decimal max = 1000000000;

    public List<Tovar> tov;

    public List<Order> _ord;


    public bool shop_open = true;

    public bool admin = false;




    public SecondWindow(User Cur_user)
    {
        InitializeComponent();

        _context = new AppDbContext();
        _context.Tovars.Load();
        tov = _context.Tovars.Local.ToList();




        if (Cur_user != null)
        {
            User_name.Text = Cur_user.name;
            switch (Cur_user.role)
            {
                case "Администратор":
                    Navigation.IsVisible = true;
                    Navigation2.IsVisible = true;
                    AddButton.IsVisible = true;
                    LoadCheckl();
                    admin = true;
                    Guest(null, admin);
                    break;
                case "Менеджер":
                    Navigation.IsVisible = true;
                    Navigation2.IsVisible = true;
                    LoadCheckl();
                    Guest(null, false);
                    break;
                default:
                    Guest(null, false);
                    break;
            }
        }
        else
        {
            Guest(null, admin);
        }
        OrderButton.Click += (_, __) => OrderButton_Check();
        ExitButton.Click += (_, __) => ExitButton_Click();
        Searchbox.TextChanged += (_, __) => SearchButton_Click();
        AddButton.Click += (_, __) => AddButton_Click();
    }

        private async Task ExitButton_Click()
    {
        var main = new MainWindow();
        main.Show();
        Close();
    }

        public void AddButton_Click()
    {
                if (shop_open)
        {
            TovarChangedButton_ClickAsync(new Tovar{photography = ""} );
        } else{
            //OrserChangedButton_ClickAsync(new Tovar{photography = ""} );
        }
    }

    private void SearchButton_Click()
    {
        Console.WriteLine(1);
        if (shop_open)
        {
            _context.Tovars.Load();
            tov = _context.Tovars.Local.ToList();

            IEnumerable<Tovar> result = tov;

            foreach (string item in ShopCheckl.SelectedItems)
            {

                switch (item)
                {
                    case "Артикул":
                        result = result.Where(e => e.article.StartsWith(Searchbox.Text));
                        break;
                    case "Наименование товара":
                        result = result.Where(e => e.name.StartsWith(Searchbox.Text));
                        break;
                    case "Единица измерения":
                        result = result.Where(e => e.value.StartsWith(Searchbox.Text));
                        break;
                    case "Цена":
                        result = result.Where(e => Convert.ToString(e.price).StartsWith(Searchbox.Text));
                        break;
                    case "Поставщик":
                        result = result.Where(e => e.deliver.StartsWith(Searchbox.Text));
                        break;
                    case "Производитель":
                        result = result.Where(e => e.producer.StartsWith(Searchbox.Text));
                        break;
                    case "Категория товара":
                        result = result.Where(e => e.category.StartsWith(Searchbox.Text));
                        break;
                    case "Действующая скидка":
                        result = result.Where(e => Convert.ToString(e.sale).StartsWith(Searchbox.Text));
                        break;
                    case "Кол-во на складе":
                        result = result.Where(e => Convert.ToString(e.count).StartsWith(Searchbox.Text));
                        break;
                    case "Описание товара":
                        result = result.Where(e => e.description.StartsWith(Searchbox.Text));
                        break;
                }

                //var sortedUsers = users.OrderBy(u => u.Age);


            }
            Guest(result.ToList(), false);

        } else{
            _context.Orders.Load();
            _ord = _context.Orders.Local.ToList();

            IEnumerable<Order> result = _ord;

            foreach (string item in OrderCheckl.SelectedItems)
            {
                switch (item)
                {
                    case "Артикул заказа":
                        result = result.Where(e => e.article.StartsWith(Searchbox.Text));
                        break;
                    case "Дата заказа":
                        result = result.Where(e =>  Convert.ToString(e.date).StartsWith(Searchbox.Text));
                        break;
                    case "Дата доставки":
                        result = result.Where(e => Convert.ToString(e.complete_date).StartsWith(Searchbox.Text));
                        break;
                    case "Адрес пункта выдачи": 
                        result = result.Where(e => Convert.ToString(e.adress).StartsWith(Searchbox.Text));
                        break;
                    case "ФИО авторизированного клиента":
                        result = result.Where(e => e.name.StartsWith(Searchbox.Text));
                        break;
                    case "Код для получения":
                        result = result.Where(e => e.code.StartsWith(Searchbox.Text));
                        break;
                    case "Статус заказа":
                        result = result.Where(e => e.status.StartsWith(Searchbox.Text));
                        break;
                }

                //var sortedUsers = users.OrderBy(u => u.Age);


            }
            Orders(result.ToList(), false);
        }



    }




    private void LoadCheckl()
    {

        ShopCheckl.ItemsSource = new string[]
                {"Артикул",  "Наименование товара",  "Единица измерения",
            "Цена","Поставщик","Производитель","Категория товара",  "Действующая скидка",   "Кол-во на складе", "Описание товара"};

        OrderCheckl.ItemsSource = new string[]
            {"Артикул заказа",
            "Дата заказа",  "Дата доставки",    "Адрес пункта выдачи",  "ФИО авторизированного клиента",    "Код для получения",    "Статус заказа"
            };

    }

    private void OrderButton_Check()
    {
        if (shop_open) // Заказы активны
        {
            shop_open = false;
            OrderButton.Content = "Товары";
            OrderCheckl.IsVisible = true;
            ShopCheckl.IsVisible = false;
            Orders(null, admin);
        }
        else // Товары активны
        {
            Guest(null, admin);
            OrderButton.Content = "Заказы";
            OrderCheckl.IsVisible = false;
            ShopCheckl.IsVisible = true;
            shop_open = true;
        }

    }




    private async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            Guest(null, true);
        }
        catch (DbUpdateException ex)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Ошибка сохранения", ex.InnerException?.Message ?? ex.Message, ButtonEnum.Ok);
            await box.ShowWindowDialogAsync(this);
        }

    }

    private async Task Orders(List<Order> _ord, bool admin)
    {
        if (_ord == null)
        {
            _context.Orders.Load();
            _ord = _context.Orders.Local.ToList();
        }
        Back.Children.Clear();
        int yy = 0;
        int xx = 0;
        foreach (Order pr in _ord)
        {
            var StackMain = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };


            var StackInfo = new StackPanel
            {
                Width = 600,
                Orientation = Avalonia.Layout.Orientation.Vertical,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };

            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Артикул: {pr.article}",
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            });

            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Статус: {pr.status}",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            }
            );



            string adres = "";
            _context.Adresses.Load();
            if (await _context.Adresses
                        .FromSql($"SELECT * FROM public.Адреса WHERE id = {pr.adress}")
                        .FirstOrDefaultAsync() != null)
            {
                Adress adr = await _context.Adresses
                    .FromSql($"SELECT * FROM public.Адреса WHERE id = {pr.adress}")
                    .FirstOrDefaultAsync();
                adres = adr.text;
            }

            Console.WriteLine(2);


            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Адрес: {adres}",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            });
            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Дата: {pr.date}",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            });
            StackMain.Children.Add(StackInfo);

            StackMain.Children.Add(new TextBlock
            {
                Text = $"Дата доставки: {pr.complete_date}",
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            });


                if (admin)
            {
                var StackButtons = new StackPanel{
                Margin = new Thickness(300,0,0,0),
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                };
            
                    Button buttondel = new Button
                {
                    Name = $"{pr.ID}",
                    Content = $"Удалить",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    Background = Brush.Parse("#FF4444"),
                    FontSize = 16,
                };

                Button button = new Button
                {
                    Name = $"{pr.ID}",
                    Content = $"Изменить",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    Background = Brush.Parse("#FFDEAD"),
                    FontSize = 16,
                };
                buttondel.Click += (_, __) => OrderDeleteSelectedAsync(pr);
                button.Click += (_, __) => OrderChangedButton_ClickAsync(pr);
                StackButtons.Children.Add(buttondel);
                StackButtons.Children.Add(button);
                StackInfo.Children.Add(StackButtons);
                yy += 20;
            }

            var border = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Width = 900,
                Height = 100,
                Padding = new Thickness(10),
                Margin = new Thickness(xx, yy, 0, 0)
            };

            yy += 110;

            //button.Click += (_, __) => AddButton_Click(pr);

            //Stack.Children.Add(button);

            border.Child = StackMain;




            Back.Children.Add(border);


        }
        ;
        Back.Height = yy + 100;



    }



    private void Guest(List<Tovar> tov, bool admin)
    {
        if (tov == null)
        {
            _context.Tovars.Load();
            tov = _context.Tovars.Local.ToList();
        }
        Back.Children.Clear();
        int yy = 0;
        int xx = 0;
        foreach (Tovar pr in tov)
        {
            var StackMain = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };

            try
            {
                var photo = new Canvas
                {
                    Margin = new Thickness(10),
                    Width = 100,
                    Height = 100,
                    Background = new ImageBrush(new Bitmap($"images/{pr.photography}"))
                };
                StackMain.Children.Add(photo);
            }
            catch
            {
                var photo = new Canvas
                {
                    Margin = new Thickness(10),
                    Width = 100,
                    Height = 100,
                    Background = new ImageBrush(new Bitmap($"images/picture.png"))
                };
                StackMain.Children.Add(photo);
            }

            var StackInfo = new StackPanel
            {
                Width = 600,
                Orientation = Avalonia.Layout.Orientation.Vertical,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };

            StackInfo.Children.Add(new TextBlock
            {
                Text = $"{pr.category}|{pr.name}    ",
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            });

            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Описание товара: {pr.description}",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            }
            );

            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Производитель: {pr.producer}",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            });
            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Поставщик: {pr.deliver}",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                FontSize = 16
            });

            var Price_Stack = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left
            };
            if (pr.sale > 0)
            {
                Price_Stack.Children.Add(new TextBlock
                {

                    Text = $"Цена: ",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    FontSize = 16
                }
                );

                var price = new TextBlock
                {

                    Text = $"{pr.price}руб",
                    TextDecorations = TextDecorations.Strikethrough,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = Brushes.Red,

                    FontSize = 16
                };
                Price_Stack.Children.Add(price);

                var real_price = new TextBlock
                {
                    Text = $"   {pr.price - pr.price * pr.sale / 100}руб",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    Foreground = Brushes.Black,
                    FontSize = 16
                };
                Price_Stack.Children.Add(real_price);
            }
            else
            {
                var price = new TextBlock
                {
                    Text = $"Цена:{pr.price}руб",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    Foreground = Brushes.Black,
                    FontSize = 16
                };
                Price_Stack.Children.Add(price);
            }

            StackInfo.Children.Add(Price_Stack);

            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Ежиница измерения: {pr.value}",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Center,
                FontSize = 16
            });

                        var bgr = Brush.Parse("#000000");
            if (pr.count == 0)
            {
                bgr = Brush.Parse("#aaaaff");
            }

            StackInfo.Children.Add(new TextBlock
            {
                Text = $"Количество на сладе: {pr.count}",
                TextWrapping = TextWrapping.Wrap,
                Foreground = bgr,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Center,
                FontSize = 16
            });

            StackMain.Children.Add(StackInfo);
            if (pr.sale > 0)
            {
                var sale = new TextBlock
                {
                    Text = $"  -{pr.sale}%  ",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    FontSize = 32
                };
                StackMain.Children.Add(sale);
            }


             bgr = Brush.Parse("#ffffff");
            if (pr.sale >= 17)
            {
                bgr = Brush.Parse("#ffDead");
            }
            if (admin)
            {
                var StackButtons = new StackPanel{
                Margin = new Thickness(300,0,0,0),
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                };
            
                    Button buttondel = new Button
                {
                    Name = $"{pr.ID}",
                    Content = $"Удалить",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    Background = Brush.Parse("#FF4444"),
                    FontSize = 16,
                };

                Button button = new Button
                {
                    Name = $"{pr.ID}",
                    Content = $"Изменить",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    Background = Brush.Parse("#FFDEAD"),
                    FontSize = 16,
                };
                buttondel.Click += (_, __) => TovarDeleteSelectedAsync(pr);
                button.Click += (_, __) => TovarChangedButton_ClickAsync(pr);
                StackButtons.Children.Add(buttondel);
                StackButtons.Children.Add(button);
                StackInfo.Children.Add(StackButtons);
                yy += 20;
            }

            var border = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Background = bgr,
                Width = 900,
                Height = 340,
                Padding = new Thickness(10),
                Margin = new Thickness(xx, yy, 0, 0)
            };

            yy += 360;
            border.Child = StackMain;

            Back.Children.Add(border);


        };
        Back.Height = yy + 100;



    }


    private async Task TovarChangedButton_ClickAsync(Tovar pr)
    {
        var newwindow = new NewTovar(pr);
        newwindow.Closed += (_, __) => Guest(null, admin);
        await newwindow.ShowDialog(this);
        
        
    }


    private async Task TovarDeleteSelectedAsync(Tovar pr)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "При удалении данные товара будет безвозратно потеряны", ButtonEnum.OkCancel);
        var result = await box.ShowAsync();
        if (result == ButtonResult.Ok)
        {
                _context.Tovars.Remove(pr);
                tov.Remove(pr);
                SaveChangesAsync();
        }  
    }

        private async Task OrderChangedButton_ClickAsync(Order pr)
    {
        //var newwindow = new NewTovar(pr.ID);
        //newwindow.ShowDialog(this);
        Orders(null, admin);
    }

    private async Task OrderDeleteSelectedAsync(Order pr)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "При удалении заказа данные будет безвозратно потеряны", ButtonEnum.OkCancel);
        var result = await box.ShowAsync();
        if (result == ButtonResult.Ok)
        {
                _context.Orders.Remove(pr);
                _ord.Remove(pr);
                SaveChangesAsync();
        }  
    }
    











}