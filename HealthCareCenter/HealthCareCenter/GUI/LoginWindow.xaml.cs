using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using HealthCareCenter.Secretary;
using HealthCareCenter.GUI.Doctor.ViewModels;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Referrals.Services;
using HealthCareCenter.Core.Referrals.Repositories;
using HealthCareCenter.GUI.Patient.AppointmentCRUD.ViewModels;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter
{
    public partial class LoginWindow : Window
    {
        private static BackgroundWorker _backgroundWorker = null;
        private static IDynamicEquipmentService _dynamicEquipmentService;

        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;

        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IRenovationScheduleService _renovationScheduleService;

        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IDoctorSurveyRatingService _doctorSurveyRatingService;

        private readonly IMedicineCreationRequestService _medicineCreationRequestService;

        private AMedicineCreationRequestRepository medicineCreationRequestRepository = new MedicineCreationRequestRepository();

        public LoginWindow(IDynamicEquipmentService dynamicEquipmentService) : this()
        {
            _dynamicEquipmentService = dynamicEquipmentService;
        }

        public LoginWindow()
        {
            IRoomService roomService = new RoomService(new EquipmentRearrangementService(),
                new HospitalRoomUnderConstructionService(new HospitalRoomUnderConstructionRepository()),
                new HospitalRoomForRenovationService(new HospitalRoomForRenovationRepository()));
            IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService = new HospitalRoomUnderConstructionService(new HospitalRoomUnderConstructionRepository());
            IEquipmentRearrangementService equipmentRearrangementService = new EquipmentRearrangementService();
            IRenovationScheduleService renovationScheduleService = new RenovationScheduleService(
                new RoomService(new EquipmentRearrangementService(), new HospitalRoomUnderConstructionService(new HospitalRoomUnderConstructionRepository()), new HospitalRoomForRenovationService(new HospitalRoomForRenovationRepository())),
                new HospitalRoomUnderConstructionService(new HospitalRoomUnderConstructionRepository()), new HospitalRoomForRenovationService(new HospitalRoomForRenovationRepository()), new RenovationScheduleRepository());
            IDoctorSurveyRatingService doctorSurveyRatingService = new DoctorSurveyRatingService();
            IHospitalRoomForRenovationService hospitalRoomForRenovationService = new HospitalRoomForRenovationService(new HospitalRoomForRenovationRepository());
            IMedicineCreationRequestService medicineCreationRequestService = new MedicineCreationRequestService(new MedicineCreationRequestRepository());

            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleService = renovationScheduleService;
            _equipmentRearrangementService = equipmentRearrangementService;
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _medicineCreationRequestService = medicineCreationRequestService;

            InitializeComponent();
            DoEquipmentRearrangements();
            FinshPossibleRenovation();

            try
            {
                UserRepository.LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            StartBackgroundWorkerIfNeeded();
        }

        private void DoEquipmentRearrangements()
        {
            List<Equipment> equipments = EquipmentService.GetEquipments();
            for (int i = 0; i < equipments.Count; i++)
            {
                _equipmentRearrangementService.DoPossibleRearrangement(equipments[i]);
            }
        }

        private void FinshPossibleRenovation()
        {
            List<RenovationSchedule> renovations = _renovationScheduleService.GetRenovations();
            for (int i = 0; i < renovations.Count; i++)
            {
                _renovationScheduleService.FinishRenovation(renovations[i]);
            }
        }

        private void StartBackgroundWorkerIfNeeded()
        {
            if (_backgroundWorker == null)
            {
                _backgroundWorker = new BackgroundWorker();
                _backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
                _backgroundWorker.RunWorkerAsync(30 * Constants.Minute);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int timeBetweenWork = (int)e.Argument;
            BackgroundWork(timeBetweenWork);
        }

        private void BackgroundWork(int timeBetweenWork)
        {
            while (true)
            {
                _dynamicEquipmentService.FulfillRequestsIfNeeded();
                Thread.Sleep(timeBetweenWork);
            }
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private bool Login(User user)
        {
            if (user.GetType() == typeof(Doctor))
            {
                DoctorWindowViewModel doctorWindowService = new DoctorWindowViewModel(
                    (Doctor)user,
                    new ReferralsService(
                        new ReferralRepository(),
                        new AppointmentRepository()),
                    new ReferralRepository(),
                    new AppointmentRepository(),
                    new AppointmentService(
                        new AppointmentRepository(),
                        new AppointmentChangeRequestRepository(),
                        new AppointmentChangeRequestService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository())),
                    _roomService, _medicineCreationRequestService, new MedicineCreationRequestRepository());
                Close();
            }
            else if (user.GetType() == typeof(Manager))
            {
                ShowWindow(new CrudHospitalRoomWindow((Manager)user, new NotificationService(new NotificationRepository()), _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
            }
            else if (user.GetType() == typeof(Patient))
            {
                Patient patient = (Patient)user;
                if (patient.IsBlocked)
                {
                    MessageBox.Show("This user is blocked");
                    usernameTextBox.Clear();
                    passwordBox.Clear();
                    return false;
                }

                NavigationStore navStore = NavigationStore.GetInstance();
                navStore.CurrentViewModel = new MyAppointmentsViewModel(
                    new AppointmentService(
                        new AppointmentRepository(),
                        new AppointmentChangeRequestRepository(),
                        new AppointmentChangeRequestService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository())),
                    patient,
                    navStore);
                GUI.Patient.MainWindow win = new GUI.Patient.MainWindow()
                {
                    DataContext = new MainViewModel(navStore, patient, new NotificationService(new NotificationRepository()))
                };
                ShowWindow(win);
            }
            else if (user.GetType() == typeof(Core.Users.Models.Secretary))
            {
                ShowWindow(new SecretaryWindow(user, new NotificationService(new NotificationRepository()), _dynamicEquipmentService));
            }
            return true;
        }

        private void TryLogin()
        {
            bool foundUser = false;
            foreach (User user in UserRepository.Users)
            {
                if (user.Username != usernameTextBox.Text)
                {
                    continue;
                }
                foundUser = true;
                if (user.Password == passwordBox.Password)
                {
                    if (!Login(user))
                        return;
                }
                else
                {
                    passwordBox.Clear();
                    MessageBox.Show("Invalid password.");
                }
            }
            if (!foundUser)
            {
                MessageBox.Show("Invalid username.");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TryLogin();
            }
        }
    }
}