using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.DoctorGUI;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Referrals.Services;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users.Services;
using HealthCareCenter.Core;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Referrals.Models;
using HealthCareCenter.Core.Referrals.Repositories;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Medicine.Repositories;
using HealthCareCenter.Core.Prescriptions;

namespace HealthCareCenter.GUI.Doctor.ViewModels
{
    public class DoctorWindowViewModel
    {
        public int selectedRoomID, selectedPatientID, selectedAppointmentIndex, comboBoxChangeCounter = 0;
        public HealthRecord selectedPatientsHealthRecord;
        private User _signedUser;
        private DoctorWindow window;
        private Referral referral;
        private Medicine chosenMedicine;
        private IReferralService _referralsService;
        private IRoomService _roomService;
        private BaseReferralRepository _referralRepository;
        private readonly BaseAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;

        public DoctorWindowViewModel(
            User signedUser, 
            IReferralService referralsService,
            BaseReferralRepository referralRepository,
            BaseAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService,
            IRoomService roomService,
            IHealthRecordService healthRecordService)
        {
            _referralsService = referralsService;
            _signedUser = signedUser;
            _referralRepository = referralRepository;
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            window = new DoctorWindow(
                signedUser, 
                this, 
                new NotificationService(
                    new NotificationRepository(),
                    new HealthRecordService(
                        new HealthRecordRepository()),
                    new MedicineInstructionService(
                        new MedicineInstructionRepository()),
                    new MedicineService(
                        new MedicineRepository())),
                new AppointmentRepository(),
                new HealthRecordRepository(),
                new MedicineRepository(),
                new MedicineInstructionRepository(),
                new PrescriptionService(
                    new MedicineInstructionRepository(),
                    new PrescriptionRepository()),
                new PrescriptionRepository());
            _roomService = roomService;
            _healthRecordService = healthRecordService;
            window.Show();
        }

        ~DoctorWindowViewModel()
        {
            _referralRepository.Save();
        }

