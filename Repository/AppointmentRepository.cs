using Hospital_Management.DTO;
using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital_Management.Repository
{
    public class AppointmentRepository :IAppointmentRepository
    {
        ApplicationDbContext context;
        public AppointmentRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public int CancleAppointment(int patientId, Status status )
        {
            var query = context.Appointments.Where(a => a.PatientID == patientId).ToList();
            if(status == Status.cancel)
            {
                foreach (var item in query)
                {
                    if (item.Time.Day != DateTime.Now.Day)
                    {
                        context.Remove(item);
                        return context.SaveChanges();
                    }
                }
            }
            return 0;

        }

        public int Delete(int patientId,int appointmentId)
        {
            var query =  context.Appointments.FirstOrDefault(a => a.PatientID == patientId && a.ID == appointmentId);
            if(query != null)
            {
                context.Remove(query);
                return context.SaveChanges();
            }
            return 0;
        }

        public List<Appointment> GetAllByPatientId(int patientId)
        {
            return context.Appointments.Where(a => a.PatientID == patientId).ToList();
        }

        public int Insert(Appointment entity)
        {
            if (entity != null)
            {
                context.Add(entity);
                return context.SaveChanges();
            }
            return 0;
        }
    }
}
