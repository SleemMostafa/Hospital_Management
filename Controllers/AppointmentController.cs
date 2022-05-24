using Hospital_Management.DTO;
using Hospital_Management.Models;
using Hospital_Management.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Hospital_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IPatientRepository  patientRepository;

        public AppointmentController(IAppointmentRepository _appointmentRepository, IPatientRepository _patientRepository)
        {
            this.appointmentRepository = _appointmentRepository;
            this.patientRepository = _patientRepository;
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int patientId)
        {
            List<Appointment> appointments = appointmentRepository.GetAllByPatientId(patientId);
            if (appointments != null)
            {
                return Ok(appointments);
            }
            return BadRequest();
        }
       [HttpPost]
        public IActionResult Create(AppointmentModel model)
        {
            if(ModelState.IsValid)
            {
                var patient = patientRepository.GetById(model.PatientID);
                if (patient != null)
                {
                    Appointment appointment = new Appointment();
                    appointment.Time = model.Time;
                    appointment.PatientID = model.PatientID;
                    appointmentRepository.Insert(appointment);
                    return Ok(appointment);
                }
                return NotFound("Not Found!");
            }
            return BadRequest();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult Delete(int patientId, int appointmentId)
        {
            if (ModelState.IsValid)
            {
                int patient = appointmentRepository.Delete(patientId,appointmentId);
                if(patient == 1)
                {
                    return Ok("Appointement Deleted");
                }
                
                return NotFound("Not Found!");
            }
            return BadRequest();
        }
        [HttpPost("cancelAppointment")]
        public IActionResult Cancel(int patientId, Status status)
        {
            if(ModelState.IsValid)
            {
                int roweffected = appointmentRepository.CancleAppointment(patientId, Status.cancel);
                if(roweffected == 1)
                {
                    return Ok("Appointment Cancel");
                }
                else if(roweffected == 0)
                {
                    return BadRequest("Not Allow Cancel");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
