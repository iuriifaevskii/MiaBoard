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
    public class DataSourcesController : ApiController
    {
        private ApplicationDbContext _context;

        public DataSourcesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/DataSources
        public IHttpActionResult GetDataSources()
        {
            var dataSourceDtos = _context.DataSources
                .ToList()
                .Select(Mapper.Map<DataSource, DataSourceDto>);

            return Ok(dataSourceDtos);
        }

        // GET /api/DataSources/1
        public IHttpActionResult GetDataSource(int id)
        {
            var dataSource = _context.DataSources.SingleOrDefault(c => c.Id == id);

            if (dataSource == null)
                return NotFound();

            return Ok(Mapper.Map<DataSource, DataSourceDto>(dataSource));
        }

        // POST /api/DataSources
        [HttpPost]
        public IHttpActionResult CreateDataSource(DataSourceDto dataSourceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dataSource = Mapper.Map<DataSourceDto, DataSource>(dataSourceDto);
            _context.DataSources.Add(dataSource);
            _context.SaveChanges();

            dataSourceDto.Id = dataSource.Id;
            return Created(new Uri(Request.RequestUri + "/" + dataSource.Id), dataSourceDto);
        }

        // PUT /api/DataSources/1
        [HttpPut]
        public IHttpActionResult UpdateDataSource(int id, DataSourceDto dataSourceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dataSourceInDb = _context.DataSources.SingleOrDefault(c => c.Id == id);

            if (dataSourceInDb == null)
                return NotFound();

            Mapper.Map(dataSourceDto, dataSourceInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE /api/DataSources/1
        [HttpDelete]
        public IHttpActionResult DeleteDataSource(int id)
        {
            var dataSourceInDb = _context.DataSources.SingleOrDefault(c => c.Id == id);


            if (dataSourceInDb == null)
                return NotFound();

            _context.DataSources.Remove(dataSourceInDb);
            _context.SaveChanges();

            return Ok();
        }
        
    }
}
