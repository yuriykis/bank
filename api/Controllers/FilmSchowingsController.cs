using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace filmShowingsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class filmShowingsController : ControllerBase
    {
        private readonly FilmShowingService _filmShowingService;
        private readonly WatcherService _watcherService;

        public filmShowingsController(FilmShowingService filmShowingService, WatcherService watcherService)
        {
            _filmShowingService = filmShowingService;
            _watcherService = watcherService;
        }

        [HttpGet]
        public ActionResult<List<FilmShowing>> Get() =>
            _filmShowingService.Get();

        [HttpGet("avaliableSeats/{filmShowingId:length(24)}", Name = "GetfilmShowing")]
        public ActionResult<List<int>> Get(string filmShowingId)
        {
            List<Watcher> found_watchers = _watcherService.Get().FindAll(foundWatchers => foundWatchers.filmShowingId == filmShowingId);
            FilmShowing film_showing = _filmShowingService.Get().Find(filmShowing => filmShowing.id == filmShowingId);
            List<int> avaliableSeats = Enumerable.Range(1, film_showing.numberOfSeatsInRoom).ToList();
            foreach (Watcher watcher in found_watchers)
            {
                avaliableSeats.Remove(watcher.seatNumber);
            }

            return avaliableSeats;
        }

        [HttpPost]
        public ActionResult<FilmShowing> Create(FilmShowing filmShowing)
        {
            _filmShowingService.Create(filmShowing);

            return CreatedAtRoute("GetfilmShowing", new { id = filmShowing.id.ToString() }, filmShowing);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, FilmShowing filmShowingIn)
        {
            var filmShowing = _filmShowingService.Get(id);

            if (filmShowing == null)
            {
                return NotFound();
            }

            _filmShowingService.Update(id, filmShowingIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var filmShowing = _filmShowingService.Get(id);

            if (filmShowing == null)
            {
                return NotFound();
            }

            _filmShowingService.Remove(filmShowing.id);

            return NoContent();
        }
    }
}