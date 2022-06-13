using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.DoctorGUI;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Prescriptions;
using HealthCareCenter.Core;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Repositories;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients;

namespace HealthCareCenter.GUI.Doctor.ViewModels
{
    public class HealthRecordWindowViewModel
    {
        private DoctorWindowViewModel windowService;
        private DoctorWindow window;
        private User signedUser;
        private int healthRecordIndex;
        private Medicine chosenMedicine;

        private HealthRecord selectedPatientsHealthRecord;
        public HealthRecordWindowViewModel(DoctorWindowViewModel service, User _signedUser, DoctorWindow _window)
        {
            window = _window;
            signedUser = _signedUser;
            windowService = service;
        }

        public void UpdateHealthRecord()
        {
            double height, weight;
            string[] previousDiseases, allergens;
            height = double.Parse(window.heightTextBox.Text);
            weight = double.Parse(window.weigthTextBox.Text);
            previousDiseases = window.previousDiseasesTextBox.Text.Split(",");
            allergens = window.alergensTextBox.Text.Split(",");
            HealthRecordService.Update(height, weight, previousDiseases, allergens, healthRecordIndex);
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
            bool successful = PrescriptionService.Create();
            if (!successful)
                return;
            window.EnableMedicineGrid(false);
            window.selectedMedicineDataTable.Rows.Clear();
            PrescriptionService.ClearData(true);
            MessageBox.Show("Added prescription successfuly");
        }

        public void UpdateHealthRecordWindow()
        {
            FillMedicinesTable();
            HealthRecord healthRecord = HealthRecordService.GetRecordByPatientID(windowService.selectedPatientID);
            ParseHealthRecordData(healthRecord);
            selectedPatientsHealthRecord = healthRecord;
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
            alergens = HealthRecordService.CheckAllergens(healthRecord);
            previousDiseases = HealthRecordService.CheckPreviousDiseases(healthRecord);
            healthRecordID = healthRecord.ID.ToString();
            height = healthRecord.Height.ToString();
            weight = healthRecord.Weight.ToString();
            if (AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis == null)
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

        public void FillMedicinesTable()
        {
            window.medicineDataTable.Rows.Clear();
            foreach (Medicine medicine in MedicineRepository.Medicines)
            {
                window.AddMedicineToMedicineTable(medicine, window.medicineDataTable);
            }
            window.medicationDataGrid.ItemsSource = window.medicineDataTable.DefaultView;
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

        public bool FindHealthRecord()
        {
            int id, appointmentIndex;
            id = TableService.GetRowItemID(window.scheduleDataGrid, "Patient Id");
            appointmentIndex = GetSelectedIndex(window.scheduleDataGrid);
            if (id == -1 || appointmentIndex == -1)
                return false;
            Core.Patients.Patient patient = PatientService.Get(id);
            HealthRecord healthRecord = HealthRecordService.Get(patient);
            if (patient == null || healthRecord == null)
                return false;
            healthRecordIndex = PatientService.GetIndex(id);
            ParseHealthRecordData(healthRecord);
            return true;
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


        private bool CheckAlergies(Medicine medicine)
        {
            string ingredient = HealthRecordService.IsAllergicTo(medicine, selectedPatientsHealthRecord);
            if (ingredient != "")
            {
                MessageBox.Show("Patient is allergic to: " + ingredient);
                return true;
            }
            return false;
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
        public void FillSelectedMedicinesTable()
        {
            window.selectedMedicineDataTable.Rows.Clear();
            Medicine medicine = PrescriptionService.SelectedMedicine;
            window.AddMedicineToMedicineTable(chosenMedicine, window.selectedMedicineDataTable);
            window.selectedMedicationDataGrid.ItemsSource = window.selectedMedicineDataTable.DefaultView;
        }
    }
}