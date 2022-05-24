using Hospital_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital_Management.Repository
{
    public class PatientRepository : IPatientRepository
    {
        ApplicationDbContext context;
        public PatientRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public int Delete(int id)
        {
            try
            {

                Patient patient = GetById(id);
                context.Patients.Remove(patient);
                return context.SaveChanges();
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public List<Patient> GetAll()
        {
            return context.Patients.ToList();
        }

        public Patient GetById(int id)
        {
            return context.Patients.FirstOrDefault(p => p.ID == id);
        }

        public int Insert(Patient entity)
        {
            context.Patients.Add(entity);
            return context.SaveChanges();
        }

        public int Update(int id, Patient entity)
        {
            Patient oldPatient = GetById(id);
            if (oldPatient != null)
            {
                oldPatient.Name = entity.Name;
                oldPatient.Address = entity.Address;
                oldPatient.Age = entity.Age;
                oldPatient.Appointments = entity.Appointments;
                return context.SaveChanges();
            }
            return 0;
        }
    }
}
