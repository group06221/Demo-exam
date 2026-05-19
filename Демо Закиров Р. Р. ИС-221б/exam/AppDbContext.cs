using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monit;

public class AppDbContext() : DbContext
{
    public DbSet<Tovar> Tovars => Set<Tovar>();
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Adress> Adresses => Set<Adress>();

    public DbSet<User> Users => Set<User>();
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options
        .UseNpgsql(
            $"Host=localhost;Port=5432;Database=postgres;Password=12345;Username=postgres;SearchPath=public"
        )
        .UseLowerCaseNamingConvention();
    //.LogTo(Console.WriteLine) ;

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Tovar>().ToTable("Товар");
        model.Entity<Order>().ToTable("Заказы");
        model.Entity<User>().ToTable("Пользователи");
        model.Entity<Adress>().ToTable("Адреса");
    }

}

    public class Tovar
    {
        [Column("id")] public int ID { get; set; }

        [Column("Артикул")] public string article { get; set; } = string.Empty;

        [Column("Наименование_товара")] public string name { get; set; } = string.Empty;

        [Column("Единица_измерения")] public string value { get; set; } = string.Empty;

        [Column("Цена")] public decimal price { get; set; }

        [Column("Поставщик")] public string deliver { get; set; } = string.Empty;

        [Column("Производитель")] public string producer { get; set; } = string.Empty;

        [Column("Категория_товара")] public string category { get; set; } = string.Empty;

        [Column("Действующая_скидка")] public decimal sale { get; set; }

        [Column("Кол_во_на_складе")] public int count { get; set; }

        [Column("Описание_товара")] public string description { get; set; } = string.Empty;

        [Column("Фото")] public string photography { get; set; } = string.Empty;
    }
public class Adress
{
    [Key][Column("id")] public int ID { get; set; }

    [Column("Адрес")] public string text { get; set; }

}
public class Order
{
    [Column("Номер_заказа")] public int ID { get; set; }

    [Column("Артикул_заказа")] public string article { get; set; }

    [Column("Дата_заказа")] public DateOnly date { get; set; }

    [Column("Дата_доставки")] public DateOnly complete_date { get; set; }

    [Column("Адрес_пункта_выдачи")] public int adress { get; set; }

    [Column("ФИО_авторизированного_клиента")] public string name { get; set; }

    [Column("Код_для_получения")] public string code { get; set; }

    [Column("Статус_заказа")] public string status { get; set; } 
}

        public class User
    {
        [Key][Column("id")] public int ID { get; set; }

        [Column("Роль_сотрудника")] public string role { get; set; } = string.Empty;
        [Column("ФИО")] public string name { get; set; } = string.Empty;
        [Column("Логин")] public string login { get; set; } = string.Empty;
        [Column("Пароль")] public string password { get; set; } = string.Empty;
    }


    

