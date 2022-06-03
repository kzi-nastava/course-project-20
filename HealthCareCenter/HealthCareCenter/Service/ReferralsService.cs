using HealthCareCenter.Model;
using HealthCareCenter.Secretary;
using System.Collections.Generic;

namespace HealthCareCenter.Service
{
    public static class ReferralsService
    {
        public static List<PatientReferral> Get(Patient patient)
        {
            List<PatientReferral> referrals = new List<PatientReferral>();
            foreach (Referral referral in ReferralRepository.Referrals)
            {
                if (referral.PatientID != patient.ID)
                {
                    continue;
                }

                Add(referral, referrals);
            }
            return referrals;
        }

        private static void Add(Referral referral, List<PatientReferral> referrals)
        {
            PatientReferral patientReferral = new PatientReferral(referral.ID);

            LinkDoctor(referral, patientReferral);
            referrals.Add(patientReferral);
        }

        private static void LinkDoctor(Referral referral, PatientReferral patientReferral)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID != referral.DoctorID)
                {
                    continue;
                }
                patientReferral.DoctorUsername = doctor.Username;
                patientReferral.DoctorFirstName = doctor.FirstName;
                patientReferral.DoctorLastName = doctor.LastName;
                return;
            }
        }

        public static Referral Find(int referralID)
        {
            foreach (Referral referral in ReferralRepository.Referrals)
            {
                if (referral.ID == referralID)
                {
                    return referral;
                }
            }
            return null;
        }

        public static void Schedule(Referral referral, Appointment appointment)
        {
            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(appointment.HospitalRoomID, appointment);
            HospitalRoomRepository.Save();

            ReferralRepository.Referrals.Remove(referral);
            ReferralRepository.Save();
        }
    }
}
