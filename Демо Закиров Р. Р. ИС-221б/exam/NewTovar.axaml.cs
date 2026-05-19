using System;
using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia.Media;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.IO;
using System.Linq;
using Monit;

namespace exam;
#pragma warning disable
public partial class NewTovar : Window
{
    public ObservableCollection<Tovar> tov { get; set; }
    private AppDbContext _context;

    private int itemdata1;

    public string newUser{ get; set; } = null;

    public NewTovar(Tovar pr)
    {
        InitializeComponent(); 

        ExitButton.Click += (_,__) => Close();
        OkButton.Click += (_,__) => AddUserFun(pr);
        _context = new AppDbContext();


        
        _context.Users.Load();
        tov = _context.Tovars.Local.ToObservableCollection();

        Box0.Text = pr.article;
        Box1.Text = pr.name;
        Box2.Text = pr.value;
        Box3.Text = Convert.ToString(pr.price);
        Box4.Text = pr.deliver;
        Box5.Text = pr.producer;
        Box6.Text = pr.category;
        Box7.Text = Convert.ToString(pr.sale);
        Box8.Text = Convert.ToString(pr.count);
        Box9.Text = pr.description;




        Box1.TextChanged += (_,__) => textBox_White(Box1);
        Box2.TextChanged += (_,__) => textBox_White(Box2);
        Box3.TextChanged += (_,__) => textBox_White(Box3);
        Box4.TextChanged += (_,__) => textBox_White(Box4);
        Box5.TextChanged += (_,__) => textBox_White(Box5);
        Box6.TextChanged += (_,__) => textBox_White(Box6);
        Box7.TextChanged += (_,__) => textBox_White(Box7);
        Box8.TextChanged += (_,__) => textBox_White(Box8);
        Box9.TextChanged += (_,__) => textBox_White(Box9);

    }

        public void textBox_White(TextBox sender){
        sender.Foreground = new SolidColorBrush(Colors.Black);
        sender.BorderBrush = new SolidColorBrush(Colors.Black);
    }
    public void textBox_Red(TextBox sender){
        sender.Foreground = new SolidColorBrush(Colors.Red);
        sender.BorderBrush = new SolidColorBrush(Colors.Red);
    }

    private void AddUserFun(Tovar pr)
    {
            
            //try {

                Tovar Upd = new ObservableCollection<Tovar>(_context.Tovars.Local.Where(p => p.ID == pr.ID)).FirstOrDefault();
            tov.Remove(Upd);

            Tovar newUser = new Tovar {ID = pr.ID, article = Box0.Text, name = Box1.Text , value = Box2.Text, price = Math.Round( Convert.ToDecimal(Box3.Text), 2)
            , deliver = Box4.Text , producer = Box5.Text  , category = Box6.Text, sale = Math.Round( Convert.ToDecimal(Box7.Text), 2),
             count = Convert.ToInt32(Box8.Text), description = Box9.Text, photography = pr.photography};

        
            tov.Add(newUser);
            SaveChangesAsync();
           
            

            try {if (Box0.Text==null){textBox_Red(Box0);}
                if (Box1.Text==null){textBox_Red(Box1);}
                if (Box2.Text==null){textBox_Red(Box3);}

                }catch{}
            //}catch{}
    }

            private async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
             Close();
        }
        catch (DbUpdateException ex)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Ошибка сохранения", ex.InnerException?.Message ?? ex.Message, ButtonEnum.Ok);
            await box.ShowWindowDialogAsync(this);
        }
    }
}
