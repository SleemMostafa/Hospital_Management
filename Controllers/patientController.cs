using Hospital_Management.DTO;
using Hospital_Management.Models;
using Hospital_Management.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hospital_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class patientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;

        public patientController(IPatientRepository _patientRepository)
        {
            patientRepository = _patientRepository;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Patient> patients = patientRepository.GetAll();
            return Ok(patients);
        }
        [HttpGet("{id:int}", Name = "getOneRoute")]
        public IActionResult GetById(int productId)
        {
            Patient patient = patientRepository.GetById(productId);
            if (patient != null)
            {
                return Ok(patient);
            }
            return BadRequest();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(PatientModel model)
        {
            Patient newPatient = new Patient();
            if (ModelState.IsValid)
            {
                try
                {
                    newPatient.Name = model.Name;
                    newPatient.Age = model.Age;
                    newPatient.Address = model.Address;
                    var query = patientRepository.Insert(newPatient);
                    string url = Url.Link("getOneRoute", new { id = newPatient.ID });
                    return Created(url, newPatient);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}")]
        public IActionResult Edit(int patientId, Patient newPatient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    patientRepository.Update(patientId, newPatient);
                    return Ok(newPatient);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            int patient = patientRepository.Delete(id);
            if (patient == 1)
            {
                return Accepted("Patient Deleted");
            }
            return BadRequest("Not Found");
        }
    }
}
