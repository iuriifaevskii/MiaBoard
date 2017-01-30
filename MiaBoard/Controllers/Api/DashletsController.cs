using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MiaBoard.Models;

namespace MiaBoard.Controllers.Api
{
    public class DashletsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Dashlets
        public IQueryable<Dashlet> GetDashlets()
        {
            return db.Dashlets;
        }

        // GET: api/Dashlets/5
        [ResponseType(typeof(Dashlet))]
        public IHttpActionResult GetDashlet(int id)
        {
            Dashlet dashlet = db.Dashlets.Include(d => d.DataSource).SingleOrDefault(X => X.Id == id);

            if (dashlet == null)
            {
                return NotFound();
            }

            return Ok(dashlet);
        }

        [Route("api/dashlets/createviadrag")]
        [HttpPost]
        public IHttpActionResult Action(Dashlet dashlet)
        {
            if (dashlet == null)
                return BadRequest();

            try {
                dashlet.Dashboard = db.Dashboards.SingleOrDefault(ds => ds.Id == dashlet.DashboardId);
                dashlet.DataSource = db.DataSources.SingleOrDefault(ds => ds.Id == dashlet.DataSourceId);

                db.Dashlets.Add(dashlet);
                db.SaveChanges();
            } catch(Exception e){
                return InternalServerError(e);
            }

            return Ok();
        }

        [Route("api/dashlets/lastid")]
        [HttpGet]
        public int GetLastId()
        {
            int lastId = db.Dashlets.Max(d => d.Id);
            return lastId;
        }

        [Route("api/dashlets/position/{id}/{column}/{position}/{dashboardid}")]
        [HttpPost]
        public IHttpActionResult PostDashletList( int id = 0, int column = 0 , int position = 0, int dashboardid = 0)
        {
            if(id == 0 || column == 0)
                return BadRequest();

            var dashletInDb = db.Dashlets.SingleOrDefault(d => d.Id == id);

            if (dashletInDb == null)
            {
                var dashlet = new Dashlet();
                dashlet.Column = column;
                dashlet.Position = position;
                dashlet.DataSource = db.DataSources.Where(d => d.Id == 4).SingleOrDefault();
                dashlet.Dashboard = db.Dashboards.Where(d => d.Id == dashboardid).SingleOrDefault(); 
                try
                {
                    db.Dashlets.Add(dashlet);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
            else
            {
                try
                {
                    dashletInDb.Position = position;
                    dashletInDb.Column = column;

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            return Ok();
        }

        // POST: api/Dashlets
        [ResponseType(typeof(Dashlet))]
        public IHttpActionResult PostDashlet(Dashlet dashlet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dashlets.Add(dashlet);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dashlet.Id }, dashlet);
        }

        // DELETE: api/Dashlets/5
        [ResponseType(typeof(Dashlet))]
        public IHttpActionResult DeleteDashlet(int id)
        {
            Dashlet dashlet = db.Dashlets.Find(id);
            if (dashlet == null)
            {
                return NotFound();
            }

            db.Dashlets.Remove(dashlet);
            db.SaveChanges();

            return Ok(dashlet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DashletExists(int id)
        {
            return db.Dashlets.Count(e => e.Id == id) > 0;
        }
    }
}