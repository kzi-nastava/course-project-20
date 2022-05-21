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
using HealthCareCenter.Service;

namespace HealthCareCenter
{
    public partial class PatientWindow : Window
    {
        private Patient signedPatient;
        private List<Appointment> unfinishedAppointments;
        HealthRecord patientHealthRecord;

        // appointment datagrid table
        private DataTable appointmentsDataTable;

        // scheduling appointment items
        private DataTable allDoctorsDataTable;
        private DataTable allAvailableTimeTable;
        private DataRowView chosenDoctor;  // selected row with doctor information in allDoctorsDataGrid
        private DateTime chosenScheduleDate;
        private bool isScheduleDateChosen = false;
        private DataRowView chosenAppointment;  // selected row with appointment information, used when making changes to an appointment
        private bool isModification = false;
        private bool shouldSendRequestToSecretary = false;
        private string startRangePriorityTime;
        private string endRangePriorityTime;
        private DataTable availablePriorityAppointmentsDataTable;

        // trolling limits
        private const int creationTrollLimit = 8;
        private const int modificationTrollLimit = 5;

        public PatientWindow(User user)
        {
            signedPatient = (Patient)user;
            HealthRecordRepository.Load();
            patientHealthRecord = HealthRecordService.FindRecord(signedPatient);
            HealthRecordRepository.Records = null;
            
            // loading all necessary information
            //============================================
            AppointmentRepository.Load();
            AppointmentChangeRequestRepository.Load();
            //============================================

            unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedPatient.HealthRecordID);
            InitializeComponent();

            // creating the appointment table and making that window visible
            ClearWindow();
            myAppointmentsGrid.Visibility = Visibility.Visible;
            CreateAppointmentTable();
            FillUnfinishedAppointmentsTable();
            currentActionTextBlock.Text = "My appointments";

            DisplayNotifications();
        }

        private void DisplayNotifications()
        {
            List<Notification> notifications = NotificationService.FindUnopened(signedPatient);
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

        // clear window methods
        //==============================================================================
        private void ClearWindow()
        {
            ClearMyAppointmentGrid();
            ClearCreateAppointmentGrid();
            ClearPrioritySchedulingGrid();
            ClearSearchDoctorGrid();
            ClearMyNotificationGrid();
            ClearMyHealthRecordGrid();
            ClearSurveyGrids();
        }

        private void ClearMyAppointmentGrid()
        {
            myAppointmentsGrid.Visibility = Visibility.Collapsed;
            if (appointmentsDataTable != null)
            {
                appointmentsDataTable.Clear();
            }
        }

        private void ClearCreateAppointmentGrid()
        {
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
            isModification = false;
            shouldSendRequestToSecretary = false;
        }

        private void ClearPrioritySchedulingGrid()
        {
            dayChoicePriorityComboBox.Items.Clear();
            monthChoicePriorityComboBox.Items.Clear();
            yearChoicePriorityComboBox.Items.Clear();
            prioritySchedulingGrid.Visibility = Visibility.Collapsed;
            startTimeRangePriorityComboBox.Items.Clear();
            endTimeRangePriorityComboBox.Items.Clear();
            if (availablePriorityAppointmentsDataTable != null)
            {
                availablePriorityAppointmentsDataTable.Clear();
            }
            availablePriorityAppointmentsDataTable = null;
        }

        private void ClearSearchDoctorGrid()
        {
            searchDoctorsGrid.Visibility = Visibility.Collapsed;
            searchDoctorSortCriteriaComboBox.Items.Clear();
            searchDoctorKeyWordSearchComboBox.Items.Clear();
        }

        private void ClearMyNotificationGrid()
        {
            myNotificationGrid.Visibility = Visibility.Collapsed;
        }

        private void ClearMyHealthRecordGrid()
        {
            myHealthRecordGrid.Visibility = Visibility.Collapsed;
            healthRecordAppointmentsSortCriteriaComboBox.Items.Clear();
        }

        private void ClearSurveyGrids()
        {
            doctorSurveyGrid.Visibility = Visibility.Collapsed;

            healthCenterGrid.Visibility = Visibility.Collapsed;
        }
        //==============================================================================

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

        private void FillUnfinishedAppointmentsTable()
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
            List<TimeSchedule> allPossibleSchedules = TimeScheduleService.GetDailySchedulesFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                int chosenDoctorID = Convert.ToInt32(chosenDoctor[0]);
                if (appointment.DoctorID == chosenDoctorID)
                {
                    if (appointment.ScheduledDate.Date.CompareTo(chosenScheduleDate.Date) == 0)
                    {
                        TimeSchedule unavailableSchedule = new TimeSchedule(appointment.ScheduledDate.Hour, appointment.ScheduledDate.Minute);
                        allPossibleSchedules.Remove(unavailableSchedule);
                    }
                }
            }

