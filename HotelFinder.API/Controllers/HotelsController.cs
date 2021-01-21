using HotelFinder.Business.Abstract;
using HotelFinder.Business.Concrete;
using HotelFinder.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IHotelService _hotelService;
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }


        /// <summary>
        /// Get All Hotels
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var hotels= _hotelService.GetAllHotels();
            return Ok(hotels);//Response kod olarak 200 dondurur
        }
        /// <summary>
        /// Get Hotel Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
      

        [HttpGet]
        [Route("[action]/{id}")]//   api/hotels/gethotelbyid/2
        public IActionResult GetHotelById(int id)
        {
                 var hotel = _hotelService.GetHotelById(id);
                   if (hotel != null)
                   {
                       return Ok(hotel);//200 +data
                   }
                   return NotFound();//404
             
        }
        [HttpGet]
        [Route("[action]/{name}")]
        public IActionResult GetHotelByName(string name)
        {
            var hotel = _hotelService.GetHotelByName(name);
            if (hotel != null)
            {
                return Ok(hotel);//200 +data
            }
            return NotFound();
        }

        [HttpGet]
        [Route("[action]/{id}/{name}")]
        public IActionResult GetHotelByIdAndName(int id ,string name)
        {
            return Ok();
        }





        /// <summary>
        /// Create Hotel
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateHotel([FromBody]Hotel hotel)
        {
                var createdHotel = _hotelService.CreateHotel(hotel);
                return CreatedAtAction("Get", new { id = createdHotel.Id }, createdHotel);//201 +data
            

            return BadRequest(ModelState);//Validation error +400
        }
        /// <summary>
        /// Update a Hotel
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("action")]
        public IActionResult UpdateHotel([FromBody] Hotel hotel)
        {
            if (_hotelService.GetHotelById(hotel.Id) != null)
            {
                return Ok(_hotelService.UpdateHotel(hotel)); //200 +data
            }
            return NotFound();
        }
        /// <summary>
        /// Delete a Hotel
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("[aciton]/{id}")]
        public IActionResult DeleteHotel(int id)
        {
            if (_hotelService.GetHotelById(id) != null)
            {
                _hotelService.DeleteHotel(id);
                return Ok(); //200
            }
            return NotFound();
        }
    }
}
