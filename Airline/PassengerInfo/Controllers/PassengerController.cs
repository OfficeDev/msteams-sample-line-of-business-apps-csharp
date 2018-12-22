using Airline.PassengerInfo.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Airline.PassengerInfo.Web.Repository;
using System.Net;

namespace Airline.PassengerInfo.Web.Controllers
{
    public class PassengerController : Controller
    {
        // GET: Passenger
        public async Task<ActionResult> Index()
        {
            var passengers = await DocumentDBRepository<Passenger>.GetItemsAsync(d => d != null);
            return View(passengers);
        }

        // GET: Passenger/Create
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,From,To,Gender,Date,Seat,Class,FlightNumber,PNR,Notes,FrequentFlyerNumber,SpecialAssistance")] Passenger passenger)
        {
            if (ModelState.IsValid)
            {

                await DocumentDBRepository<Passenger>.CreateItemAsync(passenger);
                return RedirectToAction("Index");
            }

            return View(passenger);
        }

        // GET: /Passenger/Edit/12
        public async Task<ActionResult> Edit(string PNR)
        {
            if (PNR == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var passengers = await DocumentDBRepository<Passenger>.GetItemsAsync(d => d.PNR == PNR);
            Passenger passenger = passengers.FirstOrDefault();
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "From,To,Gender,Seat,Class,FlightNumber,PNR, Notes,FrequentFlyerNumber,SpecialAssistance")] Passenger passenger)
        {
            if (ModelState.IsValid)
            {
                var passengers = await DocumentDBRepository<Passenger>.UpdateItemAsync(passenger.PNR, passenger);
                return RedirectToAction("Index");
            }
            return View(passenger);
        }

        // GET: /Passenger/Delete/5
        public async Task<ActionResult> Delete(string PNR)
        {
            if (PNR == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var passengers = await DocumentDBRepository<Passenger>.GetItemsAsync(d => d.PNR == PNR);
            Passenger passenger = passengers.FirstOrDefault();
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }
        // POST: /Passenger/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> DeleteConfirmed(string PNR)
        {
            var passengers = await DocumentDBRepository<Passenger>.GetItemsAsync(d => d.PNR == PNR);
            Passenger passenger = passengers.FirstOrDefault();
            if (passenger == null)
            {
                return HttpNotFound();
            }
            await DocumentDBRepository<Passenger>.DeleteDocumentAsync(PNR);
            return RedirectToAction("Index");
        }
    }
}