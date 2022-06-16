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
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients.Services;
using HealthCareCenter.Core.Prescriptions;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Surveys.Repositories;
using HealthCareCenter.Core.Users.Services;

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
        private readonly BaseUserRepository _userRepository;

        private BaseMedicineCreationRequestRepository medicineCreationRequestRepository = new MedicineCreationRequestRepository();

        public LoginWindow(IDynamicEquipmentService dynamicEquipmentService, BaseUserRepository userRepository) : this()
        {
            _dynamicEquipmentService = dynamicEquipmentService;
            _userRepository = userRepository;
        }

        public LoginWindow()
        {
            _userRepository = new UserRepository();
            IRoomService roomService = new RoomService(
                    new StorageRepository(),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository()),
                    new HospitalRoomForRenovationService(
                        new HospitalRoomForRenovationRepository()),
                    new HospitalRoomService(
                        new AppointmentRepository(),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomRepository()));
            IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService = new HospitalRoomUnderConstructionService(new HospitalRoomUnderConstructionRepository());
            IEquipmentRearrangementService equipmentRearrangementService = new EquipmentRearrangementService(
                    new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository())),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository()));
            IRenovationScheduleService renovationScheduleService = new RenovationScheduleService(
                new RoomService(
                    new StorageRepository(),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository()),
                    new HospitalRoomForRenovationService(
                        new HospitalRoomForRenovationRepository()),
                    new HospitalRoomService(
                        new AppointmentRepository(),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomRepository())),
                new HospitalRoomUnderConstructionService(
                    new HospitalRoomUnderConstructionRepository()),
                new HospitalRoomForRenovationService(
                    new HospitalRoomForRenovationRepository()),
                new RenovationScheduleRepository(),
                new HospitalRoomService(
                    new AppointmentRepository(),
                    new HospitalRoomForRenovationService(
                        new HospitalRoomForRenovationRepository()),
                    new HospitalRoomRepository()));
            IDoctorSurveyRatingService doctorSurveyRatingService = new DoctorSurveyRatingService(new DoctorSurveyRatingRepository(), new UserRepository());
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
            DoEquipmentRearrangements(equipmentRearrangementService, new EquipmentService(new EquipmentRepository()));
            FinshPossibleRenovation();

            try
            {
                _userRepository.LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            StartBackgroundWorkerIfNeeded();
        }

        private void DoEquipmentRearrangements(
            IEquipmentRearrangementService equipmentRearrangementService,
            IEquipmentService equipmentService)
        {
            List<Equipment> equipments = equipmentService.GetEquipments();
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
                    new ReferralService(
                        new ReferralRepository(),
                        new AppointmentRepository(),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository()),
                        new UserRepository(),
                        new HospitalRoomRepository()),
                    new ReferralRepository(),
                    new AppointmentRepository(),
                    new AppointmentService(
                        new AppointmentRepository(),
                        new AppointmentChangeRequestRepository(),
                        new AppointmentChangeRequestService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository(),
                            new HospitalRoomService(
                                new AppointmentRepository(),
                                new HospitalRoomForRenovationService(
                                    new HospitalRoomForRenovationRepository()),
                                new HospitalRoomRepository()),
                            new UserRepository()),
                        new PatientService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository(),
                            new HealthRecordRepository(),
                            new HealthRecordService(
                                new HealthRecordRepository()),
                            new PatientEditService(
                                new HealthRecordRepository(),
                                new UserRepository()),
                            new UserRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository()),
                        new HospitalRoomRepository()),
                    new RoomService(
                    new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository())),
                    new MedicineCreationRequestService(
                        new MedicineCreationRequestRepository()),
                    new MedicineCreationRequestRepository(),
                    new HealthRecordService(
                        new HealthRecordRepository()),
                    new DoctorService(
                        new DoctorSearchService(
                            new UserRepository()),
                        new UserRepository()));
                Close();
            }
            else if (user.GetType() == typeof(Manager))
            {
                ShowWindow(new CrudHospitalRoomWindow(
                (Manager)user,
                new NotificationService(
                    new NotificationRepository(),
                    new HealthRecordService(
                        new HealthRecordRepository()),
                    new MedicineInstructionService(
                            new MedicineInstructionRepository()),
                        new MedicineService(
                            new MedicineRepository()),
                    new HospitalRoomService(
                        new AppointmentRepository(),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomRepository())),
                new RoomService(
                    new StorageRepository(),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository()),
                    new HospitalRoomForRenovationService(
                        new HospitalRoomForRenovationRepository()),
                    new HospitalRoomService(
                        new AppointmentRepository(),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomRepository())),
                new HospitalRoomUnderConstructionService(
                    new HospitalRoomUnderConstructionRepository()),
                new HospitalRoomForRenovationService(
                    new HospitalRoomForRenovationRepository()),
                new RenovationScheduleService(
                    new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository())),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository()),
                    new HospitalRoomForRenovationService(
                        new HospitalRoomForRenovationRepository()),
                    new RenovationScheduleRepository(),
                    new HospitalRoomService(
                        new AppointmentRepository(),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomRepository())),
                new EquipmentRearrangementService(
                    new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository())),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository())),
                new DoctorSurveyRatingService(new DoctorSurveyRatingRepository(), new UserRepository()),
                new MedicineCreationRequestService(
                    new MedicineCreationRequestRepository())));
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
                            new AppointmentChangeRequestRepository(),
                            new HospitalRoomService(
                                new AppointmentRepository(),
                                new HospitalRoomForRenovationService(
                                    new HospitalRoomForRenovationRepository()),
                                new HospitalRoomRepository()),
                            new UserRepository()),
                        new PatientService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository(),
                            new HealthRecordRepository(),
                            new HealthRecordService(
                                new HealthRecordRepository()),
                            new PatientEditService(
                                new HealthRecordRepository(),
                                new UserRepository()),
                            new UserRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository()),
                        new HospitalRoomRepository()),
                    patient,
                    navStore);
                GUI.Patient.MainWindow win = new GUI.Patient.MainWindow()
                {
                    DataContext = new MainViewModel(
                        new NotificationService(
                            new NotificationRepository(),
                            new HealthRecordService(
                                new HealthRecordRepository()),
                            new MedicineInstructionService(
                                new MedicineInstructionRepository()),
                            new MedicineService(
                                new MedicineRepository()),
                            new HospitalRoomService(
                                new AppointmentRepository(),
                                new HospitalRoomForRenovationService(
                                    new HospitalRoomForRenovationRepository()),
                                new HospitalRoomRepository())),
                        new PrescriptionService(
                            new MedicineInstructionRepository(),
                            new PrescriptionRepository()),
                        patient,
                        navStore)
                };
                ShowWindow(win);
            }
            else if (user.GetType() == typeof(Core.Users.Models.Secretary))
            {
                ShowWindow(new SecretaryWindow(
                    user, 
                    new NotificationService(
                        new NotificationRepository(),
                        new HealthRecordService(
                            new HealthRecordRepository()),
                        new MedicineInstructionService(
                            new MedicineInstructionRepository()),
                        new MedicineService(
                            new MedicineRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository())),
                    _dynamicEquipmentService));
            }
            return true;
        }

        private void TryLogin()
        {
            bool foundUser = false;
            foreach (User user in _userRepository.Users)
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