using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCareCenter.Model;
using System.Data;
using HealthCareCenter.Enums;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.Service;
using HealthCareCenter.DoctorServices;

namespace HealthCareCenter.DoctorGUI
{
    public partial class DoctorWindow : Window
    {
        private DoctorWindowService scheduleWindowService;
        private MedicineCreationRequestWindowService medicineCreationWindowService;
        private HealthRecordWindowService healthRecordWindowService;

        private Doctor signedUser;

        public DataTable appointmentsDataTable;
        public DataTable patientsDataTable;
        public DataTable doctorsDataTable;
        public DataTable medicineDataTable;
        public DataTable selectedMedicineDataTable;
        public DataTable medicineCreationRequestDataTable;
        public DataTable equipmentDataTable;

        DataRow dr;
        public DoctorWindow(Model.User user,DoctorWindowService windowService)
        {
            healthRecordWindowService = new HealthRecordWindowService(windowService,user,this);
            this.scheduleWindowService = windowService;
            signedUser = (Doctor)user;
            PrescriptionService.Initialise(signedUser.ID);
            CreateAppointmentTable();
            CreatePatientsTable();
            CreateDoctorsTable();
            CreateMedicineTable();
            CreateMedicineCreationRequestTable();
            CreateEquipmentTable();
            HealthRecordRepository.Load();
            AppointmentRepository.Load();
            MedicineRepository.Load();
            PrescriptionRepository.Load();  
            MedicineInstructionRepository.Load();
            ReferralRepository.Load();
            MedicineCreationRequestRepository.Load();
            InitializeComponent();

            DisplayNotifications();
        }

        private void DisplayNotifications()
        {
            List<Notification> notifications = NotificationService.FindUnopened(signedUser);    
            if (notifications.Count == 0)
            {
                return;
            }
            MessageBox.Show("You have new notifications.");
            foreach (Notification notification in notifications)
            {
                MessageBox.Show(notification.Message);
            }
        }

        private void CreateMedicineTable()
        {
            medicineDataTable = new DataTable("Medicines");
            selectedMedicineDataTable = new DataTable("Selected medicines");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("Name", typeof(string));
            DataColumn dc3 = new DataColumn("Creation date", typeof(string));
            DataColumn dc4 = new DataColumn("Expiration date", typeof(string));
            DataColumn dc5 = new DataColumn("Ingredients", typeof(List<string>));
            DataColumn dc6 = new DataColumn("Manufactrer", typeof(string));
            medicineDataTable.Columns.Add(dc1);
            medicineDataTable.Columns.Add(dc2);
            medicineDataTable.Columns.Add(dc3);
            medicineDataTable.Columns.Add(dc4);
            medicineDataTable.Columns.Add(dc5);
            medicineDataTable.Columns.Add(dc6);

            dc1 = new DataColumn("Id", typeof(int));
            dc2 = new DataColumn("Name", typeof(string));
            dc3 = new DataColumn("Creation date", typeof(string));
            dc4 = new DataColumn("Expiration date", typeof(string));
            dc5 = new DataColumn("Ingredients", typeof(List<string>));
            dc6 = new DataColumn("Manufactrer", typeof(string));
            selectedMedicineDataTable.Columns.Add(dc1);
            selectedMedicineDataTable.Columns.Add(dc2);
            selectedMedicineDataTable.Columns.Add(dc3);
            selectedMedicineDataTable.Columns.Add(dc4);
            selectedMedicineDataTable.Columns.Add(dc5);
            selectedMedicineDataTable.Columns.Add(dc6);
        }

