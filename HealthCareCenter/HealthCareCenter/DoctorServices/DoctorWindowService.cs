using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCareCenter.Model;
using System.Data;
using HealthCareCenter.Enums;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.DoctorGUI;
using HealthCareCenter.Service;

namespace HealthCareCenter.DoctorServices
{
    public class DoctorWindowService
    {
        public int selectedRoomID;
        private int healthRecordIndex, selectedPatientID, selectedAppointmentIndex, comboBoxChangeCounter = 0;
        private HealthRecord selectedPatientsHealthRecord;
        private User _signedUser;
        private DoctorWindow window;
        private Referral referral;
        private Medicine chosenMedicine;
        public DoctorWindowService(User signedUser) {
            _signedUser = signedUser;
            window = new DoctorWindow(signedUser, this);
            window.Show();
            FillMedicineRequestsTable();
        }
        public void FillMedicineRequestsTable()
        {
            window.medicineCreationRequestDataTable.Rows.Clear();
            foreach (MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting)
                    continue;
                window.AddMedicineRequestToTable(request);
            }
            window.medicineCreationRequestDataGrid.ItemsSource = window.medicineCreationRequestDataTable.DefaultView;
        }
        public void FillMedicinesTable()
        {
            window.medicineDataTable.Rows.Clear();
            foreach (Medicine medicine in MedicineRepository.Medicines)
            {
                window.AddMedicineToMedicineTable(medicine, window.medicineDataTable);
            }
            window.medicationDataGrid.ItemsSource = window.medicineDataTable.DefaultView;
        }

        public void FillSelectedMedicinesTable()
        {
            window.selectedMedicineDataTable.Rows.Clear();
            Medicine medicine = PrescriptionService.SelectedMedicine;
            window.AddMedicineToMedicineTable(chosenMedicine, window.selectedMedicineDataTable);
            window.selectedMedicationDataGrid.ItemsSource = window.selectedMedicineDataTable.DefaultView;
        }

