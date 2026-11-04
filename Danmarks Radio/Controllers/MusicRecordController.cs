using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Danmarks_Radio.Controllers
{
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
        [Authorize(Roles = "Admin")] // Kræver login (gyldig JWT token og "Admin" rolle)
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

        [Authorize(Roles = "Admin")]
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "User")]
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