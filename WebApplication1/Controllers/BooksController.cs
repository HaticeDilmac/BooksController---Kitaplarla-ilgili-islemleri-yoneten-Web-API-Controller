using BooksDemo.Data;
using BooksDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace BooksDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]//HttpGet metodunu kullanacağız.
        public IActionResult GetAllBooks()
        {
            var books = AplicationContext.Books;
            //ApplicationContext clasımızda eklediğimiz fonksiyonumuzu burada bir değişkene atadık.
            return Ok(books);

        }

        [HttpGet("{id=int}")]//HttpGet metodunu kullanacağız.
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = AplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
            // LINQ , dile entegre sorgu ifadesi.
            // İdsi bizden gelen parametreye eşitleyip geleni getirir.

            if (book == null)
            {
                return NotFound();//404
            }
            return Ok(book);

        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest();
                }
                AplicationContext.Books.Add(book);//apliccationContext sınıfında yer alan Books listesine eleman ekleriz.
                return StatusCode(201, book);//fonksiyonun başarılı olması durumunda 201 hata kodunun ve book nesnesinin dönmesi 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {//Güncellenecek olan elemanın idsi parametre olarak yazılıp ,
         //gerekli bilgi de post işleminde olduğu gibi Body üzerinden yazılarak güncellenecektir.

            //check book?
            var entity = AplicationContext.Books.Find(b => b.Id.Equals(id));
            //Parametreden gelen ifadeyi bul.

            if (entity is null) {//entity değişkeni nul ise entity adlı nesne bulunamadı ifadesini dön.
                return NotFound(entity);
            }

            //chechk id
            if (id != book.Id)
            {
                return BadRequest();//400
            }

            AplicationContext.Books.Remove(entity);//books listesine gidiyoruz ve entityyi tamamen siliyoruz.
            book.Id = entity.Id;//entity değişkeninin id değerini book değerinin id değeri ile eşitliyoruz.
            AplicationContext.Books.Add(book);//parametredeki kitabı listeye ekleriz.
            return Ok(book);

        }

        [HttpDelete]
        public IActionResult DeleteAllBook()
        {
            AplicationContext.Books.Clear();
            return NoContent();//204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id" ) ] int id)
        {
            var entity = AplicationContext.Books.Find(b => b.Id.Equals(id));
            if(entity is null)
            {
                return NotFound(new
                {
                statusCode = 404 ,
                message = $"Book with id:{id} could not found."
                }); //404
            }
            AplicationContext.Books.Remove(entity);
            return NoContent();
        }


        [HttpPatch("{id:int}")]
        public IActionResult partialltUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //check entity
            var entity = AplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
                return NotFound();//404

            bookPatch.ApplyTo(
                entity);
            return NoContent();//204
        }
    }
}