        public void FillEquipmentTable()
        {
            Room room;
            try
            {
                room = _roomService.Get(selectedRoomID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Room not found");
                return;
            }
            window.equipmentDataTable.Rows.Clear();
            foreach (string name in room.EquipmentAmounts.Keys)
            {
                window.AddEquipmentToEquipmentTable(name);
            }
            window.equipmentDataGrid.ItemsSource = window.equipmentDataTable.DefaultView;
        }

        public void CreateAReferral()
        {
            referral = new Referral();
            referral.ID = ++_referralRepository.LargestID;
        }

        public void FillDoctorsTable(List<Core.Users.Models.Doctor> doctors)
        {
            window.doctorsDataTable.Rows.Clear();
            foreach (Core.Users.Models.Doctor doctor in doctors)
            {
                window.AddDoctorToDoctorsTable(doctor);
            }
            window.doctorsDataGrid.ItemsSource = window.doctorsDataTable.DefaultView;
        }

        public void FillDateTimeComboBoxes()
        {
            for (int i = 1; i <= 31; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.dayChoiceComboBox.Items.Add(s);
            }
            for (int i = 1; i <= 12; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.monthChoiceComboBox.Items.Add(s);
            }
            window.yearChoiceComboBox.Items.Add("2022");
            window.yearChoiceComboBox.Items.Add("2023");
            window.yearChoiceComboBox.Items.Add("2024");
        }

        public void FillAppointmentsTable(List<Appointment> appointments)
        {
            window.appointmentsDataTable.Rows.Clear();
            if (_appointmentRepository.Appointments == null)
            {
                appointments = _appointmentRepository.Load();
            }

            foreach (Appointment appointment in appointments)
            {
                if (appointment.DoctorID != _signedUser.ID)
                {
                    continue;
                }
                HealthRecord patientsHealthRecord = _healthRecordService.Get(appointment.HealthRecordID);
                if (patientsHealthRecord != null)
                    window.AddAppointmentToAppointmentsTable(appointment, patientsHealthRecord.PatientID);
                else
                    window.AddAppointmentToAppointmentsTable(appointment, -1);
            }
            window.scheduleDataGrid.ItemsSource = window.appointmentsDataTable.DefaultView;
        }

        public void SearchAppointments()
        {
            string day, month, year, unparsedDate;

            CheckDateComboBoxes();

            day = window.dayChoiceComboBox.SelectedItem.ToString();
            month = window.monthChoiceComboBox.SelectedItem.ToString();
            year = window.yearChoiceComboBox.SelectedItem.ToString();

            unparsedDate = day + "/" + month + "/" + year;
            DateTime date = DateTime.ParseExact(unparsedDate, Constants.DateFormat, CultureInfo.InvariantCulture);
            List<Appointment> appointmentsResult = _appointmentService.GetAppointmentsInTheFollowingDays(date, 3);
            FillAppointmentsTable(appointmentsResult);
        }

        public bool FindRoomID()
        {
            int patientID, roomID, appointmentIndex;
            patientID = TableService.GetRowItemID(window.scheduleDataGrid, "Patient ID");
            roomID = TableService.GetRowItemID(window.scheduleDataGrid, "Room ID");
            appointmentIndex = TableService.GetSelectedIndex(window.scheduleDataGrid);
            if (patientID == -1 || roomID == -1 || appointmentIndex == -1)
                return false;
            selectedRoomID = roomID;
            selectedPatientID = patientID;
            selectedAppointmentIndex = appointmentIndex;
            return true;
        }

        public void CreateAnamnessis()
        {
            string anamnesisComment = window.anamnesisTextBox.Text;
            Anamnesis anamnesis = new Anamnesis();
            anamnesis.Comment = anamnesisComment;
            anamnesis.ID = selectedAppointmentIndex;
            _appointmentRepository.Appointments[selectedAppointmentIndex].PatientAnamnesis = anamnesis;
            window.anamnesisLabel.Content = anamnesisComment;
        }

        public bool FillReferralWithData()
        {
            int doctorID = TableService.GetRowItemID(window.doctorsDataGrid, "Id");
            if (doctorID == -1)
                return false;
            _referralsService.Fill(doctorID, selectedPatientID, referral);
            _referralRepository.Referrals.Add(referral);
            return true;
        }

        public bool FillAutomaticReferralWithData()
        {
            int doctorID = TableService.GetRowItemID(window.doctorsDataGrid, "Id");
            if (doctorID == -1)
                return false;
            _referralsService.Fill(doctorID, selectedPatientID, referral);
            _referralRepository.Referrals.Add(referral);
            return true;
        }

        private void CheckDateComboBoxes()
        {
            if (window.dayChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a day");
                return;
            }
            if (window.monthChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a month");
                return;
            }
            if (window.yearChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a year");
                return;
            }
        }

        public void UpdateEquipment(int selectedRoomID)
        {
            string equipmentName = TableService.GetRowItem(window.equipmentDataGrid, "Name");
            if (equipmentName == "")
                return;
            Room room = _roomService.Get(selectedRoomID);
            int amount = _roomService.GetEquipmentAmount(room, equipmentName);
            window.equipmentTextBlock.Text = amount.ToString();
        }

        public void SubmitEquipmentChanges(int selectedRoomID)
        {
            string equipmentName, unparsedAmount;
            int usedAmount;
            equipmentName = TableService.GetRowItem(window.equipmentDataGrid, "Name");
            unparsedAmount = window.equipmentTextBox.Text;
            usedAmount = CheckTheAmount(unparsedAmount, equipmentName, selectedRoomID);
            if (usedAmount == -1) return;
            Room room = _roomService.Get(selectedRoomID);
            room.EquipmentAmounts[equipmentName] -= usedAmount;
            MessageBox.Show(equipmentName + " sucessfully updated");
            window.equipmentTextBox.Text = "";
        }

        private int CheckTheAmount(string unparsedAmount, string equipmentName, int selectedRoomID)
        {
            int usedAmount = -1, oldAmount;
            Room room;
            try
            {
                usedAmount = int.Parse(unparsedAmount);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid number");
                return -1;
            }
            room = _roomService.Get(selectedRoomID);
            oldAmount = _roomService.GetEquipmentAmount(room, equipmentName);
            if (oldAmount < usedAmount)
            {
                MessageBox.Show("The number is too big");
                return -1;
            }
            return usedAmount;
        }

        public List<Core.Users.Models.Doctor> GetDoctorsByType()
        {
            int selectedIndex;
            string chosenType = "";
            if (comboBoxChangeCounter == 0)
            {
                comboBoxChangeCounter++;
                return null;
            }
            selectedIndex = window.doctorTypeComboBox.SelectedIndex;
            switch (selectedIndex)
            {
                case 0: chosenType = "General practitioner"; window.specializationComboBox.IsEnabled = false; window.submitAutomaticReferal.IsEnabled = false; break;
                case 1: chosenType = "Special"; window.specializationComboBox.IsEnabled = true; return GetDoctorsBySpecialization();
            }
            return DoctorService.GetDoctorsOfType(chosenType);
        }

        public List<Core.Users.Models.Doctor> GetDoctorsBySpecialization()
        {
            int selectedIndex = window.specializationComboBox.SelectedIndex;
            string chosenType = "";
            if (selectedIndex == -1)
                return null;
            switch (selectedIndex)
            {
                case 0: chosenType = "Neurologist"; break;
                case 1: chosenType = "Cardiologist"; break;
                default: return null;
            }
            return DoctorService.GetDoctorsOfType(chosenType);
        }
    }
}