using Microsoft.AspNetCore.Mvc.ApplicationModels;
using BooksDemo.Models;
namespace BooksDemo.Data
{
    public class AplicationContext
    {
        public static List<Book> Books { get; set; }//Liste tanımladık.

        static AplicationContext()
        {
            Books = new List<Book>()
            {
                new Book() { Id = 1, Title = "Kürk Mantolu Madonna", Price = 50 },
                new Book() { Id = 2, Title = "Toprak Ana", Price = 30 },
                new Book() { Id = 3, Title = "Kuyucaklı Yusuf", Price = 50 }
            };
        }
    }  
}
