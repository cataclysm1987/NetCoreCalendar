using DotNetCoreCalendar.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace DotNetCoreCalendar.Data
{
    public interface IDAL
    {
        public List<Event> GetEvents();
        public List<Event> GetMyEvents(string userid);
        public Event GetEvent(int id);
        public void CreateEvent(IFormCollection form);
        public void UpdateEvent(IFormCollection form);
        public void DeleteEvent(int id);
        public List<Location> GetLocations();
        public Location GetLocation(int id);
        public void CreateLocation(Location location);
    }

    public class DAL : IDAL
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Event> GetEvents()
        {
            return db.Events.ToList();
        }

        public List<Event> GetMyEvents(string userid)
        {
            return db.Events.Where(x => x.User.Id == userid).ToList();
        }

        public Event GetEvent(int id)
        {
            return db.Events.FirstOrDefault(x => x.Id == id);
        }

        public void CreateEvent(IFormCollection form)
        {
            var locname = form["Location"].ToString();
            var user = db.Users.FirstOrDefault(x => x.Id == form["UserId"].ToString());
            var newevent = new Event(form, db.Locations.FirstOrDefault(x => x.Name == locname), user);
            db.Events.Add(newevent);
            db.SaveChanges();
        }

        public void UpdateEvent(IFormCollection form)
        {
            var locname = form["Location"].ToString();
            var eventid = int.Parse(form["Event.Id"]);
            var myevent = db.Events.FirstOrDefault(x => x.Id == eventid);
            var location = db.Locations.FirstOrDefault(x => x.Name == locname);
            var user = db.Users.FirstOrDefault(x => x.Id == form["UserId"].ToString());
            myevent.UpdateEvent(form, location, user);
            db.Entry(myevent).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteEvent(int id)
        {
            var myevent = db.Events.Find(id);
            db.Events.Remove(myevent);
            db.SaveChanges();
        }

        public List<Location> GetLocations()
        {
            return db.Locations.ToList();
        }

        public Location GetLocation(int id)
        {
            return db.Locations.Find(id);
        }

        public void CreateLocation(Location location)
        {
            db.Locations.Add(location);
            db.SaveChanges();
        }
    }
}
