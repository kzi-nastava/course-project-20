using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Users.Models;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments.Services
{
    public class AppointmentChangeRequestService : IAppointmentChangeRequestService
    {
        private readonly BaseAppointmentRepository _appointmentRepository;
        private readonly BaseAppointmentChangeRequestRepository _changeRequestRepository;

        public AppointmentChangeRequestService(
            BaseAppointmentRepository appointmentRepository, 
            BaseAppointmentChangeRequestRepository changeRequestRepository)
        {
            _appointmentRepository = appointmentRepository;
            _changeRequestRepository = changeRequestRepository;
        }

        public void DeleteAppointment(AppointmentChangeRequest request)
        {
            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.ID == request.AppointmentID)
                {
                    HospitalRoomService.Get(appointment.HospitalRoomID).AppointmentIDs.Remove(appointment.ID);
                    _appointmentRepository.Appointments.Remove(appointment);
                    _appointmentRepository.Save();
                    break;
                }
            }
        }

        public void EditAppointment(AppointmentChangeRequest request)
        {
            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.ID == request.AppointmentID)
                {
                    appointment.ScheduledDate = request.NewDate;
                    appointment.Type = request.NewAppointmentType;
                    appointment.DoctorID = request.NewDoctorID;
                    _appointmentRepository.Save();
                    break;
                }
            }
        }

        public void Refresh(List<DeleteRequestForDisplay> deleteRequests, List<EditRequestForDisplay> editRequests, Patient patient)
        {
            foreach (AppointmentChangeRequest request in _changeRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting || request.PatientID != patient.ID)
                {
                    continue;
                }
                if (request.RequestType == RequestType.Delete)
                {
                    AddDeleteRequest(request, deleteRequests);
                }
                else
                {
                    AddEditRequest(request, editRequests);
                }
            }
        }

        private void AddEditRequest(AppointmentChangeRequest request, List<EditRequestForDisplay> editRequests)
        {
            EditRequestForDisplay editRequest = new EditRequestForDisplay(request.ID, request.DateSent);

            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.ID != request.AppointmentID)
                {
                    continue;
                }

                LinkDoctor(request, editRequest, appointment);

                editRequest.OriginalAppointmentTime = appointment.ScheduledDate;
                editRequest.OriginalType = appointment.Type;
                break;
            }
            editRequest.NewAppointmentTime = request.NewDate;
            editRequest.NewType = request.NewAppointmentType;
            editRequests.Add(editRequest);
        }

        private void LinkDoctor(AppointmentChangeRequest request, EditRequestForDisplay editRequest, Appointment appointment)
        {
            bool foundOld = false;
            bool foundNew = false;
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == appointment.DoctorID)
                {
                    editRequest.OriginalDoctorUsername = doctor.Username;
                    foundOld = true;
                }
                if (doctor.ID == request.NewDoctorID)
                {
                    editRequest.NewDoctorUsername = doctor.Username;
                    foundNew = true;
                }
                if (foundNew && foundOld)
                {
                    return;
                }
            }
        }

        private void AddDeleteRequest(AppointmentChangeRequest request, List<DeleteRequestForDisplay> deleteRequests)
        {
            DeleteRequestForDisplay deleteRequest = new DeleteRequestForDisplay(request.ID, request.DateSent);

            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.ID != request.AppointmentID)
                {
                    continue;
                }

                LinkDoctor(deleteRequest, appointment);
                deleteRequest.AppointmentTime = appointment.ScheduledDate;
                break;
            }
            deleteRequests.Add(deleteRequest);
        }

        private void LinkDoctor(DeleteRequestForDisplay deleteRequest, Appointment appointment)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == appointment.DoctorID)
                {
                    deleteRequest.DoctorUsername = doctor.Username;
                    return;
                }
            }
        }

        public void RejectEditRequest(int requestID)
        {
            Get(requestID).State = RequestState.Denied;
            _changeRequestRepository.Save();
        }

        public void AcceptEditRequest(int requestID)
        {
            AppointmentChangeRequest request = Get(requestID);
            request.State = RequestState.Approved;
            EditAppointment(request);

            _appointmentRepository.Save();
            _changeRequestRepository.Save();
        }

        public void RejectDeleteRequest(int requestID)
        {
            Get(requestID).State = RequestState.Denied;
            _changeRequestRepository.Save();
        }

        public void AcceptDeleteRequest(int requestID)
        {
            AppointmentChangeRequest request = Get(requestID);
            request.State = RequestState.Approved;
            DeleteAppointment(request);

            _appointmentRepository.Save();
            _changeRequestRepository.Save();
        }

        private AppointmentChangeRequest Get(int requestID)
        {
            foreach (AppointmentChangeRequest request in _changeRequestRepository.Requests)
            {
                if (request.ID == requestID)
                {
                    return request;
                }
            }
            return null;
        }
    }
}
