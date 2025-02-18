using API.ViewModals;
using Core.Entities;
using Core.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualAppointmentController : ControllerBase
    {
        private readonly IVirtualAppointmentRepository _virtualAppointmentRepository;
        public VirtualAppointmentController(IVirtualAppointmentRepository virtualAppointmentRepository)
        {
            _virtualAppointmentRepository = virtualAppointmentRepository;
        }

        [HttpGet("VirtualAppoimentList")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetVirtualAppoimentList()
        {
            var response = await _virtualAppointmentRepository.GetVirtualAppointmentListAsync();
            return Ok(response);
        }

        [HttpGet("AppointmentById/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetVirtualAppoiment(int id)
        {
            var response = await _virtualAppointmentRepository.GetVirtualAppointmentAsync(id);
            return Ok(response);
        }

        [HttpPost("SaveVirtualAppointment")]
        public async Task<ActionResult<ApiResponse<VirtualAppointment>>> SaveVirtualAppointment([FromBody] VirtualAppointment virtualAppointment)
        {
            // Check if the virtualAppointment is null
            if (virtualAppointment == null)
            {
                return BadRequest(new ApiResponse<VirtualAppointment>
                {
                    Success = false,
                    Message = "Appointment data cannot be null."
                });
            }

            // Validate the virtualAppointment if necessary (e.g., check required fields)
            if (string.IsNullOrWhiteSpace(virtualAppointment.FirstName) || virtualAppointment.FirstName == null)
            {
                return BadRequest(new ApiResponse<VirtualAppointment>
                {
                    Success = false,
                    Message = "Appointment name and date are required."
                });
            }

            try
            {
                if (virtualAppointment.Status == null)
                {
                    virtualAppointment.Status = "Requested";
                }
                // Save the virtualAppointment to the database
                await _virtualAppointmentRepository.SaveVirtualAppointment(virtualAppointment);

                // Assuming AddAsync method returns the entity after it's saved
                // You can also return the saved entity
                return Ok(new ApiResponse<VirtualAppointment>
                {
                    Success = true,
                    Message = "Virtual appointment saved successfully.",
                    Data = virtualAppointment
                });
            }
            catch (Exception ex)
            {
                // Log exception and return error response
                return StatusCode(500, new ApiResponse<VirtualAppointment>
                {
                    Success = false,
                    Message = $"An error occurred while saving the appointment: {ex.Message}"
                });
            }
        }



        [HttpDelete("VirtualAppointment/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<VirtualAppointmentVM>>> DeleteVirtualAppointment(int id)
        {
            // Step 1: Retrieve the appointment by ID
            var appointment = await _virtualAppointmentRepository.GetVirtualAppointmentAsync(id);

            // Step 2: If the appointment doesn't exist, return NotFound
            if (appointment == null)
            {
                return NotFound(new { message = "Virtual appointment not found." });
            }

            // Step 3: Delete the appointment
            var result = await _virtualAppointmentRepository.DeleteVirtualAppointmentAsync(id);

            if (result)
            {
                return Ok(new ApiResponse<VirtualAppointmentVM> { Success = true, Message = "Virtual Appointment deleted successfully" });
            }
            else
            {
                return BadRequest(new { message = "There was an issue deleting the virtual appointment." });
            }
        }


    }
}
