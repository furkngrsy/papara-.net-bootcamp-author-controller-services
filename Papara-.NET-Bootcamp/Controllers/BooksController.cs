using Papara_Bootcamp.Models;
using Papara_Bootcamp.Models.Input;
using Papara_Bootcamp.Models.Output;
using Papara_Bootcamp.Services;
using Papara_Bootcamp.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Papara_Bootcamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<BookOutputModel>>> GetBooks()
        {
            var books = _bookService.GetAllBooks();

            // Entity'den Output modeline dönüşüm
            var outputBooks = books.Select(b => new BookOutputModel
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                PageCount = b.PageCount,
                Year = b.Year
            });

            return Ok(new ApiResponse<IEnumerable<BookOutputModel>>
            {
                Success = true,
                Message = "Kitaplar başarılı bir şekilde getirildi.",
                Data = outputBooks,
                StatusCode = 200
            });
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<BookOutputModel>> GetBook(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound(new ApiResponse<BookOutputModel>
                {
                    Success = false,
                    Message = "Kitap bulunamadı.",
                    StatusCode = 404
                });
            }

            string isClassic = (book.IsClassic()) ? "Bu kitap klasik bir eserdir." : "Bu kitap klasik bir eser değildir.";

            // Entity'den Output modeline dönüşüm
            var outputBook = new BookOutputModel
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                PageCount = book.PageCount,
                Year = book.Year
            };

            return Ok(new ApiResponse<BookOutputModel>
            {
                Success = true,
                Message = $"Kitap başarılı bir şekilde getirildi. {isClassic}",
                Data = outputBook,
                StatusCode = 200
            });
        }

        [HttpPost]
        public ActionResult<ApiResponse<BookOutputModel>> PostBook([FromBody] CreateBookInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Doğrulama hatası.",
                    Data = errors,
                    StatusCode = 400
                });
            }

            // Input modelinden Entity'ye dönüşüm
            var book = new Book
            {
                Name = inputModel.Name,
                Author = inputModel.Author,
                PageCount = inputModel.PageCount,
                Year = inputModel.Year
            };

            _bookService.AddBook(book);

            // Entity'den Output modeline dönüşüm
            var outputBook = new BookOutputModel
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                PageCount = book.PageCount,
                Year = book.Year
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, new ApiResponse<BookOutputModel>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde eklendi.",
                Data = outputBook,
                StatusCode = 201
            });
        }

        [HttpPut("{id}")]
        public IActionResult PutBook(int id, [FromBody] UpdateBookInputModel inputModel)
        {
            var existingBook = _bookService.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound(new ApiResponse<BookOutputModel>
                {
                    Success = false,
                    Message = "Kitap bulunamadı.",
                    StatusCode = 404
                });
            }

            // Input modelinden Entity'ye güncelleme
            existingBook.Name = inputModel.Name;
            existingBook.Author = inputModel.Author;
            existingBook.PageCount = inputModel.PageCount;
            existingBook.Year = inputModel.Year;

            _bookService.UpdateBook(id, existingBook);

            // Entity'den Output modeline dönüşüm
            var outputBook = new BookOutputModel
            {
                Id = existingBook.Id,
                Name = existingBook.Name,
                Author = existingBook.Author,
                PageCount = existingBook.PageCount,
                Year = existingBook.Year
            };

            return Ok(new ApiResponse<BookOutputModel>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde güncellendi.",
                Data = outputBook,
                StatusCode = 200
            });
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBook(int id, [FromBody] UpdateBookInputModel updatedFields)
        {
            var existingBook = _bookService.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound(new ApiResponse<BookOutputModel>
                {
                    Success = false,
                    Message = "Kitap bulunamadı.",
                    StatusCode = 404
                });
            }

            // Input modelinden Entity'ye güncelleme
            if (!string.IsNullOrEmpty(updatedFields.Name))
            {
                existingBook.Name = updatedFields.Name;
            }
            if (!string.IsNullOrEmpty(updatedFields.Author))
            {
                existingBook.Author = updatedFields.Author;
            }
            if (updatedFields.PageCount != 0)
            {
                existingBook.PageCount = updatedFields.PageCount;
            }
            if (updatedFields.Year != 0)
            {
                existingBook.Year = updatedFields.Year;
            }

            _bookService.UpdateBook(id, existingBook);

            // Entity'den Output modeline dönüşüm
            var outputBook = new BookOutputModel
            {
                Id = existingBook.Id,
                Name = existingBook.Name,
                Author = existingBook.Author,
                PageCount = existingBook.PageCount,
                Year = existingBook.Year
            };

            return Ok(new ApiResponse<BookOutputModel>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde güncellendi.",
                Data = outputBook,
                StatusCode = 200
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound(new ApiResponse<BookOutputModel>
                {
                    Success = false,
                    Message = "Kitap bulunamadı.",
                    StatusCode = 404
                });
            }

            _bookService.DeleteBook(id);

            // Entity'den Output modeline dönüşüm
            var outputBook = new BookOutputModel
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                PageCount = book.PageCount,
                Year = book.Year
            };

            return Ok(new ApiResponse<BookOutputModel>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde silindi.",
                Data = outputBook,
                StatusCode = 200
            });
        }

        [HttpGet("list")]
        public ActionResult<ApiResponse<IEnumerable<BookOutputModel>>> ListBooks([FromQuery] string name)
        {
            var filteredBooks = _bookService.GetAllBooks().AsQueryable();

            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new ApiResponse<IEnumerable<BookOutputModel>>
                {
                    Success = false,
                    Message = "Geçersiz parametre.",
                    StatusCode = 400
                });
            }

            filteredBooks = filteredBooks.Where(b => b.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            filteredBooks = filteredBooks.OrderBy(b => b.Id);

            // Entity'den Output modeline dönüşüm
            var outputBooks = filteredBooks.Select(b => new BookOutputModel
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                PageCount = b.PageCount,
                Year = b.Year
            });

            return Ok(new ApiResponse<IEnumerable<BookOutputModel>>
            {
                Success = true,
                Message = "Kitap başarılı bir şekilde getirildi.",
                Data = outputBooks.ToList(),
                StatusCode = 200
            });
        }
    }
}
