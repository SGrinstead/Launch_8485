using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourism.DataAccess;
using Tourism.Models;

namespace Tourism.Controllers
{
    public class StatesController : Controller
    {
        private readonly TourismContext _context;

        public StatesController(TourismContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? timezone)
        {
            var states = _context.States.AsEnumerable();
            if(timezone != null)
            {
                states = states.Where(state => state.TimeZone == timezone);
                ViewData["SearchTimezone"] = timezone;
            }
            ViewData["AllTimezones"] = _context.States.Select(state => state.TimeZone).Distinct().ToList();
            return View(states);
        }

		public IActionResult New()
		{
			return View();
		}

        [HttpPost]
        [Route("/states/")]
        public IActionResult Create(State state)
        {
            _context.Add(state);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        [Route("states/{stateId:int}")]
        public IActionResult Show(int stateId)
        {
            var state = _context.States.Find(stateId);
            return View(state);
        }

        [Route("/states/{stateId:int}/edit")]
        public IActionResult Edit(int stateId)
        {
            var state = _context.States.Find(stateId);
            return View(state);
        }

        [HttpPost]
        [Route("/states/{stateId:int}")]
        public IActionResult Update(int stateId, State state)
        {
            state.Id = stateId;
            _context.States.Update(state);
            _context.SaveChanges();

            return Redirect($"/states/{stateId}");
        }
    }
}
