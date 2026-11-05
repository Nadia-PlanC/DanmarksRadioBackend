using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Danmarks_Radio.Controllers
{
    [EnableCors("AllowAll")] // ADD CORS policy to the controller
    [Route("api/[controller]")]
    [ApiController]
    public class MusicRecordController : ControllerBase
    {
        private readonly IMusicListRepo _repository;

        public MusicRecordController(IMusicListRepo repository)
        {
            _repository = repository;
        }

        // GET: api/MusicRecord
        //[Authorize(Roles = "Admin")] // Kræver login (gyldig JWT token og "Admin" rolle)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<MusicRecord>> Get(
            [FromQuery] string? titleContains = null,
            [FromQuery] string? artist = null,
            [FromQuery] string? sortBy = null)
        {
            try
            {
                return Ok(_repository.GetAll(titleContains, artist, sortBy));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/MusicRecord/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MusicRecord> GetById(int id)
        {
            MusicRecord? record = _repository.GetRecordById(id);

            if (record == null)
            {
                return NotFound($"MusicRecord med id {id} blev ikke fundet.");
            }

            return Ok(record);
        }

        // POST api/<MusicRecordController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "User")]

        public ActionResult<MusicRecord> Post([FromBody] MusicRecord newRecord)
        {
            if (newRecord == null)
            {
                return BadRequest();
            }
            MusicRecord createdRecord = _repository.AddRecord(newRecord);
            return CreatedAtAction(nameof(Get), new { id = createdRecord.Id }, createdRecord);
        }

        // Delete api/<MusicRecordController>
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MusicRecord> Delete(int id)
        {
            MusicRecord? deletedRecord = _repository.RemoveRecord(id);

            if (deletedRecord == null)
            {
                return NotFound($"MusicRecord med id {id} blev ikke fundet.");
            }

            return Ok(deletedRecord);
        }

        // Put api/<MusicRecordController>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "User")]
        public ActionResult<MusicRecord> Put(int id, [FromBody] MusicRecord updates)
        {
            MusicRecord? updatedRecord = _repository.UpdateRecord(id, updates);
            if (updatedRecord == null)
            {
                return NotFound($"MusicRecord med id {id} blev ikke fundet.");
            }
            return Ok(updatedRecord);
        }


    }
}