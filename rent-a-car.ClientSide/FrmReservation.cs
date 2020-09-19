using rent_a_car.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rent_a_car.ClientSide
{
    public partial class FrmReservation : Form
    {
        BindingList<Reservation> listaRezervacija = new BindingList<Reservation>(); 
        public FrmReservation()
        {
            InitializeComponent();
            dgvAutomobili1.DataSource = listaRezervacija; 
        }
        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Load_FrmReservation(object sender,EventArgs e)
        {
            UIController.Instance.LoadListOfCars(dgvAutomobili);

        }

        private void search_Click(object sender, EventArgs e)
        {
            UIController.Instance.SearchCars(txtMarka, txtModel, dgvAutomobili);
        }
        private void resetuj_Click(object sender, EventArgs e)
        {
            UIController.Instance.LoadListOfCars(dgvAutomobili);
        }

        private void viewCar_click(object sender, EventArgs e)
        {
            Car car = UIController.Instance.GetCarFromDgv(dgvAutomobili);

            if (car == null)
            {
                MessageBox.Show("Morate selektovati red u tabeli!");
                return;
            }

            FrmPrikaziAutomobil frmPrikaziAutomobil = new FrmPrikaziAutomobil(car);
            frmPrikaziAutomobil.ShowDialog();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (!ProveraOpste())
            {
                MessageBox.Show("Zaboravili ste da unesete neku vrednost");
                return; 
            }
            Reservation rezervacija = new Reservation();
            rezervacija.DateOfReservation = DateTime.Now;
            rezervacija.DateFrom = dateTimePickerDatumOd.Value;
            rezervacija.DateTo = dateTimePickerDatumDo.Value;
            rezervacija.LocationFrom = txtPreuzimanje.Text;
            rezervacija.LocationTo = txtVracanje.Text; 
            rezervacija.Car = (Car)dgvAutomobili.SelectedRows[0].DataBoundItem;
            rezervacija.User = Sesion.Instance.User;
            listaRezervacija.Add(rezervacija);
            dgvAutomobili1.DataSource = listaRezervacija; 


        }

        private bool ProveraOpste()
        {
            int brojac = 0;
            if (!ProveraAuto()) brojac++;
            if (!ProveraLokacijaPreu()) brojac++;
            if (!ProveraLokacijaVrac()) brojac++;
            return brojac == 0;
        }

        private bool ProveraAuto()
        {
            try
            {
                Car c = (Car)dgvAutomobili.SelectedRows[0].DataBoundItem;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool ProveraLokacijaPreu()
        {
            return txtPreuzimanje.Text != ""; 
        }
        private bool ProveraLokacijaVrac()
        {
            return txtVracanje.Text != "";
        }
        private void btnRezervisi_Click(object sender, EventArgs e)
        {
            if(listaRezervacija.Count == 0 || listaRezervacija is null)
            {
                MessageBox.Show("Niste ubacili rezervaciju!");
                return; 
            }
            UIController.Instance.CreateReservation(listaRezervacija); 
        }
    }
}
