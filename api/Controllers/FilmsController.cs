using api.Models;
using api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace filmsApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class filmsController : ControllerBase
    {
        private readonly FilmShowingService _filmShowingService;
        private readonly FilmService _filmService;

        public filmsController(FilmService filmService, FilmShowingService filmShowingService)
        {
            _filmShowingService = filmShowingService;
            _filmService = filmService;
        }

        [HttpGet]
        public ActionResult<List<Film>> Get() =>
            _filmService.Get();

        [HttpGet("{id:length(24)}", Name = "Getfilm")]
        public ActionResult<List<FilmShowing>> Get(string id)
        {
            List<FilmShowing> found_film_showings = _filmShowingService.Get().FindAll(film_showing => film_showing.filmId == id);

            if (found_film_showings == null)
            {
                return NotFound();
            }

            return found_film_showings;
        }

        [HttpPost]
        public ActionResult<Film> Create(Film film)
        {
            _filmService.Create(film);

            return CreatedAtRoute("Getfilm", new { id = film.id.ToString() }, film);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Film filmIn)
        {
            var film = _filmService.Get(id);

            if (film == null)
            {
                return NotFound();
            }

            _filmService.Update(id, filmIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var film = _filmService.Get(id);

            if (film == null)
            {
                return NotFound();
            }

            _filmService.Remove(film.id);

            return NoContent();
        }
    }
}