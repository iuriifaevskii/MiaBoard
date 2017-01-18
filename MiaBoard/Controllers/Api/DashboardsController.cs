using AutoMapper;
using MiaBoard.Dtos;
using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiaBoard.Controllers.Api
{
    public class DashboardsController : ApiController
    {
        private ApplicationDbContext _context;

        public DashboardsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/Dashboards
        public IHttpActionResult GetDashboards()
        {
            var dashboardDtos = _context.Dashboards
                .ToList()
                .Select(Mapper.Map<Dashboard, DashboardDto>);

            return Ok(dashboardDtos);
        }

        // GET /api/Dashboards/1
        public IHttpActionResult GetDashboard(int id)
        {
            var dashboard = _context.Dashboards.SingleOrDefault(c => c.Id == id);

            if (dashboard == null)
                return NotFound();

            return Ok(Mapper.Map<Dashboard, DashboardDto>(dashboard));
        }

        // POST /api/Dashboards
        [HttpPost]
        public IHttpActionResult CreateDashboard(DashboardDto dashboardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dashboard = Mapper.Map<DashboardDto, Dashboard>(dashboardDto);
            _context.Dashboards.Add(dashboard);
            _context.SaveChanges();

            dashboardDto.Id = dashboard.Id;
            return Created(new Uri(Request.RequestUri + "/" + dashboard.Id), dashboardDto);
        }

        // PUT /api/Dashboards/1
        [HttpPut]
        public IHttpActionResult UpdateDashboard(int id, DashboardDto dashboardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dashboardInDb = _context.Dashboards.SingleOrDefault(c => c.Id == id);

            if (dashboardInDb == null)
                return NotFound();

            Mapper.Map(dashboardDto, dashboardInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE /api/Dashboards/1
        [HttpDelete]
        public IHttpActionResult DeleteDashboard(int id)
        {
            var dashboardInDb = _context.Dashboards.SingleOrDefault(c => c.Id == id);


            if (dashboardInDb == null)
                return NotFound();

            _context.Dashboards.Remove(dashboardInDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}
