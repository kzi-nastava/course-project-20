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

namespace HealthCareCenter
{
    public partial class DoctorWindow : Window
    {
        PrescriptionService prescriptionService;
        private int selectedPatientID;
        
        private Doctor signedUser;
        private DataTable appointmentsDataTable;
        private DataTable patientsDataTable;
        private DataTable doctorsDataTable;
        private DataTable medicineDataTable;
        private DataTable selectedMedicineDataTable;
        private DataTable medicineCreationRequestDataTable;
        private Referral referral;
        private bool patientsTableIsFilled = false;
        private int appointmentIndex;
        private int healthRecordIndex;
        private int comboBoxChangeCounter = 0;
        HealthRecord selectedPatientsHealthRecord;
        Medicine chosenMedicine;
        DataRow dr;
        public DoctorWindow(Model.User user)
        {
            signedUser = (Doctor)user;
            prescriptionService = new PrescriptionService(signedUser.ID);
            CreateAppointmentTable();
            CreatePatientsTable();
            CreateDoctorsTable();
            CreateMedicineTable();
            CreateMedicineCreationRequestTable();
            HealthRecordRepository.Load();
            AppointmentRepository.Load();
            MedicineRepository.Load();
            PrescriptionRepository.Load();  
            MedicineInstructionRepository.Load();
            ReferralRepository.Load();
            MedicineCreationRequestRepository.Load();
            InitializeComponent();
            FillDateTimeComboBoxes();

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
            DataColumn dc7 = new DataColumn("Room", typeof(string));
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
        private void FillPatientsTable()
        {
            patientsDataTable.Rows.Clear();
            foreach (Patient patient in UserRepository.Patients)
            {
                dr = patientsDataTable.NewRow();
                dr[0] = patient.ID;
                dr[1] = patient.FirstName;
                dr[2] = patient.LastName;
                patientsDataTable.Rows.Add(dr);
            }
            patientsTableIsFilled = true;
            patientsDataGrid.ItemsSource = patientsDataTable.DefaultView;
        }
        private void FillMedicinesTable()
        {
            medicineDataTable.Rows.Clear();
            foreach(Medicine medicine in MedicineRepository.Medicines)
            {
                dr = medicineDataTable.NewRow();
                dr[0] = medicine.ID;
                dr[1] = medicine.Name;
                dr[2] = medicine.Creation;
                dr[3] = medicine.Expiration;
                dr[4] = medicine.Ingredients;
                dr[5] = medicine.Manufacturer;
                medicineDataTable.Rows.Add(dr);
            }
            medicationDataGrid.ItemsSource = medicineDataTable.DefaultView;
        }
        private void FillMedicineRequestsTable()
        {
            medicineCreationRequestDataTable.Rows.Clear();
            foreach(MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting)
                    continue;
                dr = medicineCreationRequestDataTable.NewRow();
                dr[0] = request.ID; 
                dr[1] = request.Name;
                dr[2] = request.Manufacturer;
                medicineCreationRequestDataTable.Rows.Add(dr);
            }
            medicineCreationRequestDataGrid.ItemsSource = medicineCreationRequestDataTable.DefaultView;
        }

