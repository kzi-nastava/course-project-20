using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HealthCareCenter.Model;
using System.Data;

namespace HealthCareCenter
{
    public partial class PatientWindow : Window
    {
        private Patient signedUser;
        private List<Appointment> unfinishedAppointments;

        // appointment datagrid table
        private DataTable appointmentsDataTable;

        // scheduling appointment items
        private DataTable allDoctorsDataTable;
        private DataTable allAvailableTimeTable;
        private DataRowView chosenDoctor;  // selected row with doctor information in allDoctorsDataGrid
        private DateTime chosenScheduleDate;
        private bool isScheduleDateChosen = false;
        private DataRowView chosenAppointment;  // selected row with appointment information, used when making changes to an appointment
        private bool shouldSendRequestToSecretary = false;
        private string startRangePriorityTime;
        private string endRangePriorityTime;

        // trolling limits
        private const int creationTrollLimit = 8;
        private const int modificationTrollLimit = 5;

        public PatientWindow(User user)
        {
            signedUser = (Patient)user;
            
            // loading all necessary information
            //============================================
            AppointmentRepository.Load();
            AppointmentChangeRequestRepository.Load();
            //============================================

            unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedUser.HealthRecordID);
            InitializeComponent();

            // creating the appointment table and making that window visible
            ClearWindow();
            myAppointmentsGrid.Visibility = Visibility.Visible;
            CreateAppointmentTable();
            FillAppointmentTable();
            currentActionTextBlock.Text = "My appointments";

        }

        private void ClearWindow()
        {
            myAppointmentsGrid.Visibility = Visibility.Collapsed;
            //==
            createAppointmentGrid.Visibility = Visibility.Collapsed;
            if (allAvailableTimeTable != null)
            {
                allAvailableTimeTable.Clear();
            }
            chosenDoctor = null;
            isScheduleDateChosen = false;
            dayChoiceCUDComboBox.Items.Clear();
            monthChoiceCUDComboBox.Items.Clear();
            yearChoiceCUDComboBox.Items.Clear();
            shouldSendRequestToSecretary = false;
            //==
            dayChoicePriorityComboBox.Items.Clear();
            monthChoicePriorityComboBox.Items.Clear();
            yearChoicePriorityComboBox.Items.Clear();
            prioritySchedulingGrid.Visibility = Visibility.Collapsed;
            startTimeRangePriorityComboBox.Items.Clear();
            endTimeRangePriorityComboBox.Items.Clear();

            searchDoctorsGrid.Visibility = Visibility.Collapsed;

            myNotificationGrid.Visibility = Visibility.Collapsed;

            myHealthRecordGrid.Visibility = Visibility.Collapsed;

            doctorSurveyGrid.Visibility = Visibility.Collapsed;

            healthCenterGrid.Visibility = Visibility.Collapsed;

        }