        //Creating tables(table headers)
        private void CreateAppointmentTable()
        {
            appointmentsDataTable = new DataTable("Appointments");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("Type of appointment", typeof(string));
            DataColumn dc3 = new DataColumn("Appointment date", typeof(string));
            DataColumn dc4 = new DataColumn("Creation date", typeof(string));
            DataColumn dc5 = new DataColumn("Emergency", typeof(bool));
            DataColumn dc6 = new DataColumn("Doctors first and last name", typeof(string));
            DataColumn dc7 = new DataColumn("Room ID", typeof(int));
            DataColumn dc8 = new DataColumn("Patient ID", typeof(int));
            appointmentsDataTable.Columns.Add(dc1);
            appointmentsDataTable.Columns.Add(dc2);
            appointmentsDataTable.Columns.Add(dc3);
            appointmentsDataTable.Columns.Add(dc4);
            appointmentsDataTable.Columns.Add(dc5);
            appointmentsDataTable.Columns.Add(dc6);
            appointmentsDataTable.Columns.Add(dc7);
            appointmentsDataTable.Columns.Add(dc8);
        }
        private void CreatePatientsTable()
        {
            patientsDataTable = new DataTable("Patients");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("First name", typeof(string));
            DataColumn dc3 = new DataColumn("Last name", typeof(string));
            patientsDataTable.Columns.Add(dc1);
            patientsDataTable.Columns.Add(dc2);
            patientsDataTable.Columns.Add(dc3);
        }
        private void CreateDoctorsTable()
        {
            doctorsDataTable = new DataTable("Doctors");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("First name", typeof(string));
            DataColumn dc3 = new DataColumn("Last name", typeof(string));
            doctorsDataTable.Columns.Add(dc1);
            doctorsDataTable.Columns.Add(dc2);
            doctorsDataTable.Columns.Add(dc3);
        }
        private void CreateEquipmentTable()
        {
            equipmentDataTable = new DataTable("Equipment");
            DataColumn dc1 = new DataColumn("Name",typeof(string));
            equipmentDataTable.Columns.Add(dc1);
        }
        private void CreateMedicineCreationRequestTable()
        {
            medicineCreationRequestDataTable = new DataTable("Requests");
            DataColumn dc1 = new DataColumn("Id",typeof(int));
            DataColumn dc2 = new DataColumn("Name", typeof(string));
            DataColumn dc3 = new DataColumn("Manufacturer", typeof(string));
            medicineCreationRequestDataTable.Columns.Add(dc1);
            medicineCreationRequestDataTable.Columns.Add(dc2);
            medicineCreationRequestDataTable.Columns.Add(dc3);
        }


        //---------------------------------------------------------------------------------------
        //Putting data into tables and combo boxess


        public void AddMedicineToMedicineTable(Medicine medicine,DataTable table)
        {
            dr = table.NewRow();
            dr[0] = medicine.ID;
            dr[1] = medicine.Name;
            dr[2] = medicine.Creation;
            dr[3] = medicine.Expiration;
            dr[4] = medicine.Ingredients;
            dr[5] = medicine.Manufacturer;
            table.Rows.Add(dr);
        }

        public void AddMedicineRequestToTable(MedicineCreationRequest request)
        {
            dr = medicineCreationRequestDataTable.NewRow();
            dr[0] = request.ID;
            dr[1] = request.Name;
            dr[2] = request.Manufacturer;
            medicineCreationRequestDataTable.Rows.Add(dr);
        }
        public void AddEquipmentToEquipmentTable(string name)
        {
            dr = equipmentDataTable.NewRow();
            dr[0] = name;
            equipmentDataTable.Rows.Add(dr);
        }
        public void AddDoctorToDoctorsTable(Doctor doctor)
        {
            dr = doctorsDataTable.NewRow();
            dr[0] = doctor.ID;
            dr[1] = doctor.FirstName;
            dr[2] = doctor.LastName;
            doctorsDataTable.Rows.Add(dr);
        }
        public void AddAppointmentToAppointmentsTable(Appointment appointment, int patientID)
        {
            dr = appointmentsDataTable.NewRow();
            dr[0] = appointment.ID;
            dr[1] = appointment.Type;
            dr[2] = appointment.ScheduledDate;
            dr[3] = appointment.CreatedDate;
            dr[4] = appointment.Emergency;
            dr[5] = appointment.DoctorID;
            dr[6] = appointment.HospitalRoomID;
            dr[7] = patientID;
            appointmentsDataTable.Rows.Add(dr);
        }
        public void FillHealthRecordData(string healthRecordID, string height, string weight, string alergens, string previousDiseases, string anamnesis)
        {
            idLabel.Content = healthRecordID;
            heightTextBox.Text = height;
            weigthTextBox.Text = weight;
            anamnesisLabel.Content = anamnesis;
            alergensLabel.Content = alergens;
            previousDiseasesLabel.Content = previousDiseases;
        }

