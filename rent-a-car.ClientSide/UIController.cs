﻿using rent_a_car.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace rent_a_car.ClientSide
{
    internal class UIController
    {

        private static UIController instance;

        private UIController()
        {

        }

        public static UIController Instance
        {
            get
            {
                if (instance == null)
                    instance = new UIController();
                return instance;
            }
        }

        public bool ConnectToServer()
        {
            if (NetworkCommunication.Instance.ConnectToServer())
            {
                return true;
            }
            return false;
        }

        internal bool CloseReservation(DataGridView dgvRezervacije)
        {
            if (dgvRezervacije.SelectedRows.Count == 0 || dgvRezervacije.SelectedRows[0] == null)
            {
                MessageBox.Show("Morate odabrati rezervaciju za otkaz!");
                return false;
            }
            if (dgvRezervacije.SelectedRows[0].DataBoundItem is Reservation reservation)
            {
                reservation.CONDITIONS = $"ReservationID == {reservation.ReservationID}";

                bool deletedReservation = NetworkCommunication.Instance.DeleteReservation(reservation);

                return deletedReservation;
            }
            else
            {
                MessageBox.Show("Greska kod kastovanja reda tabele!");
            }
            return false;
        }

        internal void SearchCars(TextBox txtMarka, TextBox txtModel, DataGridView dgvAutomobili)
        {
            string criteriaBrend = txtMarka.Text;
            string criteriaModel = txtModel.Text;

            Car car = new Car();

            car.CONDITIONS = $"Brand LIKE '%{criteriaBrend}%' OR Model LIKE '%{criteriaModel}%'";

            List<Car> cars = (List<Car>)NetworkCommunication.Instance.GetCarsByConditions(car);

            if(cars == null)
            {
                MessageBox.Show("Nema automobila za kriterijum!");
                return;
            }
            MessageBox.Show("Sistem je pronasao sledece automobile za kriterijum");
            dgvAutomobili.DataSource = new BindingList<Car>(cars);
        }

        internal Car GetCarFromDgv(DataGridView dgvAutomobili)
        {
            if (dgvAutomobili.Rows.Count == 0 || dgvAutomobili.SelectedRows[0] == null)
            {
                MessageBox.Show("Morate odabrati polazak za prikaz detalja!");
                return null;
            }
            if (dgvAutomobili.SelectedRows[0].DataBoundItem is Car car)
            {
                return car;
            }
            else
            {
                MessageBox.Show("Greska kod kastovanja reda tabele!");
            }
            return null;
        }

        internal void LoadCar(int carID, Label lblMarka, Label lblModel)
        {
            Car carForDisplaying = new Car
            {
                CarID = carID
            };

            Car foundedCar = NetworkCommunication.Instance.GetCarByID(carForDisplaying);

            if(foundedCar == null)
            {
                MessageBox.Show("Nije moguce pronaci automobil!");
                return;
            }
            lblMarka.Text = foundedCar.Brand.ToString() ;
            lblModel.Text = foundedCar.Model.ToString();

        }

        internal bool LoadListOfReservation(DataGridView dgvReservation)
        {
            List<Reservation> reservations = (List<Reservation>)NetworkCommunication.Instance.LoadReservation();

            if (reservations == null)
            {
                MessageBox.Show("Error while loading reservations!");
                dgvReservation.DataSource = new BindingList<Reservation>();
                return false;
            }
            MessageBox.Show("System has loaded reservations!");
            dgvReservation.DataSource = new BindingList<Reservation>(reservations);
            return true;
        }


        internal bool LoadListOfCars(DataGridView dgvAutomobili)
        {
            List<Car> cars = (List<Car>)NetworkCommunication.Instance.LoadCars();

            if(cars == null)
            {
                MessageBox.Show("Error while loading cars!");
                dgvAutomobili.DataSource = new BindingList<Car>();
                return false;
            }
            MessageBox.Show("System has loaded cars!");
            dgvAutomobili.DataSource = new BindingList<Car>(cars);
            return true;
        }

        internal bool CreateReservation(BindingList<Reservation> listaRezervacija)
        {
            List<Reservation> rezerv = (List<Reservation>)NetworkCommunication.Instance.CreateReservation(listaRezervacija);
            if(rezerv is null)
            {
                MessageBox.Show("Doslo je do greske prilikom rezervacije!");
                return false; 
            }
            MessageBox.Show("Uspesno ste rezervisali automobil!");
            return true;
        }

        internal bool CreateCar(TextBox txtMarka, TextBox txtModel)
        {
            if (string.IsNullOrEmpty(txtMarka.Text.ToString()) ||
                string.IsNullOrEmpty(txtModel.Text.ToString()))
            {
                return false;
            }

            Car carForCreate = new Car
            {
                Model = txtModel.Text.ToString(),
                Brand = txtMarka.Text.ToString()
            };

            Car car = (Car)NetworkCommunication.Instance.CreateCar(carForCreate);

            if (car == null)
                return false;

            return true;
        }

        internal bool SignUp(TextBox txtIme, TextBox txtKorIme, TextBox txtLozinka, TextBox txtLozinka2, TextBox txtTelefon)
        {
            if (string.IsNullOrEmpty(txtIme.Text.ToString()) ||
                string.IsNullOrEmpty(txtKorIme.Text.ToString()) ||
                string.IsNullOrEmpty(txtLozinka.Text.ToString()) ||
                string.IsNullOrEmpty(txtLozinka2.Text.ToString()) ||
                string.IsNullOrEmpty(txtTelefon.Text.ToString()))
            {
                return false;
            }
            if(txtLozinka.ToString() != txtLozinka2.ToString())
            {
                return false;
            }

            User userForSignUp = new User
            {
                Name = txtKorIme.Text.ToString(),
                Phone = txtTelefon.Text.ToString(),
                Password = txtLozinka.Text.ToString()
            };

            User user = (User)NetworkCommunication.Instance.SignUp(userForSignUp);
            
            if(user == null)
            {
                txtIme = null;
                txtKorIme = null;
                txtLozinka = null;
                txtLozinka2 = null;
                txtTelefon = null;
                return false;
            }

            return true;
        }

        internal bool SignIn(TextBox txtKorIme, TextBox txtLozinka)
        {
        //    Sesion.Instance.User.UserType.TypeName = "korisnik";
          //  return true;


            if(string.IsNullOrEmpty(txtKorIme.Text) || string.IsNullOrEmpty(txtLozinka.Text))
            {
                MessageBox.Show("Sva polja su obavezna!");
                return false;
            }

            User logInUser = new User
            {
                Name = txtKorIme.Text,
                Password = txtLozinka.Text
            };

            User user = (User)NetworkCommunication.Instance.SignIn(logInUser);

            if(user == null)
            {
                txtKorIme.Text = null;
                txtLozinka.Text = null;
                return false;
            }

            Sesion.Instance.User = user;

            return true;

        }
   
    }
}