        // create/modify appointment methods
        //==============================================================================
        private void CreateAppointmentTable()
        {
            appointmentsDataTable = new DataTable("Appointments");
            appointmentsDataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            appointmentsDataTable.Columns.Add(new DataColumn("Type of appointment", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Appointment date", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Creation date", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Emergency", typeof(bool)));
            appointmentsDataTable.Columns.Add(new DataColumn("Doctor's ID", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Room", typeof(string)));

        }

        private void FillAppointmentTable()
        {
            DataRow row;
            foreach (Appointment appointment in unfinishedAppointments)
            {
                row = appointmentsDataTable.NewRow();
                row[0] = appointment.ID;
                row[1] = appointment.Type;
                row[2] = appointment.ScheduledDate;
                row[3] = appointment.CreatedDate;
                row[4] = appointment.Emergency;
                row[5] = appointment.DoctorID;
                row[6] = appointment.HospitalRoomID;
                appointmentsDataTable.Rows.Add(row);
            }
            myAppointmentsDataGrid.ItemsSource = appointmentsDataTable.DefaultView;
        }

        private void CreateAvailableTimeTable()
        {
            allAvailableTimeTable = new DataTable("Available time");
            allAvailableTimeTable.Columns.Add(new DataColumn("Day", typeof(int)));
            allAvailableTimeTable.Columns.Add(new DataColumn("Month", typeof(int)));
            allAvailableTimeTable.Columns.Add(new DataColumn("Year", typeof(int)));
            allAvailableTimeTable.Columns.Add(new DataColumn("Hour", typeof(int)));
            allAvailableTimeTable.Columns.Add(new DataColumn("Minutes", typeof(int)));
        }

        private void FillAvailableTimeTable()
        {
            List<string> allPossibleSchedules = GetDailySchedulesFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                int chosenDoctorID = Convert.ToInt32(chosenDoctor[0]);
                if (appointment.DoctorID == chosenDoctorID)
                {
                    if (appointment.ScheduledDate.Date.CompareTo(chosenScheduleDate.Date) == 0)
                    {
                        string unavailableSchedule = appointment.ScheduledDate.Hour + ":" + appointment.ScheduledDate.Minute;
                        allPossibleSchedules.Remove(unavailableSchedule);
                    }
                }
            }

            DataRow row;
            foreach (string availableTime in allPossibleSchedules)
            {
                string[] time = availableTime.Split(":");
                row = allAvailableTimeTable.NewRow();
                row[0] = chosenScheduleDate.Day;
                row[1] = chosenScheduleDate.Month;
                row[2] = chosenScheduleDate.Year;
                row[3] = Convert.ToInt32(time[0]);
                row[4] = Convert.ToInt32(time[1]);
                allAvailableTimeTable.Rows.Add(row);
            }
            allAvailableTimeCUDDataGrid.ItemsSource = allAvailableTimeTable.DefaultView;
        }
        //==============================================================================

        // action menu click methods
        //=======================================================================================
        private void myAppointmentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            myAppointmentsGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "My appointments";
        }

        private void searchDoctorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            searchDoctorsGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "Search for doctor";
        }

        private void myNotificationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            myNotificationGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "My notifications";
        }