        private void AddMedicineToTable(Medicine medicine)
        {
            dr = selectedMedicineDataTable.NewRow();
            dr[0] = medicine.ID;
            dr[1] = medicine.Name;
            dr[2] = medicine.Creation;
            dr[3] = medicine.Expiration;
            dr[4] = medicine.Ingredients;
            dr[5] = medicine.Manufacturer;
            selectedMedicineDataTable.Rows.Add(dr);
            selectedMedicationDataGrid.ItemsSource = selectedMedicineDataTable.DefaultView;
        }
        private void DeleteMedicineFromTable(int index)
        {
            selectedMedicineDataTable.Rows.RemoveAt(index); 
        }
        private void FillDoctorsTable(List<Doctor>doctors)
        {
            doctorsDataTable.Rows.Clear();
            foreach (Doctor doctor in doctors)
            {
                dr = doctorsDataTable.NewRow();
                dr[0] = doctor.ID;
                dr[1] = doctor.FirstName;
                dr[2] = doctor.LastName;
                doctorsDataTable.Rows.Add(dr);
            }
            doctorsDataGrid.ItemsSource = doctorsDataTable.DefaultView;
        }
        private void FillDateTimeComboBoxes()
        {
            for (int i = 1; i <= 31; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                dayComboBox.Items.Add(s);
                dayChoiceComboBox.Items.Add(s);
            }
            for (int i = 1; i <= 12; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                monthComboBox.Items.Add(s);
                monthChoiceComboBox.Items.Add(s);
            }
            yearComboBox.Items.Add("2022");
            yearComboBox.Items.Add("2023");
            yearComboBox.Items.Add("2024");
            yearChoiceComboBox.Items.Add("2022");
            yearChoiceComboBox.Items.Add("2023");
            yearChoiceComboBox.Items.Add("2024");
            for (int i = 8; i <= 21; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                hourComboBox.Items.Add(s);
            }
            for (int i = 0; i <= 45; i += 15)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                minuteComboBox.Items.Add(s);
            }
        }
        private void FillMedicineTakingComboBoxes()
        {
            for (int i = 0; i <= 24; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
               hourOfMedicineTakingComboBox.Items.Add(s);
            }
            for (int i = 0; i <= 59; i += 1)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
                minuteOfMedicineTakingComboBox.Items.Add(s);
            }
        }
        private void FillAppointmentsTable(List<Appointment> appointments)
        {
            appointmentsDataTable.Rows.Clear();
            if (AppointmentRepository.Appointments == null)
            {
                appointments = AppointmentRepository.Load();
            }

            foreach (Appointment appointment in appointments)
            {
                if (appointment.DoctorID != signedUser.ID)
                {
                    continue;
                }
                dr = appointmentsDataTable.NewRow();
                dr[0] = appointment.ID;
                dr[1] = appointment.Type;
                dr[2] = appointment.ScheduledDate;
                dr[3] = appointment.CreatedDate;
                dr[4] = appointment.Emergency;
                dr[5] = appointment.DoctorID;
                dr[6] = appointment.HospitalRoomID;
                foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
                {
                    if (healthRecord.ID == appointment.HealthRecordID)
                    {
                        dr[7] = healthRecord.PatientID;
                        break;
                    }
                }
                appointmentsDataTable.Rows.Add(dr);
            }
            scheduleDataGrid.ItemsSource = appointmentsDataTable.DefaultView;
        }
        private void FillApointmentWithData(Appointment appointment, bool isBeingCreated)
        {
            int id = getRowItemID(patientsDataGrid, "Id");
            if (id == -1) return;
            string selectedValue;
            try
            {
                selectedValue = ((ComboBoxItem)appointmentTypeComboBox.SelectedItem).Content.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            bool emergency = (bool)emergencyCheckBox.IsChecked;
            bool error = ValidateDateTimeComboBoxes();
            if (error)
                return;
            string unparsedDate = ParseDateTimeComboBoxes();
            DateTime date = DateTime.ParseExact(unparsedDate,
                    Constants.DateTimeFormat,
                    CultureInfo.InvariantCulture);
            foreach (Appointment appointments in AppointmentRepository.Appointments)
            {
                if (appointments.ID == appointment.ID)
                {
                    continue;
                }

                TimeSpan timeSpan = appointments.ScheduledDate.Subtract(date);
                if (Math.Abs(timeSpan.TotalMinutes) < 15)
                {
                    throw new ArgumentException("Termin je zauzet");
                }
            }
            DateTime currentDate = DateTime.Today;
            appointment.Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedValue);
            appointment.Emergency = emergency;
            appointment.DoctorID = signedUser.ID;
            foreach(Patient patient in UserRepository.Patients)
            {
                if (patient.ID == id)
                {
                    appointment.HealthRecordID = patient.HealthRecordID;
                    break;
                }
            }
            appointment.HospitalRoomID = 1;
            appointment.ScheduledDate = date;
            appointment.CreatedDate = currentDate;
            if (isBeingCreated)
            {
                appointment.ID = ++AppointmentRepository.LargestID;
                AppointmentRepository.Appointments.Add(appointment);
            }
            FillAppointmentsTable(AppointmentRepository.Appointments);
            appointmentCreationGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
        }
        private string ParseDateTimeComboBoxes()
        {
            string day, month, year, hour, minute;
            day = dayComboBox.SelectedItem.ToString();
            month = monthComboBox.SelectedItem.ToString();
            year = yearComboBox.SelectedItem.ToString();
            hour = hourComboBox.SelectedItem.ToString();
            minute = minuteComboBox.SelectedItem.ToString();
            return  day + "/" + month + "/" + year + " " + hour + ":" + minute;
        }
        private bool ValidateDateTimeComboBoxes()
        {
            if (dayComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select day from combo box");
                return true;
            }
            if (monthComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select month from combo box");
                return true;
            }
            if (yearComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select year from combo box");
                return true;
            }
            if (hourComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select hour from combo box");
                return true;
            }
            if (minuteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select minute from combo box");
                return true;
            }
            return false;
        }
        private void FillAppointmentWithDefaultValues(Appointment appointment)
        {
            string[] timeFragments = appointment.ScheduledDate.ToString().Split("/");
            dayComboBox.SelectedItem = timeFragments[0];
            string[] yearAndTime = timeFragments[2].Split(" ");
            string[] time = yearAndTime[1].Split(":");
            dayComboBox.SelectedIndex = int.Parse(timeFragments[1]) - 1;
            monthComboBox.SelectedIndex = int.Parse(timeFragments[0]) - 1;
            yearComboBox.SelectedIndex = int.Parse(yearAndTime[0]) - 2022;
            int hour = int.Parse(time[0]);
            if (hour <= 12 && yearAndTime[2] == "AM")
            {
                hour -= 8;
            }
            else
            {
                hour += 4;
            }

            hourComboBox.SelectedIndex = hour;                               
            minuteComboBox.SelectedIndex = int.Parse(time[1]) / 15;
            emergencyCheckBox.IsChecked = appointment.Emergency;
            if (appointment.Type == AppointmentType.Checkup)
            {
                appointmentTypeComboBox.SelectedIndex = 0;
            }
            else
            {
                appointmentTypeComboBox.SelectedIndex = 1;
            }

            int patientIndex = 0;
            foreach (Patient patient in UserRepository.Patients)
            {
                patientIndex++;
                if (patient.ID == appointment.HealthRecordID)
                {
                    break;
                }
            }
            patientsDataGrid.SelectedIndex = patientIndex;

        }
        private void FillHealthRecordData(HealthRecord healthRecord)
        {
            idLable.Content = healthRecord.ID.ToString();
            heightTextBox.Text = healthRecord.Height.ToString();
            weigthTextBox.Text = healthRecord.Weight.ToString();
            CheckPreviousDiseases(healthRecord);
            CheckAlergens(healthRecord);
            if(AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis == null)
            {
                anamnesisLabel.Content = "No anamnesis";
                createAPrescription.IsEnabled = false;
            }
            else
            {
                anamnesisLabel.Content = AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis.Comment;
                createAPrescription.IsEnabled = true;
            }
        }
        private void CheckAlergens(HealthRecord healthRecord)
        {
            string alergens = "";
            if (healthRecord.Allergens != null)
            {
                foreach (string s in healthRecord.Allergens)
                {
                    alergens += "," + s;
                }

                if (healthRecord.Allergens.Count == 0)
                {
                    alergensTextBox.Text = "";
                }
                else
                {
                    alergensTextBox.Text = alergens.Substring(1, alergens.Length - 1);
                }
            }
            else
            {
                alergensTextBox.Text = "none";
            }
        }
        private void CheckPreviousDiseases(HealthRecord healthRecord)
        {
            string previousDiseases = "";
            if (healthRecord.PreviousDiseases != null)
            {
                foreach (string s in healthRecord.PreviousDiseases)
                {
                    previousDiseases += "," + s;
                }

                if (healthRecord.PreviousDiseases.Count == 0)
                {
                    previousDiseasesTextBox.Text = "";
                }
                else
                {
                    previousDiseasesTextBox.Text = previousDiseases.Substring(1, previousDiseases.Length - 1);
                }
            }
            else
            {
                previousDiseasesTextBox.Text = "none";
            }
        }

        //---------------------------------------------------------------------------------------
        //First menu buttons
        private void ScheduleRewiewButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            FillAppointmentsTable(AppointmentRepository.Appointments);
        }
        private void DrugMenagmentButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            drugManagementGrid.Visibility = Visibility.Visible;
            FillMedicineRequestsTable();
        }


        //---------------------------------------------------------------------------------------
        //Buttons on schedule rewiew menu
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            appointmentCreationGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            alterAppointment.Visibility = Visibility.Collapsed;
            sumbitAppointment.Visibility = Visibility.Visible;
            if (patientsTableIsFilled)
            {
                return;
            }
            FillPatientsTable();

        }
        private void Alter_Click(object sender, RoutedEventArgs e)
        {

            int rowIndex;
            rowIndex = scheduleDataGrid.SelectedIndex;
            if (rowIndex == -1)
            {
                MessageBox.Show("Choose an appointment from the table");
                return;
            }
            appointmentCreationGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            alterAppointment.Visibility = Visibility.Visible;
            sumbitAppointment.Visibility = Visibility.Collapsed;
            if (!patientsTableIsFilled)
            {
                FillPatientsTable();
            }
            appointmentIndex = rowIndex;
            Appointment appointment = AppointmentRepository.Appointments[rowIndex];
            FillAppointmentWithDefaultValues(appointment);
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int rowIndex;
            try
            {
                rowIndex = scheduleDataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Select an appointment");
                return;
            }
            AppointmentRepository.Appointments.RemoveAt(rowIndex);
            FillAppointmentsTable(AppointmentRepository.Appointments);
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string day, month, year;
            CheckDateComboBoxes();

            day = dayChoiceComboBox.SelectedItem.ToString();
            month = monthChoiceComboBox.SelectedItem.ToString();
            year = yearChoiceComboBox.SelectedItem.ToString();

            string unparsedDate = day + "/" + month + "/" + year;
            DateTime date = DateTime.ParseExact(unparsedDate,
                    Constants.DateFormat,
                    CultureInfo.InvariantCulture);
            List<Appointment> appointmentsResult = new List<Appointment>();
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                TimeSpan timeSpan = appointment.ScheduledDate.Subtract(date);
                if (timeSpan.TotalDays <= 3 && timeSpan.TotalDays >= 0)
                {
                    appointmentsResult.Add(appointment);
                }
            }
            FillAppointmentsTable(appointmentsResult);
        }
        private void HealthRecord_Click(object sender, RoutedEventArgs e)
        {
            int id = getRowItemID(scheduleDataGrid, "Patient Id");
            if(id == -1) return;
            appointmentIndex = scheduleDataGrid.SelectedIndex;
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Visible;

            int counter = 0;

            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (healthRecord.PatientID == id)
                {
                    healthRecordIndex = counter;
                    FillHealthRecordData(healthRecord);
                    break;
                }
                counter++;
            }
        }
        private void StartAppointment_Click(object sender, RoutedEventArgs e)
        {
            int patientID = getRowItemID(scheduleDataGrid, "Patient ID");
            if(patientID == -1) return;
            selectedPatientID = patientID;
            appointmentIndex = scheduleDataGrid.SelectedIndex;  
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            updateHealthRecord.Visibility = Visibility.Visible;
            createAnamnesis.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            createAPrescription.Visibility = Visibility.Visible;
            referToADifferentPracticioner.Visibility = Visibility.Visible;
            medicineGrid.Visibility = Visibility.Visible;
            FillMedicinesTable();
            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (healthRecord.PatientID == patientID)
                {
                    FillHealthRecordData(healthRecord);
                    selectedPatientsHealthRecord = healthRecord;
                    break;
                }
            }
        }
        //---------------------------------------------------------------------------------------
        //Schedule rewiew menu functions
        private void CheckDateComboBoxes()
        {
            if (dayChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a day");
                return;
            }
            if (monthChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a month");
                return;
            }
            if (yearChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a year");
                return;
            }
        }

        //---------------------------------------------------------------------------------------
        //Buttons on appointment creation menu
        private void SubmitAppointment_Click(object sender, RoutedEventArgs e)
        {

            Appointment appointment = new Appointment();
            try
            {
                FillApointmentWithData(appointment,true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        //---------------------------------------------------------------------------------------
        //Buttons on appointment altering menu
        private void AlterAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = AppointmentRepository.Appointments[appointmentIndex];
            try
            {
                FillApointmentWithData(appointment,false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
                return;
            }
            FillAppointmentsTable(AppointmentRepository.Appointments);
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
            exitHealthRecord();
            HealthRecordRepository.Records[healthRecordIndex].Height = double.Parse(heightTextBox.Text);
            HealthRecordRepository.Records[healthRecordIndex].Weight = double.Parse(weigthTextBox.Text);
            HealthRecordRepository.Records[healthRecordIndex].PreviousDiseases.Clear();
            string[] previousDiseases = previousDiseasesTextBox.Text.Split(",");
            foreach(string disease in previousDiseases)
            {
                if (string.IsNullOrWhiteSpace(disease))
                {
                    continue;
                }

                HealthRecordRepository.Records[healthRecordIndex].PreviousDiseases.Add(disease);
            }
            HealthRecordRepository.Records[healthRecordIndex].Allergens.Clear();
            string[] allergens = alergensTextBox.Text.Split(",");
            foreach (string allergen in allergens)
            {
                if (string.IsNullOrWhiteSpace(allergen))
                {
                    continue;
                }

                HealthRecordRepository.Records[healthRecordIndex].Allergens.Add(allergen);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            exitHealthRecord();
        }
        private void ReferToADifferentPracticioner_Click(object sender, RoutedEventArgs e)
        {
            healthRecordGrid.Visibility = Visibility.Collapsed;
            doctorReferalGrid.Visibility = Visibility.Visible;
            referral = new Referral();
            referral.ID = ++ReferralRepository.LargestID;
            List<Doctor>doctors = getDoctorsByType();
            FillDoctorsTable(doctors);
        }

        private void exitHealthRecord()
        {
            scheduleGrid.Visibility = Visibility.Visible;
            healthRecordGrid.Visibility = Visibility.Collapsed;
            updateHealthRecord.Visibility = Visibility.Collapsed;
            createAnamnesis.Visibility = Visibility.Collapsed;
            referToADifferentPracticioner.Visibility = Visibility.Collapsed;
            createAPrescription.Visibility = Visibility.Collapsed;
            medicineGrid.Visibility = Visibility.Collapsed;
        }

        private void enableMedicineGrid(bool enable) {
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
        private void createAPrescription_Click(object sender, RoutedEventArgs e)
        {
            enableMedicineGrid(true);
            FillMedicineTakingComboBoxes();
        }
        private void addMedicine_Click(object sender, RoutedEventArgs e)
        {
            if(prescriptionService.SelectedMedicine != null)
            {
                MessageBox.Show("U already selected a medicine");
                return;
            }
            int medicineIndex;
            try
            {
                medicineIndex = medicationDataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Choose a medicine");
                return;
            }
            chosenMedicine = MedicineRepository.Medicines[medicineIndex];
            bool isAlergic = CheckAlergies(chosenMedicine);
            if (isAlergic)
                return;
            prescriptionService.SelectedMedicine = chosenMedicine;
            AddMedicineToTable(chosenMedicine);
        }

        private void deleteMedicine_Click(object sender, RoutedEventArgs e)
        {
            int medicineIndex;
            try
            {
                medicineIndex = selectedMedicationDataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Choose a medicine");
                return;
            }
            try
            {
                prescriptionService.SelectedMedicine = null;
                DeleteMedicineFromTable(medicineIndex);
            }
            catch { }
        }
        private void addMedicineConsumptionTime_Click(object sender, RoutedEventArgs e)
        {
            string hour = hourOfMedicineTakingComboBox.SelectedItem.ToString();
            string minute = minuteOfMedicineTakingComboBox.SelectedItem.ToString();
            bool successful = prescriptionService.AddTime(hour, minute);
            if (successful)
                MessageBox.Show("Time added successfuly");
            else
                MessageBox.Show("An error happend with time conversion");
        }

        private void addMedicineToPrescription_Click(object sender, RoutedEventArgs e)
        {
            string instructions = instructionsTextBox.Text.Trim();
            if (instructions.Length == 0)
                instructions = "";
            int consumptionPeriodIndex = consumptionPeriodComboBox.SelectedIndex;
            ConsumptionPeriod consumptionPeriod;
            switch (consumptionPeriodIndex)
            {
                case 0: consumptionPeriod = ConsumptionPeriod.AfterEating; break;
                case 1: consumptionPeriod = ConsumptionPeriod.BeforeEating; break;
                case 2: consumptionPeriod = ConsumptionPeriod.Any; break;
                default: consumptionPeriod = ConsumptionPeriod.Any; break;
            }
            int consumptionsPerDay = consumptionPerDayComboBox.SelectedIndex + 1;
            bool successful = prescriptionService.CreateMedicineInstruction(0, instructions, consumptionsPerDay, consumptionPeriod, chosenMedicine.ID);
            if (!successful)
                return;
            prescriptionService.ClearData(false);
            selectedMedicineDataTable.Clear();
        }
        private void finishPrescriptionCreation_Click(object sender, RoutedEventArgs e)
        {

            bool successful = prescriptionService.CreateAPrescription();
            if (!successful)
                return;
            enableMedicineGrid(false);
            selectedMedicineDataTable.Rows.Clear();
            prescriptionService.ClearData(true);
            MessageBox.Show("Added prescription successfuly");
        }
        //---------------------------------------------------------------------------------------
        //Health record functions
        private bool CheckAlergies(Medicine medicine)
        {
            foreach (string ingredient in medicine.Ingredients)
            {
                if (selectedPatientsHealthRecord.Allergens.Contains(ingredient))
                {
                    MessageBox.Show("Patient is allergic to: " + ingredient);
                    return true;
                }
            }
            return false;
        }

        //---------------------------------------------------------------------------------------
        //Anamnesis creation buttons
        private void SubmitAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            string anamnesisComment = anamnesisTextBox.Text;
            Anamnesis anamnesis = new Anamnesis();
            anamnesis.Comment = anamnesisComment;
            anamnesis.ID = appointmentIndex;
            AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis = anamnesis;
            anamnesisGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
            anamnesisLabel.Content = anamnesisComment;
        }

        //---------------------------------------------------------------------------------------
        //Refferal creation buttons
        private void submitReferal_Click(object sender, RoutedEventArgs e)
        {
            int doctorIndex = getRowItemID(doctorsDataGrid, "Id");
            if (doctorIndex == -1) return;
            referral.DoctorID = doctorIndex;
            referral.PatientID = selectedPatientID;
            ReferralRepository.Referrals.Add(referral);
            doctorReferalGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;

        }
        private void submitAutomaticReferal_Click(object sender, RoutedEventArgs e)
        {
            int doctorIndex = getRowItemID(doctorsDataGrid, "Id");
            if(doctorIndex == -1) return;
            referral.DoctorID =  doctorIndex;
            referral.PatientID = selectedPatientID;
            ReferralRepository.Referrals.Add(referral);
            doctorReferalGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
        }
        private void doctorReferalBack_Click(object sender, RoutedEventArgs e)
        {
            doctorReferalGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
        }

        //---------------------------------------------------------------------------------------
        //Combo box events
        private void doctorTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Doctor> doctors = getDoctorsByType();
            if (doctors == null)
                return;
            FillDoctorsTable(doctors);
        }
        private void specializationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Doctor> doctors = getDoctorsBySpecialization();
            if (doctors == null)
                return;
            FillDoctorsTable(doctors);
        }

        //---------------------------------------------------------------------------------------
        //Data manipulation methods
        private List<Doctor> getDoctorsByType()
        {
            if (comboBoxChangeCounter == 0)
            {
                comboBoxChangeCounter++;
                return null;
            }
            int selectedIndex = doctorTypeComboBox.SelectedIndex;
            string chosenType = "";
            List<Doctor> doctors = new List<Doctor>();
            switch (selectedIndex)
            {
                case 0: chosenType = "General practitioner"; specializationComboBox.IsEnabled = false; submitAutomaticReferal.IsEnabled = false; break;
                case 1: chosenType = "Special"; specializationComboBox.IsEnabled = true; return getDoctorsBySpecialization();
            }

            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.Equals(chosenType))
                    doctors.Add(doctor);
            }
            return doctors;
        }
        private List<Doctor> getDoctorsBySpecialization()
        {
            int selectedIndex = specializationComboBox.SelectedIndex;
            string chosenType = "";
            List<Doctor> doctors = new List<Doctor>();
            if (selectedIndex == -1)
                return doctors;
            switch (selectedIndex)
            {
                case 0: chosenType = "Neurologist"; break;
                case 1: chosenType = "Cardiologist"; break;
                default: return doctors;
            }
            submitAutomaticReferal.IsEnabled = true;
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.Equals(chosenType))
                    doctors.Add(doctor);
            }
            return doctors;
        }

        private int getRowItemID(DataGrid grid,string key)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)grid.SelectedItems[0];
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return -1;
            }
            return (int)row[key];
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
            LogOut();
        }

        private void LogOut()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void acceptMedicineRequestButton_Click(object sender, RoutedEventArgs e)
        {

            int medicineCreationRequestID = getRowItemID(medicineCreationRequestDataGrid,"Id");
            MedicineCreationRequest selectedRequest = MedicineCreationRequestService.getMedicineCreationRequest(medicineCreationRequestID);
            selectedRequest.State = RequestState.Approved;
            FillMedicineRequestsTable();
            MessageBox.Show("Request approved");
        }

        private void denyMedicineRequestButton_Click(object sender, RoutedEventArgs e)
        {
            string denyMessage = medicineRequestDeniedTextBox.Text;
            if(denyMessage == "")
            {
                MessageBox.Show("Please provide a reason");
                return;
            }
            int medicineCreationRequestID = getRowItemID(medicineCreationRequestDataGrid, "Id");
            MedicineCreationRequest selectedRequest = MedicineCreationRequestService.getMedicineCreationRequest(medicineCreationRequestID);
            selectedRequest.State = RequestState.Denied;
            selectedRequest.DenyComment = denyMessage;
            FillMedicineRequestsTable();
            MessageBox.Show("Request denied");
        }

        private void medicineRequestsDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int medicineCreationRequestID = getRowItemID(medicineCreationRequestDataGrid,"Id");
            MedicineCreationRequest request = MedicineCreationRequestService.getMedicineCreationRequest(medicineCreationRequestID);
            string ingredients = "";
            foreach(string ingredient in request.Ingredients){
                ingredients += ingredient + ",";
            }
            ingredients = ingredients.Substring(0,ingredients.Length-1);
            ingredientsTextBlock.Text = ingredients;
        }
    }
}
