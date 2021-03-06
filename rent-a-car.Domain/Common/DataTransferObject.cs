﻿using System;
using System.Collections.Generic;
using System.Text;

namespace rent_a_car.Domain
{
    [Serializable]
    public class DataTransferObject
    {
        public Operation Operation { get; set; }
        public DomainObject Object { get; set; }
        public object Result { get; set; }

        public object ListaObj { get; set; }
    }
    public enum Operation
    {
        SignIn,
        SignUp,
        CreateCar,
        CreateReservation,
        DeleteReservation,
        DeleteCar,
        FindCar,
        FindReservation,
        LogOut,
        GetAllCars,
        GetAllReservations,
        FindByIDCar,
        FindByIDReservation
    }
}