        private void myHealthRecordMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            myHealthRecordGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "My health record";
        }

        private void doctorSurveyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            doctorSurveyGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "Doctor survey";
        }

        private void healthCenterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            healthCenterGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "Health center survey";
        }

        private void logOutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
        }
        //=======================================================================================

        // my appointment menu button click methods
        //=======================================================================================
        private void createAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            myAppointmentsGrid.Visibility = Visibility.Collapsed;
            createAppointmentGrid.Visibility = Visibility.Visible;
            CreateDoctorTable();
            FillDoctorTable(allDoctorsCUDDataGrid);
            FillDateComboBoxes(dayChoiceCUDComboBox, monthChoiceCUDComboBox, yearChoiceCUDComboBox);
            chosenAppointment = null;
        }

        private void makeChangeAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            chosenAppointment = myAppointmentsDataGrid.SelectedItem as DataRowView;
            if (chosenAppointment == null)
            {
                MessageBox.Show("No appointment selected");
                return;
            }

            CheckShouldSendRequestToSecretary();

            myAppointmentsGrid.Visibility = Visibility.Collapsed;
            createAppointmentGrid.Visibility = Visibility.Visible;
            CreateDoctorTable();
            FillDoctorTable(allDoctorsCUDDataGrid);
            FillDateComboBoxes(dayChoiceCUDComboBox, monthChoiceCUDComboBox, yearChoiceCUDComboBox);
        }

        private void cancelAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            chosenAppointment = myAppointmentsDataGrid.SelectedItem as DataRowView;
            if (chosenAppointment == null)
            {
                MessageBox.Show("No appointment selected");
                return;
            }

            CheckShouldSendRequestToSecretary();

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel appointment", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                CancelAppointment();
            }
        }

        private void prioritySchedulingButton_Click(object sender, RoutedEventArgs e)
        {
            myAppointmentsGrid.Visibility = Visibility.Collapsed;
            prioritySchedulingGrid.Visibility = Visibility.Visible;
            chosenAppointment = null;

            FillDateComboBoxes(dayChoicePriorityComboBox, monthChoicePriorityComboBox, yearChoicePriorityComboBox);
            FillAppointmentRangeComboBoxes();
            CreateDoctorTable();
            FillDoctorTable(allDoctorsPriorityDataGrid);
        }
        //=======================================================================================

        // appointments creation/modification menu button click methods
        //=======================================================================================
        private void chooseDoctorCUDButton_Click(object sender, RoutedEventArgs e)
        {
            chosenDoctor = allDoctorsCUDDataGrid.SelectedItem as DataRowView;
            if (chosenDoctor == null)
            {
                MessageBox.Show("No doctor selected");
                return;
            }

            if (!IsDateValid(dayChoiceCUDComboBox, monthChoiceCUDComboBox, yearChoiceCUDComboBox))
            {
                return;
            }

            CreateAvailableTimeTable();
            FillAvailableTimeTable();

        }

        private void scheduleAppointmentCUDButton_Click(object sender, RoutedEventArgs e)
        {
            if (chosenDoctor == null)
            {
                MessageBox.Show("No doctor selected");
                return;
            }
            if (!isScheduleDateChosen)
            {
                MessageBox.Show("No date selected");
                return;
            }

            DataRowView chosenScheduleTime = allAvailableTimeCUDDataGrid.SelectedItem as DataRowView;
            if (chosenScheduleTime == null)
            {
                MessageBox.Show("No schedule time selected");
                return;
            }

            string confirmationMessage;
            if (IsModification())
            {
                confirmationMessage = "Make changes";
            }
            else
            {
                confirmationMessage = "Schedule appointment";
            }

            string newAppointmentDateParsed = chosenScheduleTime[0].ToString() + "/" + chosenScheduleTime[1].ToString() + "/" + chosenScheduleTime[2].ToString() + " " + chosenScheduleTime[3].ToString() + ":" + chosenScheduleTime[4].ToString();
            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest
            {
                ID = ++AppointmentChangeRequestRepository.LargestID,
                AppointmentID = (chosenAppointment == null) ? ++AppointmentRepository.LargestID : Convert.ToInt32(chosenAppointment[0]),
                RequestType = Enums.RequestType.MakeChanges,
                NewDate = Convert.ToDateTime(Convert.ToDateTime(newAppointmentDateParsed)),
                NewAppointmentType = Enums.AppointmentType.Checkup,
                NewDoctorID = Convert.ToInt32(chosenDoctor[0]),
                DateSent = DateTime.Now,
                PatientID = signedUser.ID
            };
            AddAppointmentToSchedule(newChangeRequest);

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", confirmationMessage, System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ClearWindow();
                CreateAppointmentTable();
                FillAppointmentTable();
                createAppointmentGrid.Visibility = Visibility.Collapsed;
                myAppointmentsGrid.Visibility = Visibility.Visible;
            }

        }

        private void AddAppointmentToSchedule(AppointmentChangeRequest newChangeRequest)
        {
            // adds new/modified appointment to patientRepository.UnifinishedAppointments

            if (IsModification())
            {
                if (CheckModificationTroll())
                {
                    return;
                }
                if (shouldSendRequestToSecretary)
                {
                    foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
                    {
                        if (changeRequest.AppointmentID == newChangeRequest.AppointmentID)
                        {
                            changeRequest.State = Enums.RequestState.Waiting;
                            changeRequest.NewDate = newChangeRequest.NewDate;
                            changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                            changeRequest.RequestType = newChangeRequest.RequestType;
                            AppointmentChangeRequestRepository.Save();
                            return;
                        }
                    }
                    AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                    AppointmentChangeRequestRepository.Save();
                    return;
                }
                else
                {
                    AppointmentChangeRequestService.EditAppointment(newChangeRequest);
                    unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedUser.HealthRecordID);
                }
            }
            else
            {
                if (CheckCreationTroll())
                {
                    return;
                }

                Appointment newAppointment = new Appointment
                {
                    ID = newChangeRequest.AppointmentID,
                    Type = Enums.AppointmentType.Checkup,
                    CreatedDate = DateTime.Now,
                    ScheduledDate = newChangeRequest.NewDate,
                    Emergency = false,
                    DoctorID = Convert.ToInt32(chosenDoctor[0]),
                    HealthRecordID = signedUser.HealthRecordID,
                    HospitalRoomID = -1,
                    PatientAnamnesis = null
                };
                AppointmentRepository.Appointments.Add(newAppointment);
                unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedUser.HealthRecordID);
            }

            AppointmentRepository.Save();
        }

        private bool IsModification()
        {
            // returns true if the user is in the process of modifying the appointment
            // and false if he is in the process of creating a new appointment

            return chosenAppointment != null;
        }
        //=======================================================================================

        // anti troll mechanism methods
        //=======================================================================================
        private bool CheckCreationTroll()
        {
            int creationCount = 0;
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.HealthRecordID == signedUser.HealthRecordID)
                {
                    TimeSpan timePassedSinceScheduling = DateTime.Now.Subtract(appointment.CreatedDate);
                    if (timePassedSinceScheduling.TotalDays < 30)
                    {
                        ++creationCount;
                    }
                }
            }

            if (creationCount >= creationTrollLimit)
            {
                MessageBox.Show("You have been blocked for excessive amounts of appointments created in the last 30 days");
                foreach (Patient patient in UserRepository.Patients)
                {
                    if (signedUser.ID == patient.ID)
                    {
                        MessageBox.Show(patient.ID.ToString());
                        patient.IsBlocked = true;
                        patient.BlockedBy = Enums.Blocker.System;
                        break;
                    }

                }
                UserRepository.SavePatients();
                LogOut();
                return true;
            }

            return false;
        }

        private bool CheckModificationTroll()
        {
            int modificationCount = 0;
            foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
            {
                if (changeRequest.PatientID == signedUser.ID)
                {
                    TimeSpan timePassedSinceScheduling = DateTime.Now.Subtract(changeRequest.DateSent);
                    if (timePassedSinceScheduling.TotalDays < 30)
                    {
                        ++modificationCount;
                    }
                }
            }

            if (modificationCount >= modificationTrollLimit)
            {
                MessageBox.Show("You have been blocked for excessive amounts of change requests sent in the last 30 days");
                foreach (Patient patient in UserRepository.Patients)
                {
                    if (signedUser.ID == patient.ID)
                    {
                        patient.IsBlocked = true;
                        patient.BlockedBy = Enums.Blocker.System;
                        break;
                    }
                }
                UserRepository.SavePatients();
                LogOut();
                return true;
            }

            return false;
        }
        //=======================================================================================

        // priority scheduling methods
        //=======================================================================================
        private void FillAppointmentRangeComboBoxes()
        {
            foreach (string scheduleTime in GetDailySchedulesFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0))
            {
                startTimeRangePriorityComboBox.Items.Add(scheduleTime);
                endTimeRangePriorityComboBox.Items.Add(scheduleTime);
            }
        }

        private bool IsPrioritySearchSelectionValid()
        {
            chosenDoctor = allDoctorsPriorityDataGrid.SelectedItem as DataRowView;
            if (chosenDoctor == null)
            {
                MessageBox.Show("Doctor not selected");
                return false;
            }

            if (!IsDateValid(dayChoicePriorityComboBox, monthChoicePriorityComboBox, yearChoicePriorityComboBox))
            {
                return false;
            }

            var startRange = startTimeRangePriorityComboBox.SelectedItem;
            var endRange = endTimeRangePriorityComboBox.SelectedItem;

            if (startRange == null || endRange == null)
            {
                MessageBox.Show("Start range and end range not selected");
                return false;
            }

            startRangePriorityTime = Convert.ToString(startRange);
            endRangePriorityTime = Convert.ToString(endRange);

            if (!IsTimeRangeValid())
            {
                MessageBox.Show("Invalid time range");
                return false;
            }

            return true; ;

        }

        private bool IsTimeRangeValid()
        {
            int[] startRange = GetHoursMinutesFromTime(startRangePriorityTime);
            int[] endRange = GetHoursMinutesFromTime(endRangePriorityTime);

            if (startRange[0] > endRange[0])
            {
                return false;
            }
            else if (startRange[0] == endRange[0])
            {
                if (startRange[1] >= endRange[1])
                {
                    return false;
                }
            }

            return true;
        }

        private void CreateAvailablePriorityAppointmentsTable()
        {

        }

        private Appointment GetPriorityAppointment()
        {
            Appointment newAppointment = null;
            if (Convert.ToBoolean(doctorPriorityRadioButton.IsChecked))
            {
                newAppointment = GetDoctorPriorityAppointment();
            }
            if (Convert.ToBoolean(timePriorityRadioButton.IsChecked))
            {
                newAppointment = GetTimePriorityAppointment();
            }

            return newAppointment;
        }

        private Appointment GetDoctorPriorityAppointment()
        {
            Appointment newAppointment = BothPrioritiesSearch();

            return newAppointment;
        }

        private Appointment GetTimePriorityAppointment()
        {
            Appointment newAppointment = BothPrioritiesSearch();

            return newAppointment;
        }

        private Appointment BothPrioritiesSearch()
        {   
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            int[] startRange = GetHoursMinutesFromTime(startRangePriorityTime);
            int[] endRange = GetHoursMinutesFromTime(endRangePriorityTime);
            while (date.Date.CompareTo(chosenScheduleDate.Date) <= 0)
            {
                foreach (string time in GetDailySchedulesFromRange(startRange[0], startRange[1], endRange[0], endRange[1]))
                {
                    int[] timeHoursMinutes = GetHoursMinutesFromTime(time);
                    bool isAvailable = true;
                    foreach (Appointment appointment in AppointmentRepository.Appointments)
                    {
                        if (Convert.ToInt32(chosenDoctor[0]) == appointment.DoctorID)
                        {
                            if (timeHoursMinutes[0] == appointment.ScheduledDate.Hour)
                            {
                                if (timeHoursMinutes[1] == appointment.ScheduledDate.Minute)
                                {
                                    if (appointment.ScheduledDate.Date.CompareTo(date.Date) == 0)
                                    {
                                        isAvailable = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (isAvailable)
                    {
                        string scheduleDateParse = date.ToString().Split(" ")[0];
                        scheduleDateParse += time;
                        DateTime scheduleDate = Convert.ToDateTime(scheduleDateParse);
                        Appointment newAppointment = new Appointment
                        {
                            ID = ++AppointmentRepository.LargestID,
                            Type = Enums.AppointmentType.Checkup,
                            ScheduledDate = scheduleDate,
                            CreatedDate = DateTime.Now,
                            Emergency = false,
                            DoctorID = Convert.ToInt32(chosenDoctor[0]),
                            HealthRecordID = signedUser.HealthRecordID,
                            HospitalRoomID = -1
                        };
                        return newAppointment;
                    }

                    date = date.AddDays(1);
                }
            }

            return null;
        }

        //=======================================================================================

        // priority scheduling button click methods
        //=======================================================================================
        private void scheduleAppointmentPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsPrioritySearchSelectionValid())
            {
                return;
            }

            Appointment appointment = GetPriorityAppointment();
            if (appointment == null)
            {
                MessageBox.Show("Appointment not found");
            }
            else
            {
                MessageBox.Show("Appointment found: " + appointment.ScheduledDate.ToString());
            }
        }
        //=======================================================================================

        // helper methods
        //=======================================================================================
        private void CancelAppointment()
        {
            CheckModificationTroll();
            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest
            {
                ID = ++AppointmentChangeRequestRepository.LargestID,
                AppointmentID = Convert.ToInt32(chosenAppointment[0]),
                RequestType = Enums.RequestType.Delete,
                DateSent = DateTime.Now,
                PatientID = signedUser.ID
            };

            if (shouldSendRequestToSecretary)
            {
                foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
                {
                    if (changeRequest.AppointmentID == newChangeRequest.AppointmentID)
                    {
                        changeRequest.State = Enums.RequestState.Waiting;
                        changeRequest.NewDate = newChangeRequest.NewDate;
                        changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                        changeRequest.RequestType = newChangeRequest.RequestType;
                        AppointmentChangeRequestRepository.Save();
                        return;
                    }
                }
                AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                AppointmentChangeRequestRepository.Save();
                return;
            }
            else
            {
                AppointmentChangeRequestService.DeleteAppointment(newChangeRequest);
                AppointmentRepository.Save();
                unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedUser.HealthRecordID);
            }
            CreateAppointmentTable();
            FillAppointmentTable();
        }

        private void CheckShouldSendRequestToSecretary()
        {
            DateTime chosenAppointmentDate = Convert.ToDateTime(chosenAppointment[2]);
            TimeSpan timeTillAppointment = chosenAppointmentDate.Subtract(DateTime.Now);
            if (timeTillAppointment.TotalDays <= 2)
            {
                MessageBox.Show("Since there are less than 2 days left until the appointment starts, the request will be sent to the secretary to confirm");
                shouldSendRequestToSecretary = true;
            }
        }

        private void FillDateComboBoxes(ComboBox dayChoiceComboBox, ComboBox monthChoiceComboBox, ComboBox yearChoiceComboBox)
        {
            for (int i = 1; i <= 31; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                dayChoiceComboBox.Items.Add(s);
            }
            for (int i = 1; i <= 12; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                monthChoiceComboBox.Items.Add(s);
            }
            int currentYear = DateTime.Now.Year;
            int nextYear = currentYear + 1;
            yearChoiceComboBox.Items.Add(currentYear.ToString());
            yearChoiceComboBox.Items.Add(nextYear.ToString());
        }

        private List<string> GetDailySchedulesFromRange(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            // returns all schedules from 8:00 to 21:00 knowing that an appointment lasts 15 minutes

            int hours = startHours;
            int minutes = startMinutes;
            List<string> possibleSchedules = new List<string>();
            while (hours < endHours || minutes < endMinutes)
            {
                string schedule = hours + ":" + minutes;
                possibleSchedules.Add(schedule);
                minutes += 15;
                if (minutes >= 60)
                {
                    ++hours;
                    minutes = 0;
                }
            }

            return possibleSchedules;
        }

        private void CreateDoctorTable()
        {
            allDoctorsDataTable = new DataTable("Doctors");
            allDoctorsDataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            allDoctorsDataTable.Columns.Add(new DataColumn("First name", typeof(string)));
            allDoctorsDataTable.Columns.Add(new DataColumn("Last name", typeof(string)));
            allDoctorsDataTable.Columns.Add(new DataColumn("Type", typeof(string)));
        }

        private void FillDoctorTable(DataGrid doctorGrid)
        {
            DataRow row;
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                row = allDoctorsDataTable.NewRow();
                row[0] = doctor.ID;
                row[1] = doctor.FirstName;
                row[2] = doctor.LastName;
                row[3] = doctor.Type;
                allDoctorsDataTable.Rows.Add(row);
            }
            doctorGrid.ItemsSource = allDoctorsDataTable.DefaultView;
        }

        private bool IsDateValid(ComboBox dayChoiceComboBox, ComboBox monthChoiceComboBox, ComboBox yearChoiceComboBox)
        {
            var dayChosen = dayChoiceComboBox.SelectedItem;
            var monthChosen = monthChoiceComboBox.SelectedItem;
            var yearChosen = yearChoiceComboBox.SelectedItem;

            if (dayChosen == null || monthChosen == null || yearChosen == null)
            {
                MessageBox.Show("Date combo boxes not filled");
                return false;
            }

            string chosenDate = Convert.ToInt32(dayChosen) + "/" + Convert.ToInt32(monthChosen) + "/" + Convert.ToInt32(yearChosen);
            try
            {
                chosenScheduleDate = Convert.ToDateTime(chosenDate);
                isScheduleDateChosen = true;
            }
            catch (Exception formatEx)
            {
                MessageBox.Show("Invalid date format");
                return false;
            }

            if (chosenScheduleDate.CompareTo(DateTime.Now) <= 0)
            {
                MessageBox.Show("Invalid date");
                return false;
            }

            return true;
        }

        private int[] GetHoursMinutesFromTime(string timeAsWord)
        {
            // takes in a string with the format HH:mm and returns an int array with 
            // hours and minutes as integers

            int[] hoursAndMinutes = new int[2];
            string[] time = timeAsWord.Split(":");

            hoursAndMinutes[0] = Convert.ToInt32(time[0]);
            hoursAndMinutes[1] = Convert.ToInt32(time[1]);

            return hoursAndMinutes;
        }
        //=======================================================================================

        private void LogOut()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

    }
}