        public void FillEquipmentTable()
        {
            Room room;
            try
            {
               room = RoomService.GetRoom(selectedRoomID);
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
            referral.ID = ++ReferralRepository.LargestID;
        }

        public bool SelectMedicine()
        {
            int medicineIndex;
            bool isAlergic;
            if (PrescriptionService.SelectedMedicine != null)
            {
                MessageBox.Show("U already selected a medicine");
                return false;
            }
            medicineIndex = GetSelectedIndex(window.medicationDataGrid);
            if (medicineIndex == -1)
                return false;
            chosenMedicine = MedicineRepository.Medicines[medicineIndex];
            isAlergic = CheckAlergies(chosenMedicine);
            if (isAlergic)
                return false;
            PrescriptionService.SelectedMedicine = chosenMedicine;
            return true;
        }

        public void RemoveMedicineFromSelectedMedicine()
        {
            int medicineIndex = GetSelectedIndex(window.selectedMedicationDataGrid);
            if (medicineIndex == -1)
                return;
            try
            {
                PrescriptionService.SelectedMedicine = null;
                DeleteMedicineFromTable(medicineIndex);
            }
            catch { }
        }

        private void DeleteMedicineFromTable(int index)
        {
            window.selectedMedicineDataTable.Rows.RemoveAt(index);
        }
        public void FillDoctorsTable(List<Doctor> doctors)
        {
            window.doctorsDataTable.Rows.Clear();
            foreach (Doctor doctor in doctors)
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

        public void FillMedicineTakingComboBoxes()
        {
            for (int i = 0; i <= 24; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
                window.hourOfMedicineTakingComboBox.Items.Add(s);
            }
            for (int i = 0; i <= 59; i += 1)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
                window.minuteOfMedicineTakingComboBox.Items.Add(s);
            }
        }

        public void FillAppointmentsTable(List<Appointment> appointments)
        {
            window.appointmentsDataTable.Rows.Clear();
            if (AppointmentRepository.Appointments == null)
            {
                appointments = AppointmentRepository.Load();
            }

            foreach (Appointment appointment in appointments)
            {
                if (appointment.DoctorID != _signedUser.ID)
                {
                    continue;
                }
                int patientID = -1;
                HealthRecord patientsHealthRecord = HealthRecordService.FindRecord(appointment.HealthRecordID);
                if (patientsHealthRecord != null)
                    window.AddAppointmentToAppointmentsTable(appointment, patientsHealthRecord.PatientID);
                else
                    window.AddAppointmentToAppointmentsTable(appointment, -1);
            }
            window.scheduleDataGrid.ItemsSource = window.appointmentsDataTable.DefaultView;
        }

        public void UpdateHealthRecord()
        {
            HealthRecordRepository.Records[healthRecordIndex].Height = double.Parse(window.heightTextBox.Text);
            HealthRecordRepository.Records[healthRecordIndex].Weight = double.Parse(window.weigthTextBox.Text);
            HealthRecordRepository.Records[healthRecordIndex].PreviousDiseases.Clear();
            string[] previousDiseases = window.previousDiseasesTextBox.Text.Split(",");
            foreach (string disease in previousDiseases)
            {
                if (string.IsNullOrWhiteSpace(disease))
                {
                    continue;
                }

                HealthRecordRepository.Records[healthRecordIndex].PreviousDiseases.Add(disease);
            }
            HealthRecordRepository.Records[healthRecordIndex].Allergens.Clear();
            string[] allergens = window.alergensTextBox.Text.Split(",");
            foreach (string allergen in allergens)
            {
                if (string.IsNullOrWhiteSpace(allergen))
                {
                    continue;
                }

                HealthRecordRepository.Records[healthRecordIndex].Allergens.Add(allergen);
            }
        }
   
        public void ParseHealthRecordData(HealthRecord healthRecord)
        {
            int appointmentIndex;
            string height, weight, alergens, previousDiseases, healthRecordID, anamnesis;
            appointmentIndex = window.scheduleDataGrid.SelectedIndex;
            if (appointmentIndex == -1)
            {
                MessageBox.Show("No row is selected");
                return;
            }
            alergens = HealthRecordService.CheckAlergens(healthRecord);
            previousDiseases = HealthRecordService.CheckPreviousDiseases(healthRecord);
            healthRecordID = healthRecord.ID.ToString();
            height = healthRecord.Height.ToString();
            weight = healthRecord.Weight.ToString();
            if (AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis.Comment == "")
            {
                anamnesis = "No anamnesis";
                window.createAPrescription.IsEnabled = false;
            }
            else
            {
                anamnesis = AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis.Comment;
                window.createAPrescription.IsEnabled = true;
            }
            window.FillHealthRecordData(healthRecordID, height, weight, alergens, previousDiseases, anamnesis);
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
            List<Appointment> appointmentsResult = AppointmentService.GetAppointmentsInTheFollowingDays(date, 3);
            FillAppointmentsTable(appointmentsResult);
        }

        public bool FindRoomID()
        {
            int patientID, roomID, appointmentIndex;
            patientID = GetRowItemID(window.scheduleDataGrid, "Patient ID");
            roomID = GetRowItemID(window.scheduleDataGrid, "Room ID");
            appointmentIndex = GetSelectedIndex(window.scheduleDataGrid);
            if (patientID == -1 || roomID == -1 || appointmentIndex == -1)  
                return false;
            selectedRoomID = roomID;
            selectedPatientID = patientID;
            selectedAppointmentIndex = appointmentIndex;
            return true;
        }

        public void UpdateHealthRecordWindow()
        {
            FillMedicinesTable();
            HealthRecord healthRecord = HealthRecordService.FindRecordByPatientID(selectedPatientID);
            ParseHealthRecordData(healthRecord);
            selectedPatientsHealthRecord = healthRecord;
        }
        public bool FindHealthRecord()
        {
            int id, appointmentIndex;
            id = GetRowItemID(window.scheduleDataGrid, "Patient Id");
            appointmentIndex = GetSelectedIndex(window.scheduleDataGrid);
            if (id == -1 || appointmentIndex == -1) 
                return false;
            Patient patient = PatientService.FindPatient(id);
            HealthRecord healthRecord = HealthRecordService.FindRecord(patient);
            if (patient == null || healthRecord == null)
                return false;
            healthRecordIndex = PatientService.FindPatientIndex(id);
            ParseHealthRecordData(healthRecord);
            return true;
        }

        public void AddMedicineConsumptionTime()
        {
            string hour = window.hourOfMedicineTakingComboBox.SelectedItem.ToString();
            string minute = window.minuteOfMedicineTakingComboBox.SelectedItem.ToString();
            bool successful = PrescriptionService.AddTime(hour, minute);
            if (successful)
                MessageBox.Show("Time added successfuly");
            else
                MessageBox.Show("An error happend with time conversion");
        } 

        public void AddMedicineToPrescription()
        {
            int consumptionPeriodIndex, consumptionsPerDay;
            string instructions;
            instructions = window.instructionsTextBox.Text.Trim();
            if (instructions.Length == 0)
                instructions = "";
            consumptionPeriodIndex = window.consumptionPeriodComboBox.SelectedIndex;
            ConsumptionPeriod consumptionPeriod;
            switch (consumptionPeriodIndex)
            {
                case 0: consumptionPeriod = ConsumptionPeriod.AfterEating; break;
                case 1: consumptionPeriod = ConsumptionPeriod.BeforeEating; break;
                case 2: consumptionPeriod = ConsumptionPeriod.Any; break;
                default: consumptionPeriod = ConsumptionPeriod.Any; break;
            }
            consumptionsPerDay = window.consumptionPerDayComboBox.SelectedIndex + 1;
            bool successful = PrescriptionService.CreateMedicineInstruction(0, instructions, consumptionsPerDay, consumptionPeriod, chosenMedicine.ID);
            if (!successful)
                return;
            PrescriptionService.ClearData(false);
            window.selectedMedicineDataTable.Clear();
        }

        public void CreateAPrescription()
        {
            bool successful = PrescriptionService.CreateAPrescription();
            if (!successful)
                return;
            window.enableMedicineGrid(false);
            window.selectedMedicineDataTable.Rows.Clear();
            PrescriptionService.ClearData(true);
            MessageBox.Show("Added prescription successfuly");
        }

        public void CreateAnamnessis()
        {
            string anamnesisComment = window.anamnesisTextBox.Text;
            Anamnesis anamnesis = new Anamnesis();
            anamnesis.Comment = anamnesisComment;
            anamnesis.ID = selectedAppointmentIndex;
            AppointmentRepository.Appointments[selectedAppointmentIndex].PatientAnamnesis = anamnesis;
            window.anamnesisLabel.Content = anamnesisComment;
        }

        public bool FillReferralWithData()
        {
            int doctorID = GetRowItemID(window.doctorsDataGrid, "Id");
            if (doctorID == -1) 
                return false;
            referral.DoctorID = doctorID;
            referral.PatientID = selectedPatientID;
            ReferralRepository.Referrals.Add(referral);
            return true;
        }

        public bool FillAutomaticReferralWithData()
        {
            DataRowView row;
            try
            {
                row = (DataRowView)window.doctorsDataGrid.Items[0];
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return false;
            }
            int doctorID = (int)row["Id"];
            if (doctorID == -1) 
                return false;
            referral.DoctorID = doctorID;
            referral.PatientID = selectedPatientID;
            ReferralRepository.Referrals.Add(referral);
            return true;
        }
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

        public List<Doctor> GetDoctorsByType()
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
            return DoctorService.GetDoctorsByType(chosenType);
        }

        public List<Doctor> GetDoctorsBySpecialization()
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
            return DoctorService.GetDoctorsBySpecialization(chosenType);
        }
        public int GetSelectedIndex(DataGrid dataGrid)
        {
            int rowIndex;
            try
            {
                rowIndex = dataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return -1;
            }
            return rowIndex;
        }
        public int GetRowItemID(DataGrid grid, string key)
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
        public string GetRowItem(DataGrid grid, string key)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)grid.SelectedItems[0];
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return "";
            }
            return (string)row[key];
        }

        public string GetComboBoxItem(ComboBox comboBox)
        {
            string selectedValue = "";
            try
            {
                selectedValue = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return selectedValue;
        }
    }
}