            DataRow row;
            foreach (TimeSchedule availableTime in allPossibleSchedules)
            {
                row = allAvailableTimeTable.NewRow();
                row[0] = chosenScheduleDate.Day;
                row[1] = chosenScheduleDate.Month;
                row[2] = chosenScheduleDate.Year;
                row[3] = Convert.ToInt32(availableTime.Hours);
                row[4] = Convert.ToInt32(availableTime.Minutes);
                allAvailableTimeTable.Rows.Add(row);
            }
            allAvailableTimeCUDDataGrid.ItemsSource = allAvailableTimeTable.DefaultView;
        }

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
            if (isModification)
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
                PatientID = signedPatient.ID
            };

            int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(newChangeRequest.NewDate, Enums.RoomType.Checkup);
            if (hospitalRoomID == -1)
            {
                MessageBox.Show("There are no rooms available for that schedule");
                return;
            }

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", confirmationMessage, System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                AddAppointmentToSchedule(newChangeRequest, hospitalRoomID);
                ClearWindow();
                CreateAppointmentTable();
                FillUnfinishedAppointmentsTable();
                createAppointmentGrid.Visibility = Visibility.Collapsed;
                myAppointmentsGrid.Visibility = Visibility.Visible;
            }

        }
        //==============================================================================

        // action menu click methods
        //=======================================================================================
        private void myAppointmentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            myAppointmentsGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "My appointments";
            CreateAppointmentTable();
            FillUnfinishedAppointmentsTable();

        }

        private void searchDoctorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearWindow();
            searchDoctorsGrid.Visibility = Visibility.Visible;
            currentActionTextBlock.Text = "Search for doctor";
            FillSearchDoctorKeyWordCriteriaComboBox();
            FillSearchDoctorSortComboBox();
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
            FillHealthRecordItems();
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
            isModification = true;

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

        // anti troll mechanism methods
        //=======================================================================================
        private bool CheckCreationTroll()
        {
            int creationCount = 0;
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.HealthRecordID == signedPatient.HealthRecordID)
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
                    if (signedPatient.ID == patient.ID)
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

        private bool CheckModificationTroll()
        {
            int modificationCount = 0;
            foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
            {
                if (changeRequest.PatientID == signedPatient.ID)
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
                    if (signedPatient.ID == patient.ID)
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
            foreach (TimeSchedule scheduleTime in TimeScheduleService.GetDailySchedulesFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0))
            {
                startTimeRangePriorityComboBox.Items.Add(scheduleTime.ToString());
                endTimeRangePriorityComboBox.Items.Add(scheduleTime.ToString());
            }
        }

        private bool PrioritySearchSelectionValidation()
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
            TimeSchedule startRange = new TimeSchedule(startRangePriorityTime);
            TimeSchedule endRange = new TimeSchedule(endRangePriorityTime);
            return startRange < endRange;
        }

        private void CreateAvailablePriorityAppointmentsTable()
        {
            availablePriorityAppointmentsDataTable = new DataTable("Appointments");
            availablePriorityAppointmentsDataTable.Columns.Add(new DataColumn("Doctor's ID", typeof(string)));
            availablePriorityAppointmentsDataTable.Columns.Add(new DataColumn("Appointment date", typeof(string)));
        }

        private void FillAvailablePriorityAppointmentsTable(List<AppointmentChangeRequest> similarAppointments)
        {
            DataRow row;
            int length = similarAppointments.Count >= 3 ? 3 : similarAppointments.Count;  // ensuring that max 3 appointments are shown
            for (int i = 0; i < length; ++i)
            {
                AppointmentChangeRequest appointmentRequest = similarAppointments[i];
                row = availablePriorityAppointmentsDataTable.NewRow();
                row[0] = appointmentRequest.NewDoctorID;
                row[1] = appointmentRequest.NewDate;
                availablePriorityAppointmentsDataTable.Rows.Add(row);
            }
            allAppointmentsPriorityDataGrid.ItemsSource = availablePriorityAppointmentsDataTable.DefaultView;
        }

        private AppointmentChangeRequest GetPriorityAppointment()
        {
            AppointmentChangeRequest newAppointmentRequest = null;
            if (Convert.ToBoolean(doctorPriorityRadioButton.IsChecked))
            {
                newAppointmentRequest = GetDoctorPriorityAppointment();
            }
            if (Convert.ToBoolean(timePriorityRadioButton.IsChecked))
            {
                newAppointmentRequest = GetTimePriorityAppointment();
            }

            return newAppointmentRequest;
        }

        private AppointmentChangeRequest GetDoctorPriorityAppointment()
        {
            AppointmentChangeRequest newAppointmentRequest = BothPrioritiesSearch();

            if (newAppointmentRequest == null)
            {
                newAppointmentRequest = SameDoctorDifferentTimeSearch();
            }

            return newAppointmentRequest;
        }

        private AppointmentChangeRequest GetTimePriorityAppointment()
        {
            AppointmentChangeRequest newAppointmentRequest = BothPrioritiesSearch();

            if (newAppointmentRequest == null)
            {
                newAppointmentRequest = DifferentDoctorSameTimeSearch();
            }

            return newAppointmentRequest;
        }

        private AppointmentChangeRequest PrioritySearch(int doctorID, List<TimeSchedule> possibleSchedules)
        {
            AppointmentChangeRequest newAppointmentRequest = null;
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            bool appointmentFound = false;
            while (date.Date.CompareTo(chosenScheduleDate.Date) <= 0 && !appointmentFound)
            {
                foreach (TimeSchedule timeSchedule in possibleSchedules)
                {
                    bool isAvailable = true;
                    foreach (Appointment appointment in AppointmentRepository.Appointments)
                    {
                        if (doctorID == appointment.DoctorID &&
                            timeSchedule.Hours == appointment.ScheduledDate.Hour &&
                            timeSchedule.Minutes == appointment.ScheduledDate.Minute &&
                            appointment.ScheduledDate.Date.CompareTo(date.Date) == 0)
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        string scheduleDateParse = date.ToString().Split(" ")[0] + " " + timeSchedule.ToString();
                        DateTime scheduleDate = Convert.ToDateTime(scheduleDateParse);

                        int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(scheduleDate, Enums.RoomType.Checkup);
                        if (hospitalRoomID == -1)
                        {
                            continue;
                        }

                        newAppointmentRequest = new AppointmentChangeRequest
                        {
                            ID = ++AppointmentChangeRequestRepository.LargestID,
                            AppointmentID = ++AppointmentRepository.LargestID,
                            RequestType = Enums.RequestType.MakeChanges,
                            State = Enums.RequestState.Waiting,
                            NewAppointmentType = Enums.AppointmentType.Checkup,
                            NewDate = scheduleDate,
                            DateSent = DateTime.Now,
                            NewDoctorID = doctorID,
                            PatientID = signedPatient.ID
                        };
                        appointmentFound = true;
                        break;
                    }
                }
                date = date.AddDays(1);
            }

            return newAppointmentRequest;
        }

        private AppointmentChangeRequest BothPrioritiesSearch()
        {
            // searches for an appointment based on both priorities

            TimeSchedule startRange = new TimeSchedule(startRangePriorityTime);
            TimeSchedule endRange = new TimeSchedule(endRangePriorityTime);
            List<TimeSchedule> possibleSchedules = TimeScheduleService.GetDailySchedulesFromRange(startRange, endRange);

            return PrioritySearch(Convert.ToInt32(chosenDoctor[0]), possibleSchedules);

        }

        private AppointmentChangeRequest DifferentDoctorSameTimeSearch()
        {
            // searches for an available appointment in the selected time span
            // for any doctor except the doctor that the patient chose

            TimeSchedule startRange = new TimeSchedule(startRangePriorityTime);
            TimeSchedule endRange = new TimeSchedule(endRangePriorityTime);
            List<TimeSchedule> possibleSchedules = TimeScheduleService.GetDailySchedulesFromRange(startRange, endRange);
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == Convert.ToInt32(chosenDoctor[0]))
                {
                    continue;
                }

                AppointmentChangeRequest newAppoinmentRequest = PrioritySearch(doctor.ID, possibleSchedules);
                if (newAppoinmentRequest != null)
                {
                    return newAppoinmentRequest;
                }
            }

            return null;
        }

        private AppointmentChangeRequest SameDoctorDifferentTimeSearch()
        {
            // searches the chosen doctor with every time range except the one given

            TimeSchedule startRange = new TimeSchedule(startRangePriorityTime);
            TimeSchedule endRange = new TimeSchedule(endRangePriorityTime);
            List<TimeSchedule> possibleSchedules = TimeScheduleService.GetDailySchedulesOppositeOfRange(startRange, endRange);

            return PrioritySearch(Convert.ToInt32(chosenDoctor[0]), possibleSchedules);
        }

        private List<AppointmentChangeRequest> GetAppointmentsSimilarToPriorites()
        {
            List<AppointmentChangeRequest> similarAppointments = new List<AppointmentChangeRequest>();

            TimeSchedule startRange = new TimeSchedule(startRangePriorityTime);
            TimeSchedule endRange = new TimeSchedule(endRangePriorityTime);
            List<TimeSchedule> possibleSchedules = TimeScheduleService.GetDailySchedulesFromRange(startRange, endRange);
            List<TimeSchedule> oppositePossibleSchedules = TimeScheduleService.GetDailySchedulesOppositeOfRange(startRange, endRange);

            if (Convert.ToBoolean(doctorPriorityRadioButton.IsChecked))
            {
                // similar to DifferentDoctorSameTimeSearch except it adds the found appointments
                // to a list instead of returning one appointment

                foreach (Doctor doctor in UserRepository.Doctors)
                {
                    similarAppointments.AddRange(AppointmentsSimilarToPrioritySearch(doctor.ID, possibleSchedules));
                    if (similarAppointments.Count >= 3)
                    {
                        break;
                    }
                }
            }
            if (Convert.ToBoolean(timePriorityRadioButton.IsChecked))
            {
                // similar to SameDoctorDifferentTimeSearch except it adds the found appointments
                // to a list instead of returning one appointment

                similarAppointments.AddRange(AppointmentsSimilarToPrioritySearch(Convert.ToInt32(chosenDoctor[0]), oppositePossibleSchedules));
            }

            return similarAppointments;
        }

        private List<AppointmentChangeRequest> AppointmentsSimilarToPrioritySearch(int doctorID, List<TimeSchedule> possibleSchedules)
        {
            List<AppointmentChangeRequest> similarAppointments = new List<AppointmentChangeRequest>();
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            while (date.Date.CompareTo(chosenScheduleDate.Date) <= 0 && similarAppointments.Count < 3)
            {
                foreach (TimeSchedule timeSchedule in possibleSchedules)
                {
                    bool isAvailable = true;
                    foreach (Appointment appointment in AppointmentRepository.Appointments)
                    {
                        if (doctorID == appointment.DoctorID &&
                            timeSchedule.Hours == appointment.ScheduledDate.Hour &&
                            timeSchedule.Minutes == appointment.ScheduledDate.Minute &&
                            appointment.ScheduledDate.Date.CompareTo(date.Date) == 0)
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        string scheduleDateParse = date.ToString().Split(" ")[0] + " " + timeSchedule.ToString();
                        DateTime scheduleDate = Convert.ToDateTime(scheduleDateParse);

                        int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(scheduleDate, Enums.RoomType.Checkup);
                        if (hospitalRoomID == -1)
                        {
                            continue;
                        }

                        AppointmentChangeRequest possibleAppointmentRequest = new AppointmentChangeRequest
                        {
                            ID = ++AppointmentChangeRequestRepository.LargestID,
                            AppointmentID = -1,
                            RequestType = Enums.RequestType.MakeChanges,
                            State = Enums.RequestState.Waiting,
                            NewAppointmentType = Enums.AppointmentType.Checkup,
                            NewDate = scheduleDate,
                            DateSent = DateTime.Now,
                            NewDoctorID = doctorID,
                            PatientID = signedPatient.ID
                        };
                        similarAppointments.Add(possibleAppointmentRequest);
                        break;
                    }
                }
                date = date.AddDays(1);
            }

            return similarAppointments;
        }

        private void PriorityFoundScheduling()
        {
            AppointmentChangeRequest newAppointmentRequest = GetPriorityAppointment();
            if (newAppointmentRequest == null)
            {
                MessageBox.Show("No appointments found based on the chosen priority");
                List<AppointmentChangeRequest> similarAppointments = GetAppointmentsSimilarToPriorites();
                if (similarAppointments.Count <= 0)
                {
                    MessageBox.Show("There are no appointments available that match the selected parameters");
                    ClearWindow();
                    CreateAppointmentTable();
                    FillUnfinishedAppointmentsTable();
                    myAppointmentsGrid.Visibility = Visibility.Visible;
                    return;
                }
                MessageBox.Show("Appointments similar to selected parameters found, please choose the most convenient");

                CreateAvailablePriorityAppointmentsTable();
                FillAvailablePriorityAppointmentsTable(similarAppointments);
            }
            else
            {
                int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(newAppointmentRequest.NewDate, Enums.RoomType.Checkup);
                if (hospitalRoomID == -1)
                {
                    MessageBox.Show("There are no rooms available for that schedule");
                    return;
                }

                string appointmentDetails = "Doctor: " + newAppointmentRequest.NewDoctorID + ", Schedule: " + newAppointmentRequest.NewDate.ToString();
                MessageBox.Show("Appointment found: " + appointmentDetails);
                string confirmationMessage = "Are you sure you want to schedule this appointment?";
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", confirmationMessage, System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    AddAppointmentToSchedule(newAppointmentRequest, hospitalRoomID);
                    ClearWindow();
                    CreateAppointmentTable();
                    FillUnfinishedAppointmentsTable();
                    createAppointmentGrid.Visibility = Visibility.Collapsed;
                    myAppointmentsGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void PriorityNotFoundScheduling()
        {
            chosenAppointment = allAppointmentsPriorityDataGrid.SelectedItem as DataRowView;
            if (chosenAppointment == null)
            {
                MessageBox.Show("No appointment selected");
                return;
            }

            AppointmentChangeRequest newAppointmentRequest = new AppointmentChangeRequest
            {
                ID = ++AppointmentChangeRequestRepository.LargestID,
                AppointmentID = ++AppointmentRepository.LargestID,
                RequestType = Enums.RequestType.MakeChanges,
                State = Enums.RequestState.Waiting,
                NewAppointmentType = Enums.AppointmentType.Checkup,
                NewDate = Convert.ToDateTime(chosenAppointment[1]),
                DateSent = DateTime.Now,
                NewDoctorID = Convert.ToInt32(chosenAppointment[0]),
                PatientID = signedPatient.ID
            };

            int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(newAppointmentRequest.NewDate, Enums.RoomType.Checkup);
            if (hospitalRoomID == -1)
            {
                MessageBox.Show("There are no rooms available for that schedule");
                return;
            }

            string confirmationMessage = "Are you sure you want to schedule this appointment?";
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", confirmationMessage, System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                chosenAppointment = null;
                AddAppointmentToSchedule(newAppointmentRequest, hospitalRoomID);
                ClearWindow();
                CreateAppointmentTable();
                FillUnfinishedAppointmentsTable();
                myAppointmentsGrid.Visibility = Visibility.Visible;
            }

            return;
        }

        private void scheduleAppointmentPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            if (availablePriorityAppointmentsDataTable != null)
            {
                PriorityNotFoundScheduling();
                return;
            }

            if (!PrioritySearchSelectionValidation())
            {
                return;
            }

            PriorityFoundScheduling();
        }
        //=======================================================================================

        // health record methods
        //=======================================================================================
        private void showAnamnesisButton_Click(object sender, RoutedEventArgs e)
        {
            chosenAppointment = healthRecordAppointmentsDataGrid.SelectedItem as DataRowView;
            if (chosenAppointment == null)
            {
                MessageBox.Show("No appointment chosen");
                return;
            }

            foreach (Appointment appointment in AppointmentRepository.GetPatientFinishedAppointments(signedPatient.HealthRecordID))
            {
                if (appointment.ID == Convert.ToInt32(chosenAppointment[0]))
                {
                    if (appointment.PatientAnamnesis == null)
                    {
                        MessageBox.Show("This appointment doesn't have a filled anamnesis");
                        return;
                    }
                    anamnesisTextBox.Text = appointment.PatientAnamnesis.Comment;
                    return;
                }
            }
        }

        private void sortHealthRecordAppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            var sortCriteriaChosen = healthRecordAppointmentsSortCriteriaComboBox.SelectedItem;
            if (sortCriteriaChosen == null)
            {
                MessageBox.Show("Sort criteria not chosen");
                return;
            }

            switch (sortCriteriaChosen.ToString())
            {
                case "Date":
                    FillHealthRecordAppointmentTableSortedDate();
                    break;
                case "Doctor":
                    FillHealthRecordAppointmentTableSortedDoctor();
                    break;
                case "Professional area":
                    FillHealthRecordAppointmentTableSortedProfessionalArea();
                    break;
            }
        }

        private void FillHealthRecordItems()
        {
            patientHeightTextBox.Text = patientHealthRecord.Height.ToString() + "cm";
            patientWeightTextBox.Text = patientHealthRecord.Weight.ToString() + "kg";

            previousDiseasesTextBox.Text = "";
            foreach (string disease in patientHealthRecord.PreviousDiseases)
            {
                previousDiseasesTextBox.Text += "- " + disease + "\n";
            }

            allergensTextBox.Text = "";
            foreach (string allergen in patientHealthRecord.Allergens)
            {
                allergensTextBox.Text += "- " + allergen + "\n";
            }

            FillHealthRecordAppointmentsSortCriteriaComboBoxes();

            CreateAppointmentTable();
            FillHealthRecordAppointmentTable(AppointmentRepository.GetPatientFinishedAppointments(signedPatient.HealthRecordID));

        }

        private void FillHealthRecordAppointmentTable(List<Appointment> finishedAppointments)
        {
            appointmentsDataTable.Clear();
            DataRow row;
            foreach (Appointment appointment in finishedAppointments)
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
            healthRecordAppointmentsDataGrid.ItemsSource = appointmentsDataTable.DefaultView;
        }

        private void FillHealthRecordAppointmentTableSortedDate()
        {
            List<Appointment> finishedAppointments = AppointmentRepository.GetPatientFinishedAppointments(signedPatient.HealthRecordID);
            AppointmentDateCompare appointmentComparation = new AppointmentDateCompare();
            finishedAppointments.Sort(appointmentComparation);

            FillHealthRecordAppointmentTable(finishedAppointments);
        }

        private void FillHealthRecordAppointmentTableSortedDoctor()
        {
            List<Appointment> finishedAppointments = AppointmentRepository.GetPatientFinishedAppointments(signedPatient.HealthRecordID);
            AppointmentDoctorCompare appointmentComparation = new AppointmentDoctorCompare();
            finishedAppointments.Sort(appointmentComparation);

            FillHealthRecordAppointmentTable(finishedAppointments);
        }

        private void FillHealthRecordAppointmentTableSortedProfessionalArea()
        {
            List<Appointment> finishedAppointments = AppointmentRepository.GetPatientFinishedAppointments(signedPatient.HealthRecordID);
            AppointmentProfessionalAreaCompare appointmentComparation = new AppointmentProfessionalAreaCompare();
            finishedAppointments.Sort(appointmentComparation);

            FillHealthRecordAppointmentTable(finishedAppointments);
        }

        private void FillHealthRecordAppointmentsSortCriteriaComboBoxes()
        {
            healthRecordAppointmentsSortCriteriaComboBox.Items.Add("Date");
            healthRecordAppointmentsSortCriteriaComboBox.Items.Add("Doctor");
            healthRecordAppointmentsSortCriteriaComboBox.Items.Add("Professional area");
        }
        //=======================================================================================


        // doctor search methods
        //=======================================================================================
        private void FillSearchDoctorKeyWordCriteriaComboBox()
        {
            searchDoctorKeyWordSearchComboBox.Items.Add("First name");
            searchDoctorKeyWordSearchComboBox.Items.Add("Last name");
            searchDoctorKeyWordSearchComboBox.Items.Add("Professional area");
        }

        private void FillSearchDoctorSortComboBox()
        {
            searchDoctorSortCriteriaComboBox.Items.Add("Search parameter");
            searchDoctorSortCriteriaComboBox.Items.Add("Rating");
        }

        //=======================================================================================

        // helper methods
        //=======================================================================================
        private void AddAppointmentToSchedule(AppointmentChangeRequest newChangeRequest, int hospitalRoomID)
        {
            // adds new/modified appointment to patientRepository.UnifinishedAppointments

            if (isModification)
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
                    HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newChangeRequest.AppointmentID);
                    AppointmentChangeRequestService.EditAppointment(newChangeRequest);
                    unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedPatient.HealthRecordID);
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
                    HealthRecordID = signedPatient.HealthRecordID,
                    HospitalRoomID = hospitalRoomID,
                    PatientAnamnesis = null
                };
                HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newChangeRequest.AppointmentID);
                AppointmentRepository.Appointments.Add(newAppointment);
                unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedPatient.HealthRecordID);
            }

            AppointmentRepository.Save();
        }

        private void CancelAppointment()
        {
            CheckModificationTroll();
            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest
            {
                ID = ++AppointmentChangeRequestRepository.LargestID,
                AppointmentID = Convert.ToInt32(chosenAppointment[0]),
                RequestType = Enums.RequestType.Delete,
                DateSent = DateTime.Now,
                PatientID = signedPatient.ID
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
                unfinishedAppointments = AppointmentRepository.GetPatientUnfinishedAppointments(signedPatient.HealthRecordID);
            }
            CreateAppointmentTable();
            FillUnfinishedAppointmentsTable();
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
                string day = i.ToString();
                if (day.Length == 1)
                {
                    day = "0" + day;
                }
                dayChoiceComboBox.Items.Add(day);
            }
            for (int i = 1; i <= 12; i++)
            {
                string month = i.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                monthChoiceComboBox.Items.Add(month);
            }
            int currentYear = DateTime.Now.Year;
            int nextYear = currentYear + 1;
            yearChoiceComboBox.Items.Add(currentYear.ToString());
            yearChoiceComboBox.Items.Add(nextYear.ToString());
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
        //=======================================================================================

        private void LogOut()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

    }
}
