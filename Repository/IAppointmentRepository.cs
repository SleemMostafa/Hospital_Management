using Hospital_Management.DTO;
using Hospital_Management.Models;
using System.Collections.Generic;

namespace Hospital_Management.Repository
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAllByPatientId(int patientId);
        int Insert(Appointment entity);
        int Delete(int patientId,int appointmentId);
        int CancleAppointment(int patientId , Status status);
    }
}