        //---------------------------------------------------------------------------------------
        //First menu buttons
        private void ScheduleRewiewButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            scheduleWindowService.FillAppointmentsTable(AppointmentRepository.Appointments);
        }
        private void DrugMenagmentButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            drugManagementGrid.Visibility = Visibility.Visible;
            medicineCreationWindowService = new MedicineCreationRequestWindowService(this,signedUser);
        }


        //---------------------------------------------------------------------------------------
        //Buttons on schedule rewiew menu
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddDeleteAppointmentService appointmentService = new AddDeleteAppointmentService(signedUser,this,true,scheduleWindowService);

        }
        private void Alter_Click(object sender, RoutedEventArgs e)
        {

            int rowIndex = TableService.GetSelectedIndex(scheduleDataGrid);
            if (rowIndex == -1) return;
            AddDeleteAppointmentService appointmentService = new AddDeleteAppointmentService(signedUser, this, false, scheduleWindowService, rowIndex);

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int rowIndex = TableService.GetSelectedIndex(scheduleDataGrid);
            if(rowIndex == -1) return;
            AppointmentRepository.Appointments.RemoveAt(rowIndex);
            scheduleWindowService.FillAppointmentsTable(AppointmentRepository.Appointments);
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            scheduleWindowService.SearchAppointments();
        }
        private void HealthRecord_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = healthRecordWindowService.FindHealthRecord();
            if (!sucessfull)
                return;
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Visible;
        }
        private void StartAppointment_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = scheduleWindowService.FindRoomID();
            if (!sucessfull)
                return;
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            updateHealthRecord.Visibility = Visibility.Visible;
            createAnamnesis.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            createAPrescription.Visibility = Visibility.Visible;
            referToADifferentPracticioner.Visibility = Visibility.Visible;
            medicineGrid.Visibility = Visibility.Visible;
            healthRecordWindowService.UpdateHealthRecordWindow();
        }


        //---------------------------------------------------------------------------------------
        //Health record menu buttons
        private void CreateAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            healthRecordGrid.Visibility = Visibility.Collapsed;
            anamnesisGrid.Visibility = Visibility.Visible;
        }
        private void UpdateHealthRecord_Click(object sender, RoutedEventArgs e)
        {
            ExitHealthRecord();
            healthRecordWindowService.UpdateHealthRecord();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ExitHealthRecord();
            scheduleGrid.Visibility = Visibility.Collapsed;
            equimpentUpdateGrid.Visibility = Visibility.Visible;
            scheduleWindowService.FillEquipmentTable();
        }
        private void ReferToADifferentPracticioner_Click(object sender, RoutedEventArgs e)
        {
            healthRecordGrid.Visibility = Visibility.Collapsed;
            doctorReferalGrid.Visibility = Visibility.Visible;
            scheduleWindowService.CreateAReferral();
            List<Doctor>doctors = scheduleWindowService.GetDoctorsByType();
            scheduleWindowService.FillDoctorsTable(doctors);
        }

        private void ExitHealthRecord()
        {
            scheduleGrid.Visibility = Visibility.Visible;
            healthRecordGrid.Visibility = Visibility.Collapsed;
            updateHealthRecord.Visibility = Visibility.Collapsed;
            createAnamnesis.Visibility = Visibility.Collapsed;
            referToADifferentPracticioner.Visibility = Visibility.Collapsed;
            createAPrescription.Visibility = Visibility.Collapsed;
            medicineGrid.Visibility = Visibility.Collapsed;
        }

        public void EnableMedicineGrid(bool enable) {
            addMedicine.IsEnabled = enable;
            deleteMedicine.IsEnabled = enable;
            finishPrescriptionCreation.IsEnabled = enable;
            instructionsTextBox.IsEnabled = enable;
            consumptionPerDayComboBox.IsEnabled = enable;
            consumptionPeriodComboBox.IsEnabled = enable;
            addMedicineConsumptionTime.IsEnabled = enable;
            hourOfMedicineTakingComboBox.IsEnabled = enable;
            minuteOfMedicineTakingComboBox.IsEnabled = enable;
            addMedicineToPrescription.IsEnabled = enable;
        }
        private void CreateAPrescription_Click(object sender, RoutedEventArgs e)
        {
            EnableMedicineGrid(true);
            healthRecordWindowService.FillMedicineTakingComboBoxes();
        }
        private void AddMedicine_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = healthRecordWindowService.SelectMedicine();
            if (!sucessfull)
                return;
            healthRecordWindowService.FillSelectedMedicinesTable();
        }

        private void DeleteMedicine_Click(object sender, RoutedEventArgs e)
        {
            healthRecordWindowService.RemoveMedicineFromSelectedMedicine();
        }
        private void AddMedicineConsumptionTime_Click(object sender, RoutedEventArgs e)
        {
            healthRecordWindowService.AddMedicineConsumptionTime();
        }

        private void AddMedicineToPrescription_Click(object sender, RoutedEventArgs e)
        {
            healthRecordWindowService.AddMedicineToPrescription();
        }
        private void FinishPrescriptionCreation_Click(object sender, RoutedEventArgs e)
        {
            healthRecordWindowService.CreateAPrescription();
        }

        //---------------------------------------------------------------------------------------
        //Anamnesis creation buttons
        private void SubmitAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            scheduleWindowService.CreateAnamnessis();
            anamnesisGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
        }

        //---------------------------------------------------------------------------------------
        //Refferal creation buttons
        private void SubmitReferal_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = scheduleWindowService.FillReferralWithData();
            if (!sucessfull)
                return;
            doctorReferalGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;

        }
        private void SubmitAutomaticReferal_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = scheduleWindowService.FillAutomaticReferralWithData();
            if (!sucessfull)
                return;
            doctorReferalGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
        }
        private void DoctorReferalBack_Click(object sender, RoutedEventArgs e)
        {
            doctorReferalGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
        }

        //---------------------------------------------------------------------------------------
        //Combo box events
        private void DoctorTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Doctor> doctors = scheduleWindowService.GetDoctorsByType();
            if (doctors == null)
                return;
            scheduleWindowService.FillDoctorsTable(doctors);
        }
        private void SpecializationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Doctor> doctors = scheduleWindowService.GetDoctorsBySpecialization();
            if (doctors == null)
                return;
            scheduleWindowService.FillDoctorsTable(doctors);
        }

        //---------------------------------------------------------------------------------------
        //Window closing
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            HealthRecordRepository.Save();
            AppointmentRepository.Save();
            PrescriptionRepository.Save();
            MedicineInstructionRepository.Save();
            ReferralRepository.Save();
            MedicineCreationRequestRepository.Save();
            HospitalRoomRepository.SaveRooms(HospitalRoomRepository.Rooms);
            LogOut();
        }

        private void LogOut()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void AcceptMedicineRequestButton_Click(object sender, RoutedEventArgs e)
        {

            medicineCreationWindowService.AcceptRequestState();
            MessageBox.Show("Request approved");
        }

        private void DenyMedicineRequestButton_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = medicineCreationWindowService.DenyRequestState();
            if (!sucessfull)
                return;
            MessageBox.Show("Request denied");
        }

        private void EquimpentUpdateGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            scheduleWindowService.UpdateEquipment(scheduleWindowService.selectedRoomID);
        }

        private void SubmitEquipmentChanges_Click(object sender, RoutedEventArgs e)
        {
            scheduleWindowService.SubmitEquipmentChanges(scheduleWindowService.selectedRoomID);
        }

        private void BackEquipmentChanges_Click(object sender, RoutedEventArgs e)
        {
            scheduleGrid.Visibility = Visibility.Visible;
            equimpentUpdateGrid.Visibility = Visibility.Collapsed;
        }

        private void MedicineRequestsDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            medicineCreationWindowService.ParseIngredients();
        }
    }
}
