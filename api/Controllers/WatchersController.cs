using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace watchersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class watchersController : ControllerBase
    {
        private readonly WatcherService _watcherService;
        private readonly UserService _userService;

        public watchersController(WatcherService watcherService, UserService userService)
        {
            _watcherService = watcherService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<Watcher>> Get() =>
            _watcherService.Get();

        [HttpGet("{id:length(24)}", Name = "Getwatcher")]
        public ActionResult<Watcher> Get(string id)
        {
            var watcher = _watcherService.Get(id);

            if (watcher == null)
            {
                return NotFound();
            }

            return watcher;
        }

        [HttpPost]
        public ActionResult<int> Create([FromBody] Dictionary<String, String> watcherData)
        {
            int seatNumber = int.Parse(watcherData["seat_number"]);
            string filmShowingId = watcherData["filmShowingId"];
            string userName = watcherData["user_name"];
            Watcher watcher = _watcherService.Get().Find(w => w.seatNumber == seatNumber && w.filmShowingId == filmShowingId);
            User user = _userService.Get().Find(w => w.name == userName);
            if (watcher == null && user != null)
            {
                Watcher new_watcher = new Watcher { seatNumber = seatNumber, userId = user.id, filmShowingId = filmShowingId };
                _watcherService.Create(new_watcher);
                return 200;
            }
            else
            {
                return 400;
            }
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Watcher watcherIn)
        {
            var watcher = _watcherService.Get(id);

            if (watcher == null)
            {
                return NotFound();
            }

            _watcherService.Update(id, watcherIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var watcher = _watcherService.Get(id);

            if (watcher == null)
            {
                return NotFound();
            }

            _watcherService.Remove(watcher.id);

            return NoContent();
        }
    }
}